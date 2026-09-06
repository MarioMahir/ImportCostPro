using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Enums;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class OrdenImportacionService : IOrdenImportacionService
    {
        private readonly AppDbContext _context;

        public OrdenImportacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrdenImportacion>> ObtenerTodasAsync()
        {
            return await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.Moneda)
                .OrderByDescending(o => o.Fecha)
                .ThenByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<List<OrdenImportacion>> ObtenerAbiertasAsync()
        {
            return await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Where(o => o.Estado == EstadoOrden.Abierta)
                .OrderByDescending(o => o.Fecha)
                .ThenByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<OrdenImportacion?> ObtenerPorIdAsync(int id)
        {
            return await _context.OrdenesImportacion
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrdenImportacion?> ObtenerCompletaAsync(int id)
        {
            return await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.Moneda)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(o => o.Gastos)
                    .ThenInclude(g => g.Moneda)
                .Include(o => o.ResultadoOficial)
                    .ThenInclude(r => r!.MonedaLocal)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task CrearAsync(OrdenImportacion orden)
        {
            await ValidarReferenciasAsync(orden);
            orden.Estado = EstadoOrden.Abierta;
            _context.OrdenesImportacion.Add(orden);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(OrdenImportacion orden)
        {
            var actual = await _context.OrdenesImportacion.FindAsync(orden.Id);
            if (actual == null)
            {
                throw new InvalidOperationException("La orden de importación no existe.");
            }

            if (actual.Estado != EstadoOrden.Abierta)
            {
                throw new InvalidOperationException(
                    $"No se puede editar la orden #{actual.Id} porque está en estado {actual.Estado}. Solo las órdenes abiertas admiten cambios.");
            }

            await ValidarReferenciasAsync(orden);

            actual.Fecha = orden.Fecha;
            actual.ImportadorId = orden.ImportadorId;
            actual.ProveedorId = orden.ProveedorId;
            actual.MonedaId = orden.MonedaId;
            await _context.SaveChangesAsync();
        }

        public async Task CerrarOrdenAsync(int id)
        {
            var orden = await _context.OrdenesImportacion
                .Include(o => o.Detalles)
                .Include(o => o.ResultadoOficial)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                throw new InvalidOperationException("La orden de importación no existe.");
            }

            if (orden.Estado == EstadoOrden.Cancelada)
            {
                throw new InvalidOperationException("No se puede cerrar una orden cancelada.");
            }

            if (orden.Estado != EstadoOrden.Calculada)
            {
                throw new InvalidOperationException("Solo se pueden cerrar órdenes en estado Calculada.");
            }

            if (orden.ResultadoOficial == null)
            {
                throw new InvalidOperationException("No se puede cerrar esta orden porque no tiene un cálculo oficial de landed cost guardado.");
            }

            if (!orden.Detalles.Any())
            {
                throw new InvalidOperationException("No se puede cerrar esta orden porque no tiene productos registrados.");
            }

            if (orden.ResultadoOficial.CostoTotalImportacion <= 0)
            {
                throw new InvalidOperationException("No se puede cerrar esta orden porque el cálculo oficial tiene un costo total de importación de 0.");
            }

            // El cierre solo cambia el estado: no recalcula ni toca el resultado oficial.
            orden.Estado = EstadoOrden.Cerrada;
            orden.FechaCierre = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task CancelarAsync(int id)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(id);
            if (orden == null)
            {
                throw new InvalidOperationException("La orden de importación no existe.");
            }

            if (orden.Estado == EstadoOrden.Cerrada)
            {
                throw new InvalidOperationException("Una orden cerrada no puede cancelarse.");
            }

            if (orden.Estado == EstadoOrden.Cancelada)
            {
                throw new InvalidOperationException("La orden ya está cancelada.");
            }

            orden.Estado = EstadoOrden.Cancelada;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var orden = await _context.OrdenesImportacion
                .Include(o => o.ResultadoOficial)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                throw new InvalidOperationException("La orden de importación no existe.");
            }

            if (orden.ResultadoOficial != null)
            {
                throw new InvalidOperationException("No se puede eliminar esta orden porque ya tiene un cálculo oficial de landed cost guardado. Si no procede, cancélela.");
            }

            if (orden.Estado == EstadoOrden.Cerrada)
            {
                throw new InvalidOperationException("No se puede eliminar una orden cerrada.");
            }

            var detalles = _context.DetallesOrdenImportacion.Where(d => d.OrdenImportacionId == id);
            var gastos = _context.GastosImportacion.Where(g => g.OrdenImportacionId == id);
            _context.DetallesOrdenImportacion.RemoveRange(detalles);
            _context.GastosImportacion.RemoveRange(gastos);
            _context.OrdenesImportacion.Remove(orden);
            await _context.SaveChangesAsync();
        }

        // Importador, proveedor y moneda deben existir y estar activos para registrar una orden.
        private async Task ValidarReferenciasAsync(OrdenImportacion orden)
        {
            var importador = await _context.Importadores.FindAsync(orden.ImportadorId);
            if (importador == null || !importador.Estado)
            {
                throw new InvalidOperationException("Debe seleccionar un importador activo.");
            }

            var proveedor = await _context.Proveedores.FindAsync(orden.ProveedorId);
            if (proveedor == null || !proveedor.Estado)
            {
                throw new InvalidOperationException("Debe seleccionar un proveedor activo.");
            }

            var moneda = await _context.Monedas.FindAsync(orden.MonedaId);
            if (moneda == null || !moneda.Estado)
            {
                throw new InvalidOperationException("Debe seleccionar una moneda activa.");
            }
        }
    }
}
