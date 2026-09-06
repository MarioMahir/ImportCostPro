using ImportCostPro.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.Entities
{
    public class OrdenImportacion
    {
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public int ImportadorId { get; set; }

        public Importador? Importador { get; set; }

        public int ProveedorId { get; set; }

        public Proveedor? Proveedor { get; set; }

        public int MonedaId { get; set; }

        public Moneda? Moneda { get; set; }

        public EstadoOrden Estado { get; set; } = EstadoOrden.Abierta;

        public DateTime? FechaCierre { get; set; }

        public ResultadoLandedCost? ResultadoOficial { get; set; }

        public ICollection<DetalleOrdenImportacion> Detalles { get; set; }
            = new List<DetalleOrdenImportacion>();

        public ICollection<GastoImportacion> Gastos { get; set; }
            = new List<GastoImportacion>();
    }
}