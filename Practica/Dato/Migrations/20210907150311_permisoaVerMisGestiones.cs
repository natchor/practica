using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class permisoaVerMisGestiones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VerMisGestiones",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerMisGestiones",
                table: "Role");
        }
    }
}
