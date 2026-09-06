using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class ConfiguracionImpuestos
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal PorcentajeITBIS { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal PorcentajeTasaServicioAduanal { get; set; }
    }
}