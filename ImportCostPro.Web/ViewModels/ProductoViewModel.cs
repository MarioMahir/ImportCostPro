using ImportCostPro.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Web.ViewModels
{

    public class ProductoViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        public List<SelectListItem> Paises { get; set; } = new();

        public List<SelectListItem> Categorias { get; set; } = new();

        [Required]
        [StringLength(50)]
        [Display(Name = "Código de referencia")]
        public string CodigoReferencia { get; set; } = string.Empty;

        [Required]
        [Display(Name = "País de origen")]
        public int PaisId { get; set; }

        [Required]
        [Display(Name = "Categoría arancelaria")]
        public int CategoriaArancelariaId { get; set; }

        [Required]
        [Display(Name = "Peso unitario (kg)")]
        public decimal PesoUnitario { get; set; }

        [Display(Name = "Largo (m)")]
        public decimal? Largo { get; set; }

        [Display(Name = "Ancho (m)")]
        public decimal? Ancho { get; set; }

        [Display(Name = "Alto (m)")]
        public decimal? Alto { get; set; }

        [Required]
        [Display(Name = "Unidad de medida")]
        public UnidadMedida UnidadMedida { get; set; }

        [StringLength(250)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;
    }
}