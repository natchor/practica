using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class nuevoCampoCodEstadoLic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodEstadoLicitacion",
                table: "Solicitud",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoStr",
                table: "Estado",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodEstadoLicitacion",
                table: "Solicitud");

            migrationBuilder.DropColumn(
                name: "CodigoStr",
                table: "Estado");
        }
    }
}
