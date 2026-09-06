using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly AppDbContext _context;

        public ProveedorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }

        public async Task<Proveedor?> ObtenerPorIdAsync(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }

        public async Task CrearAsync(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Proveedor proveedor)
        {
            var actual =
                await _context.Proveedores
                .Include(p => p.OrdenesImportacion)
                .FirstOrDefaultAsync(p => p.Id == proveedor.Id);

            if (actual == null)
            {
                throw new Exception("Proveedor no encontrado.");
            }

            if (actual.OrdenesImportacion.Any())
            {
                if (actual.PaisId != proveedor.PaisId ||
                    actual.MonedaId != proveedor.MonedaId)
                {
                    throw new Exception(
                        "No se puede modificar el país ni la moneda porque este proveedor tiene órdenes registradas.");
                }
            }

            if (actual == null)
            {
                throw new Exception("Proveedor no encontrado.");
            }
            _context.Proveedores.Update(proveedor);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tieneOrdenes =
                await _context.OrdenesImportacion
                .AnyAsync(o => o.ProveedorId == id);

            if (tieneOrdenes)
            {
                throw new Exception(
                    "No se puede eliminar este proveedor porque tiene órdenes de importación registradas.");
            }

            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);

                await _context.SaveChangesAsync();
            }
        }
    }
}