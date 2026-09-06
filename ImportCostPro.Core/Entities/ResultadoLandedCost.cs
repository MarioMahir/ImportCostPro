using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Entities
{
    public class ResultadoLandedCost
    {
        public int Id { get; set; }

        public int OrdenImportacionId { get; set; }

        public OrdenImportacion? OrdenImportacion { get; set; }

        public DateTime FechaCalculo { get; set; }

        public decimal FOBTotalOriginal { get; set; }

        public decimal FOBTotalLocal { get; set; }

        public decimal FleteTotal { get; set; }

        public decimal SeguroTotal { get; set; }

        public decimal CIFTotal { get; set; }

        public decimal TotalArancel { get; set; }

        public decimal TotalImpuestoSelectivo { get; set; }

        public decimal TotalTasaServicioAduanal { get; set; }

        public decimal TotalITBIS { get; set; }

        public decimal TotalGastosLocales { get; set; }

        public decimal CostoTotalImportacion { get; set; }

        public int CantidadTotalImportada { get; set; }

        public decimal TasaCambioOrden { get; set; }

        // Copia de la configuracion de impuestos usada, para que cambiarla luego no altere el historico.
        public decimal PorcentajeITBIS { get; set; }

        public decimal PorcentajeTasaServicioAduanal { get; set; }

        public int MonedaLocalId { get; set; }

        public Moneda? MonedaLocal { get; set; }

        public ICollection<DetalleResultadoLandedCost> Detalles { get; set; }
            = new List<DetalleResultadoLandedCost>();
    }
}