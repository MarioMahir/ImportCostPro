using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.ViewModels
{
    public class ImportadorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El RNC es obligatorio")]
        [StringLength(20)]
        public string RNC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un país")]
        public int PaisId { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(250)]
        public string? Direccion { get; set; }

        public bool Estado { get; set; } = true;

        public IEnumerable<SelectListItem> Paises { get; set; }
            = new List<SelectListItem>();
    }
}