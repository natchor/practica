using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class nuevoCampoSolicitud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObservacionGeneral",
                table: "Solicitud",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObservacionGeneral",
                table: "Solicitud");
        }
    }
}
