using ImportCostPro.Core.Entities;

namespace ImportCostPro.Core.Entities
{
    public class DetalleResultadoLandedCost
    {
        public int Id { get; set; }

        public int ResultadoLandedCostId { get; set; }

        public ResultadoLandedCost? ResultadoLandedCost { get; set; }

        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }

        public decimal FOBOriginal { get; set; }

        public decimal FOBLocal { get; set; }

        public decimal FleteAsignado { get; set; }

        public decimal SeguroAsignado { get; set; }

        public decimal CIF { get; set; }

        // Porcentajes de la categoria arancelaria en el momento del calculo.
        public decimal PorcentajeArancel { get; set; }

        public decimal PorcentajeImpuestoSelectivo { get; set; }

        public decimal Arancel { get; set; }

        public decimal ImpuestoSelectivo { get; set; }

        public decimal TasaServicioAduanal { get; set; }

        public decimal ITBIS { get; set; }

        public decimal GastosLocalesAsignados { get; set; }

        public decimal CostoTotalImportado { get; set; }

        public decimal CostoUnitarioImportado { get; set; }

        public decimal MargenDeseado { get; set; }

        public decimal PrecioVentaSugerido { get; set; }
    }
}