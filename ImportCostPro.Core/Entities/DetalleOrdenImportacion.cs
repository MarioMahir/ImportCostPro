using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class DetalleOrdenImportacion
    {
        public int Id { get; set; }

        public int OrdenImportacionId { get; set; }

        public OrdenImportacion? OrdenImportacion { get; set; }

        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Range(0.01, 999999)]
        public decimal PrecioUnitario { get; set; }

        // Porcentaje visible (30 = 30 %). Debe ser >= 0 y < 100 para poder calcular el precio sugerido.
        [Range(0, 99.99)]
        public decimal MargenDeseado { get; set; }
    }
}