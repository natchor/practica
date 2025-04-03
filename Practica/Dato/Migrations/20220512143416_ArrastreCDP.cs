using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class ArrastreCDP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Arrastre",
                table: "Solicitud",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arrastre",
                table: "Solicitud");

        }
    }
}
