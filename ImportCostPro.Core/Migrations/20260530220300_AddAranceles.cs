using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAranceles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaArancelariaId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImportadorId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoriasArancelarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PorcentajeArancel = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasArancelarias", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaArancelariaId",
                table: "Productos",
                column: "CategoriaArancelariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_ImportadorId",
                table: "Productos",
                column: "ImportadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_CategoriasArancelarias_CategoriaArancelariaId",
                table: "Productos",
                column: "CategoriaArancelariaId",
                principalTable: "CategoriasArancelarias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Importadores_ImportadorId",
                table: "Productos",
                column: "ImportadorId",
                principalTable: "Importadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_CategoriasArancelarias_CategoriaArancelariaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Importadores_ImportadorId",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "CategoriasArancelarias");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CategoriaArancelariaId",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_ImportadorId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CategoriaArancelariaId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "ImportadorId",
                table: "Productos");
        }
    }
}
