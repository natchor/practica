using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class PropertiesSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropertiesSystem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesSystem", x => x.Id);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_PropertiesSystem_CodigoUNQ",
            //    table: "PropertiesSystem",
            //    column: "Codigo");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "PropertiesSystem");

            //migrationBuilder.DropIndex(
            //    table: "PropertiesSystem",
            //    name: "IX_PropertiesSystem_CodigoUNQ");


        }
    }
}
