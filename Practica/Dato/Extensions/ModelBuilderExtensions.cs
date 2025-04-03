using Dato.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dato.Extensions
{
    public static class ModelBuilderExtensions
    {

        /// <summary>
        /// Para incluir al crear la base de datos como datos iniciales
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AprobacionConfig>().HasData(
               new AprobacionConfig { Id = 1, Nombre = "Obtencion del CDP Analista", Quien = "select Id from reqCompra..[User] where SectorId = 231 and CargoId = 2", Orden = 2, EstaActivo = true, MontoUTMDesde = -1, MontoUTMHasta = -1, EsParaTodoConceptoPre = true, AConfigRequeridaId = null, RequiereAsignacion = false },
               new AprobacionConfig { Id = 3, Nombre = "Jefe Directo", Quien = "select JefeDirectoId AS 'Id' from reqCompra..[User] where Id = @UserId -- Autorizador nivel 1", Orden = 4, EstaActivo = true, MontoUTMDesde = -1, MontoUTMHasta = -1, EsParaTodoConceptoPre = true, AConfigRequeridaId = null, RequiereAsignacion = false },
               new AprobacionConfig { Id = 4, Nombre = "Jefe DAF", Quien = "select Id from reqCompra..[User] where SectorId = 228 and CargoId = 14 --JEFE DAF", Orden = 6, EstaActivo = true, MontoUTMDesde = 30, MontoUTMHasta = 350, EsParaTodoConceptoPre = true, AConfigRequeridaId = 10, RequiereAsignacion = false },
               new AprobacionConfig { Id = 6, Nombre = "Aprobador Gabinete", Quien = "select Id from reqCompra..[User] where SectorId in (237, 241) and CargoId in (9, 8) -- ministro y subse", Orden = 7, EstaActivo = true, MontoUTMDesde = 350, MontoUTMHasta = -1, EsParaTodoConceptoPre = true, AConfigRequeridaId = 4, RequiereAsignacion = false },
               new AprobacionConfig { Id = 10, Nombre = "Aprobador Jefe Administración", Quien = "select Id from reqCompra..[User] where SectorId = 229 and CargoId = 14 --Pertinencia 2", Orden = 5, EstaActivo = true, MontoUTMDesde = 0, MontoUTMHasta = 30, EsParaTodoConceptoPre = true, AConfigRequeridaId = null, RequiereAsignacion = false },
               new AprobacionConfig { Id = 1012, Nombre = "Analista de Compra", Quien = null, Orden = 1, EstaActivo = true, MontoUTMDesde = null, MontoUTMHasta = null, EsParaTodoConceptoPre = true, AConfigRequeridaId = null, RequiereAsignacion = true },
               new AprobacionConfig { Id = 1015, Nombre = "Obtencion del CDP JEFA/E", Quien = "select Id from reqCompra..[User] where SectorId = 231 and CargoId = 14  ", Orden = 3, EstaActivo = true, MontoUTMDesde = 193, MontoUTMHasta = -1, EsParaTodoConceptoPre = true, AConfigRequeridaId = null, RequiereAsignacion = false }
           );


            modelBuilder.Entity<Cargo>().HasData(
                new Cargo { Id = 2, Nombre = "PROFESIONAL" },
                new Cargo { Id = 3, Nombre = "Tecnico" },
                new Cargo { Id = 5, Nombre = "Jefe Unidad" },
                new Cargo { Id = 6, Nombre = "Asistente Administrativo" },
                new Cargo { Id = 8, Nombre = "MINISTRA/O" },
                new Cargo { Id = 9, Nombre = "Subsecretario" },
                new Cargo { Id = 10, Nombre = "Seremi" },
                new Cargo { Id = 14, Nombre = "JEFA/E" },
                new Cargo { Id = 15, Nombre = "ASESOR/A" },
                new Cargo { Id = 16, Nombre = "AUXILIAR " },
                new Cargo { Id = 17, Nombre = "CONDUCTOR/A" }
             );

            modelBuilder.Entity<Estado>().HasData(
                new Estado { Id = 1, Nombre = "Creada", PermiteGenerarOC = false },
                new Estado { Id = 2, Nombre = "En proceso de aprobacion", PermiteGenerarOC = false },
                new Estado { Id = 3, Nombre = "Aprobada", PermiteGenerarOC = true },
                new Estado { Id = 4, Nombre = "Rechazada en aprobación", PermiteGenerarOC = false },
                new Estado { Id = 8, Nombre = "Asignada", PermiteGenerarOC = false },
                new Estado { Id = 9, Nombre = "Generando OC", PermiteGenerarOC = true },
                new Estado { Id = 10, Nombre = "Finalizada", PermiteGenerarOC = false },
                new Estado { Id = 11, Nombre = "Anulada", PermiteGenerarOC = false }

            );


            modelBuilder.Entity<ModalidadCompra>().HasData(
                new ModalidadCompra { Id = 1, Nombre = "Anual" },
                new ModalidadCompra { Id = 2, Nombre = "Multianual" }

            );



            modelBuilder.Entity<ProgramaPresupuestario>().HasData(
                new ProgramaPresupuestario { Id = 1, Nombre = "Programa 01" },
                new ProgramaPresupuestario { Id = 3, Nombre = "Programa 03" },
                new ProgramaPresupuestario { Id = 4, Nombre = "Programa 04" },
                new ProgramaPresupuestario { Id = 5, Nombre = "Programa 05" },
                new ProgramaPresupuestario { Id = 6, Nombre = "FONDOS NO AFECTOS A LEY (EXTRAPRESUPUESTARIOS)" }
           );



            modelBuilder.Entity<PropertiesEmail>().HasData(
                new PropertiesEmail { Id = 1, Nombre = "APROBACION", Asunto = "Notifica Aprobación en Solicitud de Compra N° $Id", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" fue aprobada por $Usuario  <br/>" },
                new PropertiesEmail { Id = 2, Nombre = "CREACION", Asunto = "Notifica Creación en Solicitud de Compra N° $Id", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, se ha creado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" por $Usuario <br/>" },
                new PropertiesEmail { Id = 3, Nombre = "ASIGNACION", Asunto = "Solicitud de Compra N° $Id asignada", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, se le ha asignado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" <br/>" },
                new PropertiesEmail { Id = 4, Nombre = "CDP", Asunto = "Notifica Creación de CDP en Solicitud de Compra N° $Id", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, se ha creado CDP correspondiente a la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" por $Usuario <br/>" },
                new PropertiesEmail { Id = 5, Nombre = "SIGUIENTE", Asunto = "Solicitud de Compra N° $Id requiere de su aprobación", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" requiere de su aprobación<br/>" },
                new PropertiesEmail { Id = 6, Nombre = "RECHAZA", Asunto = "Solicitud de Compra N° $Id rechazada", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" fue rechazada por $Usuario <br/>" },
                new PropertiesEmail { Id = 7, Nombre = "ANULA", Asunto = "Solicitud de Compra N° $Id anulada", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" fue anulada por $Usuario <br/>" },
                new PropertiesEmail { Id = 8, Nombre = "FINALIZA", Asunto = "Notifica proceso de Solicitud de Compra N° $Id Finalizado", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, ha finalizado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\".<br>La diferencia de montos para los años es el siguietne: <br>$foreachMontos<br/>" },
                new PropertiesEmail { Id = 9, Nombre = "MODIFICADA", Asunto = "Notifica Modificación en Solicitud de Compra N° $Id", From = "req-compra@minenergia.cl", FromNombre = "ReqCompra", Mensaje = "Por este medio se comunica que, con fecha $fecha, se ha modificado la Solicitud de compra N° $Id, con nombre \"$NombreCompra\" por $Usuario <br/>" }

           );

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, Nombre = "Administrador", IngresaSolicitud = true, PuedeAsignar = true, AdmMantenedores = true, VeGestionSolicitudes = true, FinalizaSolicitud = true, ApruebaCDP = true, IngresaOC = true, ModificaMatrizAprobacion = true },
               new Role { Id = 2, Nombre = "Analista de presupuesto", IngresaSolicitud = true, ApruebaCDP = true, VeGestionSolicitudes = true, VerPorFinalizar = true },
               new Role { Id = 3, Nombre = "Jefe Presupuesto", IngresaSolicitud = true, ApruebaCDP = true, VeGestionSolicitudes = true, VerPorFinalizar = true },
               new Role { Id = 4, Nombre = "Asignador de Solicitud", IngresaSolicitud = true, PuedeAsignar = true, VeGestionSolicitudes = true, IngresaOC = true, FinalizaSolicitud = true, ModificaMatrizAprobacion = true, VerPorFinalizar = true },
               new Role { Id = 5, Nombre = "Analista de compras", IngresaSolicitud = true, VeGestionSolicitudes = true, IngresaOC = true, FinalizaSolicitud = true, PuedeAsignar = true, VerPorFinalizar = true },
               new Role { Id = 6, Nombre = "Funcionario solicitante", IngresaSolicitud = true, VeGestionSolicitudes = true }
           );


            modelBuilder.Entity<TipoCompra>().HasData(
               new TipoCompra { Id = 1, Nombre = "Convenio Marco" },
               new TipoCompra { Id = 2, Nombre = "Gran compra" },
               new TipoCompra { Id = 3, Nombre = "Compra ágil" },
               new TipoCompra { Id = 4, Nombre = "Trato directo" },
               new TipoCompra { Id = 5, Nombre = "Licitación Pública" },
               new TipoCompra { Id = 6, Nombre = "Licitación privada" },
               new TipoCompra { Id = 7, Nombre = "Gastos de representación" },
               new TipoCompra { Id = 8, Nombre = "Compra Coordinada" }

            );

            modelBuilder.Entity<TipoMoneda>().HasData(
              new TipoMoneda { Id = 1, Nombre = "UTM", Codigo = "UTM", Estado = true, Valor = 52005.00m, UrlCMF = "https://api.sbif.cl/api-sbifv3/recursos_api/utm?apikey={0}&formato=xml" },
              new TipoMoneda { Id = 2, Nombre = "UF", Codigo = "UF", Estado = true, Valor = 29624.70m, UrlCMF = "https://api.sbif.cl/api-sbifv3/recursos_api/uf?apikey={0}&formato=xml" },
              new TipoMoneda { Id = 3, Nombre = "Dolar", Codigo = "USD", Estado = true, Valor = 720.44m, UrlCMF = "https://api.sbif.cl/api-sbifv3/recursos_api/dolar?apikey={0}&formato=xml" },
              new TipoMoneda { Id = 4, Nombre = "Euro", Codigo = "EUR", Estado = true, Valor = 879.66m, UrlCMF = "https://api.sbif.cl/api-sbifv3/recursos_api/euro?apikey={0}&formato=xml" },
              new TipoMoneda { Id = 5, Nombre = "Peso Chileno", Codigo = "CLP", Estado = false, Valor = 1.00m, UrlCMF = null }

            );
        }

        public static void Sequences(this ModelBuilder modelBuilder)
        {

            //CREATE SEQUENCE[dbo].[Solicitud_CDP]
            //AS[bigint]
            // START WITH 1
            // INCREMENT BY 1
            // MINVALUE - 9223372036854775808
            // MAXVALUE 9223372036854775807
            // CACHE
            //GO

            modelBuilder.HasSequence<int>("Solicitud_CDP", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);

            modelBuilder.HasSequence<int>("Solicitud_CorrelativoAnual", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);
        }

        public static void Keys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(sc => new { sc.UserId, sc.RoleId });
            modelBuilder.Entity<SectProgPre>().HasKey(sc => new { sc.SectorId, sc.ProgramaPresupuestarioId, sc.UserEncargadoId });
            //modelBuilder.Entity<Aprobacion>().HasKey(sc => new { sc.SolicitudId, sc.UserAprobadorId });

        }

        public static void Properties(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Solicitud>()
                .Property(p => p.MontoAprox)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Solicitud>()
                .Property(p => p.MontoUTM)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<TipoMoneda>()
                .Property(p => p.Valor)
                .HasColumnType("decimal(18,2)");
        }

        public static void DefaultValues(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Archivo>().Property(x => x.FechaCreacion).HasDefaultValue("getdate()");
        }

        public static void Relations(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Aprobacion>()
            //    .HasOne(b => b.Solicitud)
            //    .WithMany(a => a.Aprobaciones)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Solicitud>()
               .HasOne(b => b.AprobadorActual)
               .WithMany(a => a.Solicitudes)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Solicitud>()
              .HasOne(b => b.Estado)
              .WithMany(a => a.Solicitudes)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
               .HasOne(b => b.JefeDirecto)
               .WithMany(a => a.Users)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Aprobacion>()
              .HasOne(b => b.AprobacionConfig)
              .WithMany(a => a.Aprobaciones)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Solicitud>()
           .HasOne(b => b.UnidadDemandante)
           .WithMany(a => a.SolicitudesUnidadesDemandantes)
           .OnDelete(DeleteBehavior.Restrict);
            // Pendiente: 30/03/2021 JCP
            //modelBuilder.Entity<User>()
            //    .HasIndex(u => u.Email)
            //    .IsUnique();

            //modelBuilder.Entity<User>()
            //    .HasIndex(u => u.UserName)
            //    .IsUnique();
        }
    }
}
