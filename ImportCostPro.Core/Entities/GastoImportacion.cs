using ImportCostPro.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class GastoImportacion
    {
        public int Id { get; set; }

        [Required]
        public int OrdenImportacionId { get; set; }

        public OrdenImportacion? OrdenImportacion { get; set; }

        [Required]
        public TipoGasto TipoGasto { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public int MonedaId { get; set; }

        public Moneda? Moneda { get; set; }

        [Required]
        public MetodoDistribucion MetodoDistribucion { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
    }
}