using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class NuevosCamposAjustarCDP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsAjuste",
                table: "SolicitudDetalle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoMonedaSelFinal",
                table: "SolicitudDetalle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsAjuste",
                table: "SolicitudDetalle");

            migrationBuilder.DropColumn(
                name: "MontoMonedaSelFinal",
                table: "SolicitudDetalle");
        }
    }
}
