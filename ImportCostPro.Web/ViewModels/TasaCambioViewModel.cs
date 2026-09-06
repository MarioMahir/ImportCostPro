using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Web.ViewModels
{
    public class TasaCambioViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Moneda origen")]
        public int MonedaOrigenId { get; set; }

        [Required]
        [Display(Name = "Moneda destino")]
        public int MonedaDestinoId { get; set; }

        [Required]
        [Range(0.0001, 999999)]
        [Display(Name = "Valor de la tasa")]
        public decimal ValorTasa { get; set; }

        [Required]
        [Display(Name = "Fecha de vigencia")]
        public DateTime FechaVigencia { get; set; }

        [Display(Name = "Activa")]
        public bool Estado { get; set; } = true;

        public List<SelectListItem> Monedas { get; set; }
            = new();
    }
}