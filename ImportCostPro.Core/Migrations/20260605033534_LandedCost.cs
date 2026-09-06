using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Core.Migrations
{
    /// <inheritdoc />
    public partial class LandedCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultadosLandedCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenImportacionId = table.Column<int>(type: "int", nullable: false),
                    FechaCalculo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FOBTotalOriginal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FOBTotalLocal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CIFTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArancel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalImpuestoSelectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTasaServicioAduanal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalITBIS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalGastosLocales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoTotalImportacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TasaCambioOrden = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonedaLocalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosLandedCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadosLandedCost_Monedas_MonedaLocalId",
                        column: x => x.MonedaLocalId,
                        principalTable: "Monedas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadosLandedCost_OrdenesImportacion_OrdenImportacionId",
                        column: x => x.OrdenImportacionId,
                        principalTable: "OrdenesImportacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesResultadoLandedCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultadoLandedCostId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FOBOriginal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FOBLocal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FleteAsignado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeguroAsignado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CIF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Arancel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImpuestoSelectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TasaServicioAduanal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ITBIS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GastosLocalesAsignados = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoTotalImportado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoUnitarioImportado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargenDeseado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioVentaSugerido = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesResultadoLandedCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesResultadoLandedCost_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesResultadoLandedCost_ResultadosLandedCost_ResultadoLandedCostId",
                        column: x => x.ResultadoLandedCostId,
                        principalTable: "ResultadosLandedCost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesResultadoLandedCost_ProductoId",
                table: "DetallesResultadoLandedCost",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesResultadoLandedCost_ResultadoLandedCostId",
                table: "DetallesResultadoLandedCost",
                column: "ResultadoLandedCostId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosLandedCost_MonedaLocalId",
                table: "ResultadosLandedCost",
                column: "MonedaLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosLandedCost_OrdenImportacionId",
                table: "ResultadosLandedCost",
                column: "OrdenImportacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesResultadoLandedCost");

            migrationBuilder.DropTable(
                name: "ResultadosLandedCost");
        }
    }
}
