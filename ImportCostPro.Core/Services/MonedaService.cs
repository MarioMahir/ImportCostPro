using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class MonedaService : IMonedaService
    {
        private readonly AppDbContext _context;

        public MonedaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Moneda>> ObtenerTodasAsync()
        {
            return await _context.Monedas.ToListAsync();
        }

        public async Task<List<Moneda>> ObtenerActivasAsync()
        {
            return await _context.Monedas
                .Where(m => m.Estado)
                .ToListAsync();
        }

        public async Task<Moneda?> ObtenerPorIdAsync(int id)
        {
            return await _context.Monedas.FindAsync(id);
        }

        public async Task CrearAsync(Moneda moneda)
        {
            moneda.CodigoISO = moneda.CodigoISO.ToUpper();

            if (moneda.EsMonedaLocal)
            {
                bool existeMonedaLocal = await _context.Monedas
                    .AnyAsync(m => m.EsMonedaLocal);

                if (existeMonedaLocal)
                {
                    throw new Exception(
                        "Ya existe una moneda configurada como moneda local.");
                }
            }

            bool existe = await _context.Monedas.AnyAsync(m => m.CodigoISO == moneda.CodigoISO);

            if (existe)
            {
                throw new InvalidOperationException("Ya existe una moneda con el mismo código ISO.");
            }

            _context.Monedas.Add(moneda);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Moneda moneda)
        {
            moneda.CodigoISO = moneda.CodigoISO.ToUpper();

            var monedaActual = await _context.Monedas
                .FirstOrDefaultAsync(m => m.Id == moneda.Id);

            if (monedaActual!.EsMonedaLocal && !moneda.Estado)
            {
                throw new Exception("No se puede desactivar la moneda local del sistema.");
            }

            if (moneda.EsMonedaLocal)
            {
                bool existeOtraMonedaLocal = await _context.Monedas
                    .AnyAsync(m =>
                        m.EsMonedaLocal &&
                        m.Id != moneda.Id);

                if (existeOtraMonedaLocal)
                {
                    throw new Exception(
                        "Ya existe una moneda configurada como moneda local.");
                }
            }

            bool existe = await _context.Monedas
                .AnyAsync(m =>
                m.CodigoISO == moneda.CodigoISO &&
                m.Id != moneda.Id);

            if (existe)
                throw new Exception("Ya existe una moneda registrada con este código ISO.");

            _context.Monedas.Update(moneda);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var moneda = await _context.Monedas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (moneda == null)
            {
                throw new Exception("Moneda no encontrada.");
            }

            if (moneda.EsMonedaLocal)
            {
                throw new Exception(
                    "No se puede eliminar la moneda local del sistema.");
            }

            if (moneda != null)
            {
                _context.Monedas.Remove(moneda);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TieneRelacionesAsync(int id)
        {
            bool usadaEnTasas =
                await _context.TasasCambio.AnyAsync(
                    t => t.MonedaOrigenId == id
                      || t.MonedaDestinoId == id);

            bool usadaEnOrdenes =
                await _context.OrdenesImportacion.AnyAsync(
                    o => o.MonedaId == id);

            bool usadaEnGastos =
                await _context.GastosImportacion.AnyAsync(
                    g => g.MonedaId == id);

            return usadaEnTasas
                || usadaEnOrdenes
                || usadaEnGastos;
        }
    }
}