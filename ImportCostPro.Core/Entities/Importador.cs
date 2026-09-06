using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Importador
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El RNC es obligatorio")]
        [StringLength(20)]
        [Display(Name = "RNC")]
        public string RNC { get; set; } = string.Empty;

        [Required]
        [Display(Name = "País")]
        public int PaisId { get; set; }

        public Pais? Pais { get; set; }

        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        [StringLength(250)]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;

        public ICollection<OrdenImportacion> OrdenesImportacion
        { get; set; } = new List<OrdenImportacion>();
    }
}