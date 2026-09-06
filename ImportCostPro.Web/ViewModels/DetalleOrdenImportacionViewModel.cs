using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Web.ViewModels
{
    public class DetalleOrdenImportacionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Orden")]
        public int OrdenImportacionId { get; set; }

        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Range(1, 999999, ErrorMessage = "La cantidad debe ser mayor que 0.")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio FOB unitario")]
        [Range(0.01, 999999, ErrorMessage = "El precio FOB unitario debe ser mayor que 0.")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Margen deseado (%)")]
        [Range(0, 99.99, ErrorMessage = "El margen deseado debe ser mayor o igual que 0 y menor que 100.")]
        public decimal MargenDeseado { get; set; }

        public List<SelectListItem>
            Ordenes
        { get; set; }
            = new();

        public List<SelectListItem>
            Productos
        { get; set; }
            = new();
    }
}