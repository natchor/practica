using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class CamposNuevosTipoCompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Contrato",
                table: "TipoCompra",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "TipoCompra",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contrato",
                table: "TipoCompra");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "TipoCompra");
        }
    }
}
