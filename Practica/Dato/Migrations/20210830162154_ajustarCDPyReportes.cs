using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class ajustarCDPyReportes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AjustarCDP",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Reportes",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AjustarCDP",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Reportes",
                table: "Role");
        }
    }
}
