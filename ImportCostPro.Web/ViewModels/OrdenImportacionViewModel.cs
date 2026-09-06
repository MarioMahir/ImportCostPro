using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ImportCostPro.Core.Enums;

namespace ImportCostPro.Web.ViewModels
{
    public class OrdenImportacionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Importador")]
        public int ImportadorId { get; set; }

        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        [Display(Name = "Moneda de la orden")]
        public int MonedaId { get; set; }

        public EstadoOrden Estado { get; set; }

        public List<SelectListItem> Importadores { get; set; } = new();

        public List<SelectListItem> Proveedores { get; set; } = new();
        
        public List<SelectListItem> Monedas { get; set; } = new();
    }
}