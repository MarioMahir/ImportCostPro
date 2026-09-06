using ImportCostPro.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Código de referencia")]
        public string CodigoReferencia { get; set; } = string.Empty;

        [Required]
        [Display(Name = "País de origen")]
        public int PaisId { get; set; }

        public Pais? Pais { get; set; }

        [Required]
        [Display(Name = "Categoría arancelaria")]
        public int CategoriaArancelariaId { get; set; }

        public CategoriaArancelaria? CategoriaArancelaria { get; set; }

        [Required]
        [Range(0.01, 999999)]
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

        public ICollection<DetalleOrdenImportacion> DetallesOrdenImportacion { get; set; }
                = new List<DetalleOrdenImportacion>();
    }
}