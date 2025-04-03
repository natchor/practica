using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class CamposNuevosEstado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "ProgramaPresupuestario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ProgramaPresupuestario");
        }
    }
}
