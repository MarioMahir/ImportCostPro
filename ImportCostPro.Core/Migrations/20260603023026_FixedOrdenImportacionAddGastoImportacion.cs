using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixedOrdenImportacionAddGastoImportacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "OrdenesImportacion",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "MonedaId",
                table: "OrdenesImportacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GastosImportacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenImportacionId = table.Column<int>(type: "int", nullable: false),
                    TipoGasto = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MonedaId = table.Column<int>(type: "int", nullable: false),
                    MetodoDistribucion = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastosImportacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GastosImportacion_Monedas_MonedaId",
                        column: x => x.MonedaId,
                        principalTable: "Monedas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GastosImportacion_OrdenesImportacion_OrdenImportacionId",
                        column: x => x.OrdenImportacionId,
                        principalTable: "OrdenesImportacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_MonedaId",
                table: "OrdenesImportacion",
                column: "MonedaId");

            migrationBuilder.CreateIndex(
                name: "IX_GastosImportacion_MonedaId",
                table: "GastosImportacion",
                column: "MonedaId");

            migrationBuilder.CreateIndex(
                name: "IX_GastosImportacion_OrdenImportacionId",
                table: "GastosImportacion",
                column: "OrdenImportacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Monedas_MonedaId",
                table: "OrdenesImportacion",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Monedas_MonedaId",
                table: "OrdenesImportacion");

            migrationBuilder.DropTable(
                name: "GastosImportacion");

            migrationBuilder.DropIndex(
                name: "IX_OrdenesImportacion_MonedaId",
                table: "OrdenesImportacion");

            migrationBuilder.DropColumn(
                name: "MonedaId",
                table: "OrdenesImportacion");

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "OrdenesImportacion",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
