using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dato.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "Solicitud_CDP",
                schema: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "Solicitud_CorrelativoAnual",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "AprobacionConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MontoUTMDesde = table.Column<long>(type: "bigint", nullable: true),
                    MontoUTMHasta = table.Column<long>(type: "bigint", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false),
                    EsParaTodoConceptoPre = table.Column<bool>(type: "bit", nullable: false),
                    AConfigRequeridaId = table.Column<int>(type: "int", nullable: true),
                    RequiereAsignacion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AprobacionConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AprobacionConfig_AprobacionConfig_AConfigRequeridaId",
                        column: x => x.AConfigRequeridaId,
                        principalTable: "AprobacionConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cargo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermiteGenerarOC = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModalidadCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModalidadCompra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramaPresupuestario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramaPresupuestario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertiesEmail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Asunto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesEmail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngresaSolicitud = table.Column<bool>(type: "bit", nullable: false),
                    ApruebaCDP = table.Column<bool>(type: "bit", nullable: false),
                    PuedeAsignar = table.Column<bool>(type: "bit", nullable: false),
                    AdmMantenedores = table.Column<bool>(type: "bit", nullable: false),
                    VeGestionSolicitudes = table.Column<bool>(type: "bit", nullable: false),
                    VerPorFinalizar = table.Column<bool>(type: "bit", nullable: false),
                    IngresaOC = table.Column<bool>(type: "bit", nullable: false),
                    FinalizaSolicitud = table.Column<bool>(type: "bit", nullable: false),
                    ModificaMatrizAprobacion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    SectorPadre = table.Column<int>(type: "int", nullable: true),
                    TienePresupuesto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCompra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoMoneda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaReferencia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UrlCMF = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMoneda", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConceptoPresupuestario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    SectorPertinenciaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptoPresupuestario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptoPresupuestario_Sector_SectorPertinenciaId",
                        column: x => x.SectorPertinenciaId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SectProgPre",
                columns: table => new
                {
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    ProgramaPresupuestarioId = table.Column<int>(type: "int", nullable: false),
                    UserEncargadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectProgPre", x => new { x.SectorId, x.ProgramaPresupuestarioId, x.UserEncargadoId });
                    table.ForeignKey(
                        name: "FK_SectProgPre_ProgramaPresupuestario_ProgramaPresupuestarioId",
                        column: x => x.ProgramaPresupuestarioId,
                        principalTable: "ProgramaPresupuestario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectProgPre_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoId = table.Column<int>(type: "int", nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    JefeDirectoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Cargo_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_User_JefeDirectoId",
                        column: x => x.JefeDirectoId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solicitud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroSolicitud = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolicitanteId = table.Column<int>(type: "int", nullable: true),
                    AprobadorActualId = table.Column<int>(type: "int", nullable: false),
                    UnidadDemandanteId = table.Column<int>(type: "int", nullable: false),
                    ProgramaPresupuestarioId = table.Column<int>(type: "int", nullable: false),
                    IniciativaVigenteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IniciativaVigente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConceptoPresupuestarioId = table.Column<int>(type: "int", nullable: false),
                    FolioRequerimientoSIGFE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoliocompromisoSIGFE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoCompraId = table.Column<int>(type: "int", nullable: false),
                    TipoMonedaId = table.Column<int>(type: "int", nullable: false),
                    NombreCompra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjetivoJustificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MontoAprox = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoUTM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoMultiAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MontoAnhoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ModalidadCompraId = table.Column<int>(type: "int", nullable: false),
                    FechaDerivacionAnalista = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AnalistaProcesoId = table.Column<int>(type: "int", nullable: true),
                    FuncionarioValidacionCDPId = table.Column<int>(type: "int", nullable: true),
                    FechaValidacionCDP = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CDPNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidacionCDP = table.Column<bool>(type: "bit", nullable: false),
                    ContraparteTecnicaId = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrdenCompra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedorNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedorRut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaOrdenCompra = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    FaseCDP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MontoCLP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorDivisa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnalistaPresupuestoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitud", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitud_AprobacionConfig_AprobadorActualId",
                        column: x => x.AprobadorActualId,
                        principalTable: "AprobacionConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitud_ConceptoPresupuestario_ConceptoPresupuestarioId",
                        column: x => x.ConceptoPresupuestarioId,
                        principalTable: "ConceptoPresupuestario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitud_ModalidadCompra_ModalidadCompraId",
                        column: x => x.ModalidadCompraId,
                        principalTable: "ModalidadCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_TipoCompra_TipoCompraId",
                        column: x => x.TipoCompraId,
                        principalTable: "TipoCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_TipoMoneda_TipoMonedaId",
                        column: x => x.TipoMonedaId,
                        principalTable: "TipoMoneda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_User_AnalistaPresupuestoId",
                        column: x => x.AnalistaPresupuestoId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitud_User_AnalistaProcesoId",
                        column: x => x.AnalistaProcesoId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitud_User_ContraparteTecnicaId",
                        column: x => x.ContraparteTecnicaId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitud_User_FuncionarioValidacionCDPId",
                        column: x => x.FuncionarioValidacionCDPId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitud_User_SolicitanteId",
                        column: x => x.SolicitanteId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aprobacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    AprobacionConfigId = table.Column<int>(type: "int", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserAprobadorId = table.Column<int>(type: "int", nullable: false),
                    EstaAprobado = table.Column<bool>(type: "bit", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aprobacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aprobacion_AprobacionConfig_AprobacionConfigId",
                        column: x => x.AprobacionConfigId,
                        principalTable: "AprobacionConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aprobacion_Solicitud_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aprobacion_User_UserAprobadorId",
                        column: x => x.UserAprobadorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Archivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Archivo_Solicitud_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bitacora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SolicitudId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bitacora_Solicitud_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bitacora_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    MontoPresupuestado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    MontoFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoMonedaSel = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudDetalle_Solicitud_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AprobacionConfig",
                columns: new[] { "Id", "AConfigRequeridaId", "EsParaTodoConceptoPre", "EstaActivo", "MontoUTMDesde", "MontoUTMHasta", "Nombre", "Orden", "Quien", "RequiereAsignacion" },
                values: new object[,]
                {
                    { 1, null, true, true, -1L, -1L, "Obtencion del CDP Analista", 2, "select Id from reqCompra..[User] where SectorId = 231 and CargoId = 2", false },
                    { 3, null, true, true, -1L, -1L, "Jefe Directo", 4, "select JefeDirectoId AS 'Id' from reqCompra..[User] where Id = @UserId -- Autorizador nivel 1", false },
                    { 10, null, true, true, 0L, 30L, "Aprobador Jefe Administración", 5, "select Id from reqCompra..[User] where SectorId = 229 and CargoId = 14 --Pertinencia 2", false },
                    { 1012, null, true, true, null, null, "Analista de Compra", 1, null, true },
                    { 1015, null, true, true, 193L, -1L, "Obtencion del CDP JEFA/E", 3, "select Id from reqCompra..[User] where SectorId = 231 and CargoId = 14  ", false }
                });

            migrationBuilder.InsertData(
                table: "Cargo",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 17, "CONDUCTOR/A" },
                    { 16, "AUXILIAR " },
                    { 14, "JEFA/E" },
                    { 10, "Seremi" },
                    { 9, "Subsecretario" },
                    { 15, "ASESOR/A" },
                    { 6, "Asistente Administrativo" },
                    { 5, "Jefe Unidad" },
                    { 3, "Tecnico" },
                    { 2, "PROFESIONAL" },
                    { 8, "MINISTRA/O" }
                });

            migrationBuilder.InsertData(
                table: "Estado",
                columns: new[] { "Id", "Nombre", "PermiteGenerarOC" },
                values: new object[,]
                {
                    { 11, "Anulada", false },
                    { 10, "Finalizada", false },
                    { 9, "Generando OC", true },
                    { 8, "Asignada", false },
                    { 4, "Rechazada en aprobación", false },
                    { 2, "En proceso de aprobacion", false },
                    { 1, "Creada", false },
                    { 3, "Aprobada", true }
                });

            migrationBuilder.InsertData(
                table: "ModalidadCompra",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Anual" },
                    { 2, "Multianual" }
                });

            migrationBuilder.InsertData(
                table: "ProgramaPresupuestario",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 6, "FONDOS NO AFECTOS A LEY (EXTRAPRESUPUESTARIOS)" },
                    { 4, "Programa 04" },
                    { 5, "Programa 05" },
                    { 1, "Programa 01" },
                    { 3, "Programa 03" }
                });

            migrationBuilder.InsertData(
                table: "PropertiesEmail",
                columns: new[] { "Id", "Asunto", "Cc", "Cco", "From", "FromNombre", "Mensaje", "Nombre" },
                values: new object[,]
                {
                    { 1, "Notifica Aprobación en Solicitud de Compra N° $Id", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" fue aprobada por $Usuario  <br/>", "APROBACION" },
                    { 2, "Notifica Creación en Solicitud de Compra N° $Id", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, se ha creado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" por $Usuario <br/>", "CREACION" },
                    { 3, "Solicitud de Compra N° $Id asignada", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, se le ha asignado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" <br/>", "ASIGNACION" },
                    { 4, "Notifica Creación de CDP en Solicitud de Compra N° $Id", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, se ha creado CDP correspondiente a la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" por $Usuario <br/>", "CDP" },
                    { 5, "Solicitud de Compra N° $Id requiere de su aprobación", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" requiere de su aprobación<br/>", "SIGUIENTE" },
                    { 6, "Solicitud de Compra N° $Id rechazada", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" fue rechazada por $Usuario <br/>", "RECHAZA" },
                    { 7, "Solicitud de Compra N° $Id anulada", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" fue anulada por $Usuario <br/>", "ANULA" },
                    { 8, "Notifica proceso de Solicitud de Compra N° $Id Finalizado", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, ha finalizado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\".<br>La diferencia de montos para los años es el siguietne: <br>$foreachMontos<br/>", "FINALIZA" },
                    { 9, "Notifica Modificación en Solicitud de Compra N° $Id", null, null, "req-compra@minenergia.cl", "ReqCompra", "Por este medio se comunica que, con fecha $fecha, se ha modificado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" por $Usuario <br/>", "MODIFICADA" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "AdmMantenedores", "ApruebaCDP", "FinalizaSolicitud", "IngresaOC", "IngresaSolicitud", "ModificaMatrizAprobacion", "Nombre", "PuedeAsignar", "VeGestionSolicitudes", "VerPorFinalizar" },
                values: new object[,]
                {
                    { 4, false, false, true, true, true, true, "Asignador de Solicitud", true, true, true },
                    { 6, false, false, false, false, true, false, "Funcionario solicitante", false, true, false }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "AdmMantenedores", "ApruebaCDP", "FinalizaSolicitud", "IngresaOC", "IngresaSolicitud", "ModificaMatrizAprobacion", "Nombre", "PuedeAsignar", "VeGestionSolicitudes", "VerPorFinalizar" },
                values: new object[,]
                {
                    { 5, false, false, true, true, true, false, "Analista de compras", true, true, true },
                    { 2, false, true, false, false, true, false, "Analista de presupuesto", false, true, true },
                    { 1, true, true, true, true, true, true, "Administrador", true, true, false },
                    { 3, false, true, false, false, true, false, "Jefe Presupuesto", false, true, true }
                });

            migrationBuilder.InsertData(
                table: "TipoCompra",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 8, "Compra Coordinada" },
                    { 1, "Convenio Marco" },
                    { 2, "Gran compra" },
                    { 3, "Compra ágil" },
                    { 4, "Trato directo" },
                    { 5, "Licitación Pública" },
                    { 6, "Licitación privada" },
                    { 7, "Gastos de representación" }
                });

            migrationBuilder.InsertData(
                table: "TipoMoneda",
                columns: new[] { "Id", "Codigo", "Estado", "FechaReferencia", "FechaSolicitud", "Nombre", "UrlCMF", "Valor" },
                values: new object[,]
                {
                    { 3, "USD", true, null, null, "Dolar", "https://api.sbif.cl/api-sbifv3/recursos_api/dolar?apikey={0}&formato=xml", 720.44m },
                    { 4, "EUR", true, null, null, "Euro", "https://api.sbif.cl/api-sbifv3/recursos_api/euro?apikey={0}&formato=xml", 879.66m },
                    { 1, "UTM", true, null, null, "UTM", "https://api.sbif.cl/api-sbifv3/recursos_api/utm?apikey={0}&formato=xml", 52005.00m },
                    { 2, "UF", true, null, null, "UF", "https://api.sbif.cl/api-sbifv3/recursos_api/uf?apikey={0}&formato=xml", 29624.70m },
                    { 5, "CLP", false, null, null, "Peso Chileno", null, 1.00m }
                });

            migrationBuilder.InsertData(
                table: "AprobacionConfig",
                columns: new[] { "Id", "AConfigRequeridaId", "EsParaTodoConceptoPre", "EstaActivo", "MontoUTMDesde", "MontoUTMHasta", "Nombre", "Orden", "Quien", "RequiereAsignacion" },
                values: new object[] { 4, 10, true, true, 30L, 350L, "Jefe DAF", 6, "select Id from reqCompra..[User] where SectorId = 228 and CargoId = 14 --JEFE DAF", false });

            migrationBuilder.InsertData(
                table: "AprobacionConfig",
                columns: new[] { "Id", "AConfigRequeridaId", "EsParaTodoConceptoPre", "EstaActivo", "MontoUTMDesde", "MontoUTMHasta", "Nombre", "Orden", "Quien", "RequiereAsignacion" },
                values: new object[] { 6, 4, true, true, 350L, -1L, "Aprobador Gabinete", 7, "select Id from reqCompra..[User] where SectorId in (237, 241) and CargoId in (9, 8) -- ministro y subse", false });

            migrationBuilder.CreateIndex(
                name: "IX_Aprobacion_AprobacionConfigId",
                table: "Aprobacion",
                column: "AprobacionConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Aprobacion_SolicitudId",
                table: "Aprobacion",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Aprobacion_UserAprobadorId",
                table: "Aprobacion",
                column: "UserAprobadorId");

            migrationBuilder.CreateIndex(
                name: "IX_AprobacionConfig_AConfigRequeridaId",
                table: "AprobacionConfig",
                column: "AConfigRequeridaId");

            migrationBuilder.CreateIndex(
                name: "IX_Archivo_SolicitudId",
                table: "Archivo",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_SolicitudId",
                table: "Bitacora",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_UserId",
                table: "Bitacora",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoPresupuestario_SectorPertinenciaId",
                table: "ConceptoPresupuestario",
                column: "SectorPertinenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_SectProgPre_ProgramaPresupuestarioId",
                table: "SectProgPre",
                column: "ProgramaPresupuestarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_AnalistaPresupuestoId",
                table: "Solicitud",
                column: "AnalistaPresupuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_AnalistaProcesoId",
                table: "Solicitud",
                column: "AnalistaProcesoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_AprobadorActualId",
                table: "Solicitud",
                column: "AprobadorActualId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_ConceptoPresupuestarioId",
                table: "Solicitud",
                column: "ConceptoPresupuestarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_ContraparteTecnicaId",
                table: "Solicitud",
                column: "ContraparteTecnicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_EstadoId",
                table: "Solicitud",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_FuncionarioValidacionCDPId",
                table: "Solicitud",
                column: "FuncionarioValidacionCDPId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_ModalidadCompraId",
                table: "Solicitud",
                column: "ModalidadCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_SolicitanteId",
                table: "Solicitud",
                column: "SolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_TipoCompraId",
                table: "Solicitud",
                column: "TipoCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_TipoMonedaId",
                table: "Solicitud",
                column: "TipoMonedaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudDetalle_SolicitudId",
                table: "SolicitudDetalle",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CargoId",
                table: "User",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_User_JefeDirectoId",
                table: "User",
                column: "JefeDirectoId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SectorId",
                table: "User",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aprobacion");

            migrationBuilder.DropTable(
                name: "Archivo");

            migrationBuilder.DropTable(
                name: "Bitacora");

            migrationBuilder.DropTable(
                name: "PropertiesEmail");

            migrationBuilder.DropTable(
                name: "SectProgPre");

            migrationBuilder.DropTable(
                name: "SolicitudDetalle");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "ProgramaPresupuestario");

            migrationBuilder.DropTable(
                name: "Solicitud");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "AprobacionConfig");

            migrationBuilder.DropTable(
                name: "ConceptoPresupuestario");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "ModalidadCompra");

            migrationBuilder.DropTable(
                name: "TipoCompra");

            migrationBuilder.DropTable(
                name: "TipoMoneda");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Cargo");

            migrationBuilder.DropTable(
                name: "Sector");

            migrationBuilder.DropSequence(
                name: "Solicitud_CDP",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "Solicitud_CorrelativoAnual",
                schema: "dbo");
        }
    }
}
