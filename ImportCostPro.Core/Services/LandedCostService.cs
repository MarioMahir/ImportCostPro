using ImportCostPro.Core.Data;
using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Enums;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    // Calculo del landed cost segun el documento funcional: validaciones previas, conversion a
    // moneda local, prorrateo de gastos por FOB/peso/volumen/cantidad, CIF, arancel, impuesto
    // selectivo, tasa de servicio aduanal, ITBIS, gastos locales, costo unitario y precio sugerido.
    // Todos los calculos se hacen con decimal y precision completa; el redondeo es solo visual.
    public class LandedCostService : ILandedCostService
    {
        private readonly AppDbContext _context;

        public LandedCostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrdenImportacion>> ObtenerOrdenesCalculablesAsync()
        {
            return await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.Moneda)
                .Where(o => o.Estado == EstadoOrden.Abierta)
                .OrderByDescending(o => o.Fecha)
                .ThenByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<LandedCostCalculoDto> CalcularAsync(int ordenId)
        {
            var orden = await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.Moneda)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(p => p!.CategoriaArancelaria)
                .Include(o => o.Gastos)
                    .ThenInclude(g => g.Moneda)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == ordenId);

            if (orden == null)
            {
                throw new InvalidOperationException("La orden de importación no existe.");
            }

            if (orden.Estado != EstadoOrden.Abierta)
            {
                throw new InvalidOperationException(
                    $"No se puede calcular el landed cost porque la orden está en estado {orden.Estado}. Solo se calculan órdenes abiertas.");
            }

            if (!orden.Detalles.Any())
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque la orden no tiene productos agregados.");
            }

            if (orden.Moneda == null)
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque la orden no tiene una moneda válida.");
            }

            var monedaLocal = await _context.Monedas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EsMonedaLocal && m.Estado);

            if (monedaLocal == null)
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque no existe una moneda local activa configurada en el mantenimiento de monedas.");
            }

            var configuracion = await _context.ConfiguracionesImpuestos
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (configuracion == null)
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque no existe una configuración de impuestos registrada.");
            }

            if (!orden.Gastos.Any(g => g.TipoGasto == TipoGasto.FleteInternacional))
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque la orden no tiene un gasto de tipo Flete internacional registrado.");
            }

            if (!orden.Gastos.Any(g => g.TipoGasto == TipoGasto.SeguroInternacional))
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque la orden no tiene un gasto de tipo Seguro internacional registrado.");
            }

            foreach (var detalle in orden.Detalles)
            {
                if (detalle.Producto == null)
                {
                    throw new InvalidOperationException("No se puede calcular el landed cost porque un producto de la orden ya no existe.");
                }

                if (detalle.Producto.CategoriaArancelaria == null)
                {
                    throw new InvalidOperationException($"No se puede calcular el landed cost porque el producto {detalle.Producto.Nombre} no tiene una categoría arancelaria válida.");
                }

                if (detalle.Cantidad <= 0)
                {
                    throw new InvalidOperationException($"No se puede calcular el landed cost porque el producto {detalle.Producto.Nombre} tiene cantidad 0.");
                }

                if (detalle.MargenDeseado < 0 || detalle.MargenDeseado >= 100)
                {
                    throw new InvalidOperationException($"No se puede calcular el landed cost porque el margen deseado del producto {detalle.Producto.Nombre} debe ser mayor o igual que 0 y menor que 100.");
                }
            }

            // Paso 1 y 2: FOB por producto y conversion a moneda local con la tasa vigente a la fecha de la orden.
            var tasaOrden = await ObtenerTasaAsync(orden.MonedaId, orden.Moneda.CodigoISO, monedaLocal, orden.Fecha,
                $"la orden está registrada en {orden.Moneda.CodigoISO} y no existe una tasa de cambio activa hacia la moneda local para la fecha de la orden");

            var resultado = new LandedCostCalculoDto
            {
                OrdenId = orden.Id,
                FechaOrden = orden.Fecha,
                Importador = orden.Importador?.Nombre ?? string.Empty,
                Proveedor = orden.Proveedor?.Nombre ?? string.Empty,
                MonedaOrden = orden.Moneda.CodigoISO,
                MonedaLocal = monedaLocal.CodigoISO,
                MonedaLocalId = monedaLocal.Id,
                TasaCambioOrden = tasaOrden,
                PorcentajeITBIS = configuracion.PorcentajeITBIS,
                PorcentajeTasaServicioAduanal = configuracion.PorcentajeTasaServicioAduanal
            };

            // Cada linea del resultado se empareja con su detalle de origen por posicion.
            var lineas = new List<(DetalleOrdenImportacion Detalle, LandedCostDetalleDto Linea)>();
            foreach (var detalle in orden.Detalles)
            {
                var fob = detalle.Cantidad * detalle.PrecioUnitario;
                var linea = new LandedCostDetalleDto
                {
                    ProductoId = detalle.ProductoId,
                    Producto = detalle.Producto!.Nombre,
                    Cantidad = detalle.Cantidad,
                    FOBOriginal = fob,
                    FOBLocal = fob * tasaOrden,
                    PorcentajeArancel = detalle.Producto.CategoriaArancelaria!.PorcentajeArancel,
                    PorcentajeImpuestoSelectivo = detalle.Producto.CategoriaArancelaria.AplicaImpuestoSelectivo
                        ? detalle.Producto.CategoriaArancelaria.PorcentajeImpuestoSelectivo
                        : 0,
                    MargenDeseado = detalle.MargenDeseado
                };
                resultado.Detalles.Add(linea);
                lineas.Add((detalle, linea));
            }

            resultado.FOBTotalOriginal = resultado.Detalles.Sum(d => d.FOBOriginal);
            resultado.FOBTotalLocal = resultado.Detalles.Sum(d => d.FOBLocal);
            resultado.CantidadTotalImportada = resultado.Detalles.Sum(d => d.Cantidad);

            if (resultado.FOBTotalOriginal <= 0)
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque el FOB total de la orden es 0.");
            }

            // Paso 3: cada gasto a moneda local con la tasa vigente a la fecha del gasto.
            var gastosLocales = new List<(GastoImportacion Gasto, decimal MontoLocal)>();
            foreach (var gasto in orden.Gastos)
            {
                if (gasto.Moneda == null)
                {
                    throw new InvalidOperationException($"No se puede calcular el landed cost porque el gasto {DescribirGasto(gasto.TipoGasto)} no tiene una moneda válida.");
                }

                if (gasto.Monto <= 0)
                {
                    throw new InvalidOperationException($"No se puede calcular el landed cost porque el gasto {DescribirGasto(gasto.TipoGasto)} tiene monto 0.");
                }

                var tasaGasto = await ObtenerTasaAsync(gasto.MonedaId, gasto.Moneda.CodigoISO, monedaLocal, gasto.Fecha,
                    $"el gasto {DescribirGasto(gasto.TipoGasto)} está en {gasto.Moneda.CodigoISO} y no existe una tasa de cambio activa hacia la moneda local para la fecha del gasto");

                gastosLocales.Add((gasto, gasto.Monto * tasaGasto));
            }

            // Paso 4: bases de distribucion, validadas solo si algun gasto las necesita.
            var metodosUsados = orden.Gastos.Select(g => g.MetodoDistribucion).Distinct().ToList();
            var pesoTotal = orden.Detalles.Sum(d => d.Cantidad * d.Producto!.PesoUnitario);
            var volumenTotal = orden.Detalles.Sum(d => d.Cantidad * (d.Producto!.Largo ?? 0) * (d.Producto.Ancho ?? 0) * (d.Producto.Alto ?? 0));
            var cantidadTotal = resultado.CantidadTotalImportada;

            if (metodosUsados.Contains(MetodoDistribucion.PorPeso) &&
                (pesoTotal <= 0 || orden.Detalles.Any(d => d.Producto!.PesoUnitario <= 0)))
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque existen gastos distribuidos por peso y uno o más productos no tienen peso configurado.");
            }

            if (metodosUsados.Contains(MetodoDistribucion.PorVolumen) &&
                (volumenTotal <= 0 || orden.Detalles.Any(d => (d.Producto!.Largo ?? 0) <= 0 || (d.Producto.Ancho ?? 0) <= 0 || (d.Producto.Alto ?? 0) <= 0)))
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque existen gastos distribuidos por volumen y uno o más productos no tienen largo, ancho y alto configurados.");
            }

            if (metodosUsados.Contains(MetodoDistribucion.PorCantidad) && cantidadTotal <= 0)
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque existen gastos distribuidos por cantidad y la cantidad total de la orden es 0.");
            }

            if (metodosUsados.Contains(MetodoDistribucion.PorValorFOB) && resultado.FOBTotalLocal <= 0)
            {
                throw new InvalidOperationException("No se puede calcular el landed cost porque existen gastos distribuidos por valor FOB y el FOB total de la orden es 0.");
            }

            // Paso 5 y 6: prorratear cada gasto y separar flete, seguro y gastos locales.
            foreach (var (gasto, montoLocal) in gastosLocales)
            {
                switch (gasto.TipoGasto)
                {
                    case TipoGasto.FleteInternacional:
                        resultado.FleteTotal += montoLocal;
                        break;
                    case TipoGasto.SeguroInternacional:
                        resultado.SeguroTotal += montoLocal;
                        break;
                    default:
                        resultado.TotalGastosLocales += montoLocal;
                        break;
                }

                foreach (var (detalle, linea) in lineas)
                {
                    var factor = gasto.MetodoDistribucion switch
                    {
                        MetodoDistribucion.PorPeso => detalle.Cantidad * detalle.Producto!.PesoUnitario / pesoTotal,
                        MetodoDistribucion.PorVolumen => detalle.Cantidad * (detalle.Producto!.Largo ?? 0) * (detalle.Producto.Ancho ?? 0) * (detalle.Producto.Alto ?? 0) / volumenTotal,
                        MetodoDistribucion.PorCantidad => (decimal)detalle.Cantidad / cantidadTotal,
                        _ => linea.FOBLocal / resultado.FOBTotalLocal
                    };

                    var asignado = montoLocal * factor;
                    switch (gasto.TipoGasto)
                    {
                        case TipoGasto.FleteInternacional:
                            linea.FleteAsignado += asignado;
                            break;
                        case TipoGasto.SeguroInternacional:
                            linea.SeguroAsignado += asignado;
                            break;
                        default:
                            linea.GastosLocalesAsignados += asignado;
                            break;
                    }
                }
            }

            // Pasos 7 a 14 por producto.
            foreach (var (detalle, linea) in lineas)
            {
                linea.CIF = linea.FOBLocal + linea.FleteAsignado + linea.SeguroAsignado;
                linea.Arancel = linea.CIF * (linea.PorcentajeArancel / 100m);
                linea.ImpuestoSelectivo = linea.CIF * (linea.PorcentajeImpuestoSelectivo / 100m);
                linea.TasaServicioAduanal = linea.CIF * (resultado.PorcentajeTasaServicioAduanal / 100m);

                if (detalle.Producto!.CategoriaArancelaria!.AplicaITBIS)
                {
                    var baseItbis = linea.CIF + linea.Arancel + linea.ImpuestoSelectivo + linea.TasaServicioAduanal;
                    linea.ITBIS = baseItbis * (resultado.PorcentajeITBIS / 100m);
                }

                linea.CostoTotalImportado = linea.FOBLocal + linea.FleteAsignado + linea.SeguroAsignado +
                    linea.Arancel + linea.ImpuestoSelectivo + linea.TasaServicioAduanal + linea.ITBIS + linea.GastosLocalesAsignados;
                linea.CostoUnitarioImportado = linea.CostoTotalImportado / linea.Cantidad;
                linea.PrecioVentaSugerido = linea.CostoUnitarioImportado / (1 - (linea.MargenDeseado / 100m));
            }

            resultado.CIFTotal = resultado.Detalles.Sum(d => d.CIF);
            resultado.TotalArancel = resultado.Detalles.Sum(d => d.Arancel);
            resultado.TotalImpuestoSelectivo = resultado.Detalles.Sum(d => d.ImpuestoSelectivo);
            resultado.TotalTasaServicioAduanal = resultado.Detalles.Sum(d => d.TasaServicioAduanal);
            resultado.TotalITBIS = resultado.Detalles.Sum(d => d.ITBIS);
            resultado.CostoTotalImportacion = resultado.Detalles.Sum(d => d.CostoTotalImportado);

            return resultado;
        }

        public async Task<ResultadoLandedCost> GuardarCalculoOficialAsync(int ordenId)
        {
            var yaExiste = await _context.ResultadosLandedCost.AnyAsync(r => r.OrdenImportacionId == ordenId);
            if (yaExiste)
            {
                throw new InvalidOperationException("Esta orden ya tiene un cálculo oficial de landed cost guardado. No se permite más de uno por orden.");
            }

            var calculo = await CalcularAsync(ordenId);

            var resultado = new ResultadoLandedCost
            {
                OrdenImportacionId = ordenId,
                FechaCalculo = DateTime.Now,
                MonedaLocalId = calculo.MonedaLocalId,
                TasaCambioOrden = calculo.TasaCambioOrden,
                PorcentajeITBIS = calculo.PorcentajeITBIS,
                PorcentajeTasaServicioAduanal = calculo.PorcentajeTasaServicioAduanal,
                FOBTotalOriginal = calculo.FOBTotalOriginal,
                FOBTotalLocal = calculo.FOBTotalLocal,
                FleteTotal = calculo.FleteTotal,
                SeguroTotal = calculo.SeguroTotal,
                CIFTotal = calculo.CIFTotal,
                TotalArancel = calculo.TotalArancel,
                TotalImpuestoSelectivo = calculo.TotalImpuestoSelectivo,
                TotalTasaServicioAduanal = calculo.TotalTasaServicioAduanal,
                TotalITBIS = calculo.TotalITBIS,
                TotalGastosLocales = calculo.TotalGastosLocales,
                CostoTotalImportacion = calculo.CostoTotalImportacion,
                CantidadTotalImportada = calculo.CantidadTotalImportada,
                Detalles = calculo.Detalles.Select(d => new DetalleResultadoLandedCost
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    FOBOriginal = d.FOBOriginal,
                    FOBLocal = d.FOBLocal,
                    FleteAsignado = d.FleteAsignado,
                    SeguroAsignado = d.SeguroAsignado,
                    CIF = d.CIF,
                    PorcentajeArancel = d.PorcentajeArancel,
                    PorcentajeImpuestoSelectivo = d.PorcentajeImpuestoSelectivo,
                    Arancel = d.Arancel,
                    ImpuestoSelectivo = d.ImpuestoSelectivo,
                    TasaServicioAduanal = d.TasaServicioAduanal,
                    ITBIS = d.ITBIS,
                    GastosLocalesAsignados = d.GastosLocalesAsignados,
                    CostoTotalImportado = d.CostoTotalImportado,
                    CostoUnitarioImportado = d.CostoUnitarioImportado,
                    MargenDeseado = d.MargenDeseado,
                    PrecioVentaSugerido = d.PrecioVentaSugerido
                }).ToList()
            };

            var orden = await _context.OrdenesImportacion.FirstAsync(o => o.Id == ordenId);
            orden.Estado = EstadoOrden.Calculada;

            _context.ResultadosLandedCost.Add(resultado);
            await _context.SaveChangesAsync();

            return resultado;
        }

        public async Task<ResultadoLandedCost?> ObtenerResultadoOficialAsync(int ordenId)
        {
            return await _context.ResultadosLandedCost
                .Include(r => r.MonedaLocal)
                .Include(r => r.OrdenImportacion)
                    .ThenInclude(o => o!.Moneda)
                .Include(r => r.OrdenImportacion)
                    .ThenInclude(o => o!.Importador)
                .Include(r => r.OrdenImportacion)
                    .ThenInclude(o => o!.Proveedor)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.Producto)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.OrdenImportacionId == ordenId);
        }

        // Tasa 1 si la moneda ya es la local; si no, la tasa activa mas reciente con vigencia <= fecha.
        private async Task<decimal> ObtenerTasaAsync(int monedaId, string codigoMoneda, Moneda monedaLocal, DateTime fecha, string motivoError)
        {
            if (monedaId == monedaLocal.Id)
            {
                return 1m;
            }

            var tasa = await _context.TasasCambio
                .AsNoTracking()
                .Where(t => t.Estado &&
                            t.MonedaOrigenId == monedaId &&
                            t.MonedaDestinoId == monedaLocal.Id &&
                            t.FechaVigencia.Date <= fecha.Date)
                .OrderByDescending(t => t.FechaVigencia)
                .ThenByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            if (tasa == null || tasa.ValorTasa <= 0)
            {
                throw new InvalidOperationException($"No se puede calcular el landed cost porque {motivoError} ({codigoMoneda} → {monedaLocal.CodigoISO}).");
            }

            return tasa.ValorTasa;
        }

        private static string DescribirGasto(TipoGasto tipo) => tipo switch
        {
            TipoGasto.FleteInternacional => "de flete internacional",
            TipoGasto.SeguroInternacional => "de seguro internacional",
            TipoGasto.GastosPortuarios => "de gastos portuarios",
            TipoGasto.TransporteLocal => "de transporte local",
            TipoGasto.HonorariosAduanales => "de honorarios aduanales",
            TipoGasto.Almacenaje => "de almacenaje",
            TipoGasto.ManejoCarga => "de manejo de carga",
            _ => "de otros gastos"
        };
    }
}
