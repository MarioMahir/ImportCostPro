using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class TasaCambioService : ITasaCambioService
    {
        private readonly AppDbContext _context;

        public TasaCambioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteDuplicadaAsync(int origenId, int destinoId, DateTime fecha, int? excluirId = null)
        {
            var query = _context.TasasCambio
                .Where(t =>
                    t.MonedaOrigenId == origenId &&
                    t.MonedaDestinoId == destinoId &&
                    t.FechaVigencia.Date == fecha.Date &&
                    t.Estado);

            if (excluirId.HasValue)
            {
                query = query.Where(t => t.Id != excluirId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<List<TasaCambio>> ObtenerTodasAsync()
        {
            return await _context.TasasCambio
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .ToListAsync();
        }

        public async Task<TasaCambio?> ObtenerPorIdAsync(int id)
        {
            return await _context.TasasCambio
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CrearAsync(TasaCambio tasa)
        {
            _context.TasasCambio.Add(tasa);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(TasaCambio tasa)
        {
            _context.TasasCambio.Update(tasa);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tasa = await _context.TasasCambio.FindAsync(id);

            if (tasa != null)
            {
                // Una tasa pudo haberse usado en un calculo oficial: orden o gasto en su moneda origen
                // con fecha igual o posterior a la vigencia. En ese caso se conserva por trazabilidad.
                bool usadaEnOrdenes = await _context.ResultadosLandedCost.AnyAsync(r =>
                    r.OrdenImportacion!.MonedaId == tasa.MonedaOrigenId &&
                    r.MonedaLocalId == tasa.MonedaDestinoId &&
                    r.OrdenImportacion.Fecha >= tasa.FechaVigencia);
                bool usadaEnGastos = await _context.GastosImportacion.AnyAsync(g =>
                    g.MonedaId == tasa.MonedaOrigenId &&
                    g.Fecha >= tasa.FechaVigencia &&
                    _context.ResultadosLandedCost.Any(r => r.OrdenImportacionId == g.OrdenImportacionId && r.MonedaLocalId == tasa.MonedaDestinoId));

                if (usadaEnOrdenes || usadaEnGastos)
                {
                    throw new InvalidOperationException("No se puede eliminar esta tasa de cambio porque fue usada en un cálculo oficial de landed cost. Cambie su estado a inactiva.");
                }

                _context.TasasCambio.Remove(tasa);

                await _context.SaveChangesAsync();
            }
        }
    }
}