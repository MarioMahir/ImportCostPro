using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class PaisService : IPaisService
    {
        private readonly AppDbContext _context;

        public PaisService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pais>> ObtenerTodosAsync()
        {
            return await _context.Paises.ToListAsync();
        }

        public async Task<Pais?> ObtenerPorIdAsync(int id)
        {
            return await _context.Paises.FindAsync(id);
        }

        public async Task<List<Pais>> ObtenerActivosAsync()
        {
            return await _context.Paises
                .Where(p => p.Estado)
                .ToListAsync();
        }

        public async Task CrearAsync(Pais pais)
        {
            pais.CodigoISO = pais.CodigoISO.ToUpper();

            var existe = await _context.Paises.AnyAsync(p => p.CodigoISO == pais.CodigoISO);

            if (existe)
            {
                throw new InvalidOperationException("Ya existe un país con el mismo código ISO.");
            }

            _context.Paises.Add(pais);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Pais pais)
        {
            pais.CodigoISO = pais.CodigoISO.ToUpper();

            var existe = await _context.Paises.AnyAsync(p => p.CodigoISO == pais.CodigoISO && p.Id != pais.Id);

            if (existe)
            {
                throw new InvalidOperationException("Ya existe un país con el mismo código ISO.");
            }

            _context.Paises.Update(pais);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var pais = await _context.Paises.FindAsync(id);

            if (pais == null)
            {
                return;
            }

            bool tieneRelaciones =
                await _context.Productos.AnyAsync(p => p.PaisId == id) ||
                await _context.Proveedores.AnyAsync(p => p.PaisId == id) ||
                await _context.Importadores.AnyAsync(i => i.PaisId == id);

            if (tieneRelaciones)
            {
                throw new InvalidOperationException("No se puede eliminar este país porque está asociado a otros registros del sistema (productos, proveedores o importadores). Cambie su estado a inactivo.");
            }

            _context.Paises.Remove(pais);

            await _context.SaveChangesAsync();
        }
    }
}