using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class convenio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Convenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    AutorizadorFinId = table.Column<int>(type: "int", nullable: true),
                    AutorizadorPresId = table.Column<int>(type: "int", nullable: true),
                    Antecedente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuentaCorriente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaldoCuenta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificadoSaldo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAutorizacionFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAutorizacionPres = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Convenio_Solicitud_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Convenio_SolicitudId",
                table: "Convenio",
                column: "SolicitudId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Convenio");
        }
    }
}
