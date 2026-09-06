using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Importadores_ImportadorId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Monedas_MonedaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Paises_PaisId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaId",
                table: "TasasCambio");

            migrationBuilder.DropColumn(
                name: "Contacto",
                table: "Proveedores");

            migrationBuilder.RenameColumn(
                name: "Tasa",
                table: "TasasCambio",
                newName: "ValorTasa");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "TasasCambio",
                newName: "FechaVigencia");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Productos",
                newName: "PesoUnitario");

            migrationBuilder.RenameColumn(
                name: "ImportadorId",
                table: "Productos",
                newName: "PaisId1");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_ImportadorId",
                table: "Productos",
                newName: "IX_Productos_PaisId1");

            migrationBuilder.AlterColumn<int>(
                name: "MonedaId",
                table: "TasasCambio",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MonedaDestinoId",
                table: "TasasCambio",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MonedaOrigenId",
                table: "TasasCambio",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Proveedores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "MonedaId",
                table: "Proveedores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Proveedores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Productos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "MonedaId",
                table: "Productos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaArancelariaId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Alto",
                table: "Productos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Ancho",
                table: "Productos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaArancelariaId1",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoReferencia",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Largo",
                table: "Productos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnidadMedida",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Importadores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "RNC",
                table: "Importadores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Importadores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Importadores",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Importadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AplicaITBIS",
                table: "CategoriasArancelarias",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AplicaImpuestoSelectivo",
                table: "CategoriasArancelarias",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CodigoArancelario",
                table: "CategoriasArancelarias",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PorcentajeImpuestoSelectivo",
                table: "CategoriasArancelarias",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TasasCambio_MonedaDestinoId",
                table: "TasasCambio",
                column: "MonedaDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_TasasCambio_MonedaOrigenId",
                table: "TasasCambio",
                column: "MonedaOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_MonedaId",
                table: "Proveedores",
                column: "MonedaId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_PaisId",
                table: "Proveedores",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaArancelariaId1",
                table: "Productos",
                column: "CategoriaArancelariaId1");

            migrationBuilder.CreateIndex(
                name: "IX_Importadores_PaisId",
                table: "Importadores",
                column: "PaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Importadores_Paises_PaisId",
                table: "Importadores",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_CategoriasArancelarias_CategoriaArancelariaId1",
                table: "Productos",
                column: "CategoriaArancelariaId1",
                principalTable: "CategoriasArancelarias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Monedas_MonedaId",
                table: "Productos",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Paises_PaisId",
                table: "Productos",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Paises_PaisId1",
                table: "Productos",
                column: "PaisId1",
                principalTable: "Paises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_Monedas_MonedaId",
                table: "Proveedores",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_Paises_PaisId",
                table: "Proveedores",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaDestinoId",
                table: "TasasCambio",
                column: "MonedaDestinoId",
                principalTable: "Monedas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaId",
                table: "TasasCambio",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaOrigenId",
                table: "TasasCambio",
                column: "MonedaOrigenId",
                principalTable: "Monedas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Importadores_Paises_PaisId",
                table: "Importadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_CategoriasArancelarias_CategoriaArancelariaId1",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Monedas_MonedaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Paises_PaisId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Paises_PaisId1",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedores_Monedas_MonedaId",
                table: "Proveedores");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedores_Paises_PaisId",
                table: "Proveedores");

            migrationBuilder.DropForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaDestinoId",
                table: "TasasCambio");

            migrationBuilder.DropForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaId",
                table: "TasasCambio");

            migrationBuilder.DropForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaOrigenId",
                table: "TasasCambio");

            migrationBuilder.DropIndex(
                name: "IX_TasasCambio_MonedaDestinoId",
                table: "TasasCambio");

            migrationBuilder.DropIndex(
                name: "IX_TasasCambio_MonedaOrigenId",
                table: "TasasCambio");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_MonedaId",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_PaisId",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CategoriaArancelariaId1",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Importadores_PaisId",
                table: "Importadores");

            migrationBuilder.DropColumn(
                name: "MonedaDestinoId",
                table: "TasasCambio");

            migrationBuilder.DropColumn(
                name: "MonedaOrigenId",
                table: "TasasCambio");

            migrationBuilder.DropColumn(
                name: "MonedaId",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "Alto",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Ancho",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CategoriaArancelariaId1",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CodigoReferencia",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Largo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Importadores");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Importadores");

            migrationBuilder.DropColumn(
                name: "AplicaITBIS",
                table: "CategoriasArancelarias");

            migrationBuilder.DropColumn(
                name: "AplicaImpuestoSelectivo",
                table: "CategoriasArancelarias");

            migrationBuilder.DropColumn(
                name: "CodigoArancelario",
                table: "CategoriasArancelarias");

            migrationBuilder.DropColumn(
                name: "PorcentajeImpuestoSelectivo",
                table: "CategoriasArancelarias");

            migrationBuilder.RenameColumn(
                name: "ValorTasa",
                table: "TasasCambio",
                newName: "Tasa");

            migrationBuilder.RenameColumn(
                name: "FechaVigencia",
                table: "TasasCambio",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "PesoUnitario",
                table: "Productos",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "PaisId1",
                table: "Productos",
                newName: "ImportadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_PaisId1",
                table: "Productos",
                newName: "IX_Productos_ImportadorId");

            migrationBuilder.AlterColumn<int>(
                name: "MonedaId",
                table: "TasasCambio",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Proveedores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contacto",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Productos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<int>(
                name: "MonedaId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaArancelariaId",
                table: "Productos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Importadores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RNC",
                table: "Importadores",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Importadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Importadores_ImportadorId",
                table: "Productos",
                column: "ImportadorId",
                principalTable: "Importadores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Monedas_MonedaId",
                table: "Productos",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Paises_PaisId",
                table: "Productos",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TasasCambio_Monedas_MonedaId",
                table: "TasasCambio",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
