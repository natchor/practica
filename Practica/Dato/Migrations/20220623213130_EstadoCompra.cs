using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class EstadoCompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
              name: "EstadoCompra",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),

                  EstadoId = table.Column<int>(type: "int", nullable: false),
                  TipoCompraId = table.Column<int>(type: "int", nullable: false)
    },
              constraints: table =>
              {
                  table.PrimaryKey("PK_EstadoCompra", x => x.Id);
              });

            migrationBuilder.AddColumn<int>(
                name: "TipoBitacora",
                table: "Bitacora",
                type: "int",
                defaultValue: 0,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadoCompra");

            migrationBuilder.DropColumn(
                name: "TipoBitacora",
                table: "Bitacora");
        }
    }
}
