using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class programaConCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConCS",
                table: "ProgramaPresupuestario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConCS",
                table: "ProgramaPresupuestario");
        }
    }
}
