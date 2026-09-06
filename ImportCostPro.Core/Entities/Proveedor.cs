using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class Proveedor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "País de origen")]
        public int PaisId { get; set; }

        public Pais? Pais { get; set; }

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

        public Moneda? Moneda { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;

        public ICollection<OrdenImportacion> OrdenesImportacion
            = new List<OrdenImportacion>();
    }
}