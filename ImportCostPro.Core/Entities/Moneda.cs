using Microsoft.Win32.SafeHandles;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class Moneda
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Nombre de la moneda")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El símbolo es obligatorio")]
        [StringLength(10)]
        [Display(Name = "Símbolo")]
        public string Simbolo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código ISO es obligatorio")]
        [StringLength(3, MinimumLength = 3)]
        [Display(Name = "Código ISO")]
        public string CodigoISO { get; set; } = string.Empty;

        [Display(Name = "Activa")]
        public bool Estado { get; set; } = true;

        [Display(Name = "Moneda local")]
        public bool EsMonedaLocal { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

        public ICollection<TasaCambio> TasasCambio { get; set; } = new List<TasaCambio>();

        public ICollection<GastoImportacion> GastosImportacion { get; set; } = new List<GastoImportacion>();
    }
}