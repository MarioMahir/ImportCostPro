using ImportCostPro.Core.Data;
using ImportCostPro.Core.Enums;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class DetalleOrdenImportacionService
        : IDetalleOrdenImportacionService
    {
        private readonly AppDbContext _context;

        public DetalleOrdenImportacionService( AppDbContext context )
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

        public async Task<List<DetalleOrdenImportacion>>
            ObtenerTodosAsync()
        {
            return await _context
                .DetallesOrdenImportacion
                .Include(d => d.Producto)
                .Include(d => d.OrdenImportacion)
                .ToListAsync();
        }

        public async Task<List<DetalleOrdenImportacion>>
            ObtenerPorOrdenAsync(int ordenId)
        {
            return await _context
                .DetallesOrdenImportacion
                .Include(d => d.Producto)
                .Where(d =>
                    d.OrdenImportacionId == ordenId)
                .ToListAsync();
        }

        public async Task<DetalleOrdenImportacion?>
            ObtenerPorIdAsync(int id)
        {
            return await _context
                .DetallesOrdenImportacion
                .FindAsync(id);
        }

        public async Task CrearAsync(
            DetalleOrdenImportacion detalle)
        {
            await ValidarOrdenAbiertaAsync(detalle.OrdenImportacionId);
            _context
                .DetallesOrdenImportacion
                .Add(detalle);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(
            DetalleOrdenImportacion detalle)
        {
            await ValidarOrdenAbiertaAsync(detalle.OrdenImportacionId);
            _context
                .DetallesOrdenImportacion
                .Update(detalle);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var detalle =
                await _context
                .DetallesOrdenImportacion
                .FindAsync(id);

            if (detalle != null)
            {
                await ValidarOrdenAbiertaAsync(detalle.OrdenImportacionId);
                _context
                    .DetallesOrdenImportacion
                    .Remove(detalle);

                await _context.SaveChangesAsync();
            }
        }
    }
}