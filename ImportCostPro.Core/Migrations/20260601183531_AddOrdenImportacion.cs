using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdenImportacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenesImportacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImportadorId = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesImportacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesImportacion_Importadores_ImportadorId",
                        column: x => x.ImportadorId,
                        principalTable: "Importadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesImportacion_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesOrdenImportacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenImportacionId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesOrdenImportacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesOrdenImportacion_OrdenesImportacion_OrdenImportacionId",
                        column: x => x.OrdenImportacionId,
                        principalTable: "OrdenesImportacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesOrdenImportacion_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesOrdenImportacion_OrdenImportacionId",
                table: "DetallesOrdenImportacion",
                column: "OrdenImportacionId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesOrdenImportacion_ProductoId",
                table: "DetallesOrdenImportacion",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_ImportadorId",
                table: "OrdenesImportacion",
                column: "ImportadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_ProveedorId",
                table: "OrdenesImportacion",
                column: "ProveedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesOrdenImportacion");

            migrationBuilder.DropTable(
                name: "OrdenesImportacion");
        }
    }
}
