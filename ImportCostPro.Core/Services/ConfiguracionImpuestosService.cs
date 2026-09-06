using ImportCostPro.Core.Data;
using ImportCostPro.Core.Entities;
using ImportCostPro.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class ConfiguracionImpuestosService
        : IConfiguracionImpuestosService
    {
        private readonly AppDbContext _context;

        public ConfiguracionImpuestosService(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracionImpuestos?>
            ObtenerAsync()
        {
            return await _context
                .ConfiguracionesImpuestos
                .FirstOrDefaultAsync();
        }

        public async Task GuardarAsync(
            ConfiguracionImpuestos configuracion)
        {
            var existente =
                await ObtenerAsync();

            if (existente == null)
            {
                _context
                    .ConfiguracionesImpuestos
                    .Add(configuracion);
            }
            else
            {
                existente.PorcentajeITBIS =
                    configuracion.PorcentajeITBIS;

                existente
                    .PorcentajeTasaServicioAduanal =
                    configuracion
                    .PorcentajeTasaServicioAduanal;
            }

            await _context.SaveChangesAsync();
        }
    }
}