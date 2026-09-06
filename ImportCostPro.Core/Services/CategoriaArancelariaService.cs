using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class CategoriaArancelariaService : ICategoriaArancelariaService
    {
        private readonly AppDbContext _context;

        public CategoriaArancelariaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaArancelaria>> ObtenerTodasAsync()
        {
            return await _context.CategoriasArancelarias
                .ToListAsync();
        }

        public async Task<List<CategoriaArancelaria>> ObtenerActivasAsync()
        {
            return await _context.CategoriasArancelarias
                .Where(c => c.Estado)
                .ToListAsync();
        }

        public async Task<CategoriaArancelaria?> ObtenerPorIdAsync(int id)
        {
            return await _context.CategoriasArancelarias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CrearAsync(CategoriaArancelaria categoria)
        {
            categoria.CodigoArancelario =
                categoria.CodigoArancelario.Trim();

            _context.CategoriasArancelarias.Add(categoria);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(CategoriaArancelaria categoria)
        {
            _context.CategoriasArancelarias.Update(categoria);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteCodigoAsync(
            string codigo,
            int? excluirId = null)
        {
            codigo = codigo.Trim();

            return await _context.CategoriasArancelarias
                .AnyAsync(c =>
                    c.CodigoArancelario == codigo &&
                    (!excluirId.HasValue || c.Id != excluirId));
        }

        public async Task<bool> TieneProductosAsync(int categoriaId)
        {
            return await _context.Productos
                .AnyAsync(p =>
                    p.CategoriaArancelariaId == categoriaId);
        }

        public async Task EliminarAsync(int id)
        {
            var categoria =
                await _context.CategoriasArancelarias.FindAsync(id);

            if (categoria != null)
            {
                _context.CategoriasArancelarias.Remove(categoria);

                await _context.SaveChangesAsync();
            }
        }
    }
}