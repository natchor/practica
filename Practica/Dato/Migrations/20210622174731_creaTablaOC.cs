using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class creaTablaOC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    CantidadEvaluacion = table.Column<int>(type: "int", nullable: false),
                    CargoContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cargos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoCategoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoEstadoProveedor = table.Column<int>(type: "int", nullable: false),
                    CodigoOC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoProducto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoSucursal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComunaProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descuentos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DireccionProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EspecificacionComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EspecificacionProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAceptacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCancelacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Financiamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FonoContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FonoContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Impuestos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListadoCorrelativo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreSucursal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaisProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PorcentajeIva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromedioCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedorCodigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutSucursal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TieneItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCargos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuentos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalImpuestos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNeto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unidad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenCompra", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_UnidadDemandanteId",
                table: "Solicitud",
                column: "UnidadDemandanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitud_Sector_UnidadDemandanteId",
                table: "Solicitud",
                column: "UnidadDemandanteId",
                principalTable: "Sector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitud_Sector_UnidadDemandanteId",
                table: "Solicitud");

            migrationBuilder.DropTable(
                name: "OrdenCompra");

            migrationBuilder.DropIndex(
                name: "IX_Solicitud_UnidadDemandanteId",
                table: "Solicitud");
        }
    }
}
