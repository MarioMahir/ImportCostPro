using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Web.ViewModels
{
    public class ProveedorViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "País de origen")]
        public int PaisId { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required]
        [Display(Name = "Moneda principal")]
        public int MonedaId { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;

        public List<SelectListItem> Paises { get; set; }
            = new();

        public List<SelectListItem> Monedas { get; set; }
            = new();
    }
}