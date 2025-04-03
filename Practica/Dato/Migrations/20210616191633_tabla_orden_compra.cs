using Microsoft.EntityFrameworkCore.Migrations;

namespace Dato.Migrations
{
    public partial class tabla_orden_compra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoOC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoEstado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoLicitacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoEstadoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEnvio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAceptacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCancelacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaUltimaModificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TieneItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromedioCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadEvaluacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descuentos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cargos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalNeto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PorcentajeIva = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Impuestos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Financiamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaisOC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDespacho = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompradorCodigoOrganismo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreOrganismo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Actividad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DireccionUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComunaUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionUnidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaisOrganismo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FonoContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedorCodigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActividadProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoSucursal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreSucursal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutSucursal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comuna = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaisProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FonoContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailContactoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemsCantidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListadoCorrelativo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoCategoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoProducto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EspecificacionComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EspecificacionProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecioNeto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCargos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDescuentos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalImpuestos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenCompra", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenCompra");
        }
    }
}
