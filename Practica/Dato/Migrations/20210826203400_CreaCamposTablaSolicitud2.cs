using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class CreaCamposTablaSolicitud2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuncionarioCambioCDPId",
                table: "Solicitud",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuncionarioCambioCDPId",
                table: "Solicitud");
        }
    }
}
