using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class SinCDP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SinCDP",
                table: "ProgramaPresupuestario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SinCDP",
                table: "ProgramaPresupuestario");
        }
    }
}
