using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerActivosAsync()
        {
            return await _context.Productos
                .Where(p => p.Estado)
                .ToListAsync();
        }
        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos
                .Include(p => p.Pais)
                .Include(p => p.CategoriaArancelaria)
                .ToListAsync();
        }

        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Pais)
                .Include(p => p.CategoriaArancelaria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CrearAsync(Producto producto)
        {
            bool alguna =
                producto.Largo.HasValue ||
                producto.Ancho.HasValue ||
                producto.Alto.HasValue;

            bool todas =
                producto.Largo.HasValue &&
                producto.Ancho.HasValue &&
                producto.Alto.HasValue;

            if (alguna && !todas)
            {
                throw new Exception(
                    "Debe completar largo, ancho y alto.");
            }

            _context.Productos.Add(producto);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Producto producto)
        {
            bool alguna =
                producto.Largo.HasValue ||
                producto.Ancho.HasValue ||
                producto.Alto.HasValue;

            bool todas =
                producto.Largo.HasValue &&
                producto.Ancho.HasValue &&
                producto.Alto.HasValue;

            if (alguna && !todas)
            {
                throw new Exception(
                    "Debe completar largo, ancho y alto.");
            }

            bool existe = await _context.Productos
                .AnyAsync(p =>
                    p.Id != producto.Id &&
                    p.CodigoReferencia == producto.CodigoReferencia);

            if (existe)
            {
                throw new Exception(
                    "Ya existe un producto registrado con este código.");
            }

            _context.Productos.Update(producto);
            producto.CodigoReferencia = producto.CodigoReferencia.Trim();

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            bool tieneOrdenes =
                await _context.DetallesOrdenImportacion
                .AnyAsync(d => d.ProductoId == id);

            if (tieneOrdenes)
            {
                throw new Exception("No se puede eliminar este producto porque está asociado a órdenes.");
            }

            var producto = await _context.Productos.FindAsync(id);

            if (producto != null)
            {
                _context.Productos.Remove(producto);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteCodigoAsync(string codigo)
        {
            return await _context.Productos
                .AnyAsync(p => p.CodigoReferencia == codigo);
        }
    }
}