namespace ImportCostPro.Core.Dtos
{
    // Resultado del calculo de landed cost de una orden, expresado en moneda local.
    // Se usa tanto para la vista previa como para construir el resultado oficial.
    public class LandedCostCalculoDto
    {
        public int OrdenId { get; set; }
        public DateTime FechaOrden { get; set; }
        public string Importador { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string MonedaOrden { get; set; } = string.Empty;
        public string MonedaLocal { get; set; } = string.Empty;
        public int MonedaLocalId { get; set; }
        public decimal TasaCambioOrden { get; set; }
        public decimal PorcentajeITBIS { get; set; }
        public decimal PorcentajeTasaServicioAduanal { get; set; }

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

        public List<LandedCostDetalleDto> Detalles { get; set; } = new();
    }

    public class LandedCostDetalleDto
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal FOBOriginal { get; set; }
        public decimal FOBLocal { get; set; }
        public decimal FleteAsignado { get; set; }
        public decimal SeguroAsignado { get; set; }
        public decimal CIF { get; set; }
        public decimal PorcentajeArancel { get; set; }
        public decimal Arancel { get; set; }
        public decimal PorcentajeImpuestoSelectivo { get; set; }
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
