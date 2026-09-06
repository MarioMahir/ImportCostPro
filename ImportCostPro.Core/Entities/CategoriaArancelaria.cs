using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class CategoriaArancelaria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código arancelario es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Código arancelario")]
        public string CodigoArancelario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(150)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Porcentaje Arancel")]
        [Range(0, 100)]
        public decimal PorcentajeArancel { get; set; }

        [Display(Name = "Aplica ITBIS")]
        public bool AplicaITBIS { get; set; }

        [Display(Name = "Aplica Impuesto Selectivo")]
        public bool AplicaImpuestoSelectivo { get; set; }

        [Display(Name = "Porcentaje Impuesto Selectivo")]
        [Range(0, 100)]
        public decimal PorcentajeImpuestoSelectivo { get; set; }

        [Display(Name = "Activa")]
        public bool Estado { get; set; } = true;

        public ICollection<Producto> Productos { get; set; }
            = new List<Producto>();
    }
}