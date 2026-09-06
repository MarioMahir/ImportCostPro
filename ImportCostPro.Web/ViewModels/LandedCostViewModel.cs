using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.ViewModels
{
    public class LandedCostViewModel
    {
        [Display(Name = "Orden de importación")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una orden abierta.")]
        public int OrdenId { get; set; }

        public List<SelectListItem> Ordenes { get; set; } = new();
    }
}
