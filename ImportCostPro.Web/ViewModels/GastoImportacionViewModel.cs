using ImportCostPro.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Web.ViewModels
{
    public class GastoImportacionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Orden")]
        [Required]
        public int OrdenImportacionId { get; set; }

        [Display(Name = "Tipo de gasto")]
        [Required]
        public TipoGasto TipoGasto { get; set; }

        [Required]
        [Range(0.01, 999999999, ErrorMessage = "El monto debe ser mayor que 0.")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Display(Name = "Moneda")]
        [Required]
        public int MonedaId { get; set; }

        [Display(Name = "Método de distribución")]
        [Required]
        public MetodoDistribucion MetodoDistribucion { get; set; }

        [Required]
        [Display(Name = "Fecha del gasto")]
        public DateTime Fecha { get; set; }
            = DateTime.Today;

        public List<SelectListItem> Ordenes { get; set; }
            = new();

        public List<SelectListItem> Monedas { get; set; }
            = new();
    }
}