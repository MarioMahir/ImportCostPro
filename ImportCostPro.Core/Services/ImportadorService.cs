using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class ImportadorService : IImportadorService
    {
        private readonly AppDbContext _context;

        public ImportadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Importador>> ObtenerTodosAsync()
        {
            return await _context.Importadores.ToListAsync();
        }

        public async Task<Importador?> ObtenerPorIdAsync(int id)
        {
            return await _context.Importadores.FindAsync(id);
        }

        public async Task CrearAsync(Importador importador)
        {
            _context.Importadores.Add(importador);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Importador importador)
        {
            _context.Importadores.Update(importador);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var importador = await _context.Importadores.FindAsync(id);

            if (importador != null)
            {
                bool tieneOrdenes = await _context.OrdenesImportacion.AnyAsync(o => o.ImportadorId == id);
                if (tieneOrdenes)
                {
                    throw new InvalidOperationException("No se puede eliminar este importador porque tiene órdenes de importación asociadas. Cambie su estado a inactivo.");
                }

                _context.Importadores.Remove(importador);

                await _context.SaveChangesAsync();
            }
        }
    }
}