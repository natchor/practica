using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class CamposFechaContrato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFinContrato",
                table: "Solicitud",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioContrato",
                table: "Solicitud",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFinContrato",
                table: "Solicitud");

            migrationBuilder.DropColumn(
                name: "FechaInicioContrato",
                table: "Solicitud");
        }
    }
}
