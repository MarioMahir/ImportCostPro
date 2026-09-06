using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class TasaCambio
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Moneda origen")]
        public int MonedaOrigenId { get; set; }

        public Moneda? MonedaOrigen { get; set; }

        [Required]
        [Display(Name = "Moneda destino")]
        public int MonedaDestinoId { get; set; }

        public Moneda? MonedaDestino { get; set; }

        [Required]
        [Range(0.0001, 999999)]
        [Display(Name = "Valor de la tasa")]
        public decimal ValorTasa { get; set; }

        [Required]
        [Display(Name = "Fecha de vigencia")]
        public DateTime FechaVigencia { get; set; }

        public bool Estado { get; set; } = true;
    }
}