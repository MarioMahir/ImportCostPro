using ImportCostPro.Core.Data;
using ImportCostPro.Core.Enums;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class GastoImportacionService : IGastoImportacionService
    {
        private readonly AppDbContext _context;

        public GastoImportacionService(AppDbContext context)
        {
            _context = context;
        }

        // Los productos y gastos solo se modifican mientras la orden esta Abierta; una vez
        // calculada, cerrada o cancelada el calculo oficial debe quedar intacto.
        private async Task ValidarOrdenAbiertaAsync(int ordenId)
        {
            var orden = await _context.OrdenesImportacion
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == ordenId);

            if (orden == null)
            {
                throw new InvalidOperationException("La orden de importación no existe.");
            }

            if (orden.Estado != EstadoOrden.Abierta)
            {
                throw new InvalidOperationException(
                    $"No se puede modificar la orden #{orden.Id} porque está en estado {orden.Estado}. Solo las órdenes abiertas admiten cambios.");
            }
        }

        public async Task<List<GastoImportacion>> ObtenerTodosAsync()
        {
            return await _context.GastosImportacion
                .Include(g => g.Moneda)
                .Include(g => g.OrdenImportacion)
                .ToListAsync();
        }

        public async Task<GastoImportacion?> ObtenerPorIdAsync(int id)
        {
            return await _context.GastosImportacion
                .Include(g => g.Moneda)
                .Include(g => g.OrdenImportacion)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task CrearAsync(GastoImportacion gasto)
        {
            await ValidarOrdenAbiertaAsync(gasto.OrdenImportacionId);
            _context.GastosImportacion.Add(gasto);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(GastoImportacion gasto)
        {
            await ValidarOrdenAbiertaAsync(gasto.OrdenImportacionId);
            _context.GastosImportacion.Update(gasto);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var gasto = await _context.GastosImportacion
                .FindAsync(id);

            if (gasto != null)
            {
                await ValidarOrdenAbiertaAsync(gasto.OrdenImportacionId);
                _context.GastosImportacion.Remove(gasto);

                await _context.SaveChangesAsync();
            }
        }
    }
}