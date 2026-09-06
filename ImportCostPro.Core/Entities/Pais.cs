using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class Pais
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre del país")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código ISO es obligatorio")]
        [StringLength(3, MinimumLength = 2,
            ErrorMessage = "El código ISO debe tener entre 2 y 3 caracteres")]
        [Display(Name = "Código ISO")]
        public string CodigoISO { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

    }
}
