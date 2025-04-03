using Dato.Entities;
using Dato.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dato
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Keys();

            modelBuilder.Seed();

            modelBuilder.Sequences();

            modelBuilder.Properties();

            modelBuilder.Relations();

            modelBuilder.DefaultValues();
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Aprobacion> Aprobacion { get; set; }
        public DbSet<TipoMoneda> TipoMoneda { get; set; }
        public DbSet<Solicitud> Solicitud { get; set; }
        public DbSet<ConceptoPresupuestario> ConceptoPresupuestario { get; set; }
        public DbSet<Sector> Sector { get; set; }
        public DbSet<TipoCompra> TipoCompra { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<ProgramaPresupuestario> ProgramaPresupuestario { get; set; }
        public DbSet<PropertiesEmail> PropertiesEmail { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<EstadoCompra> EstadoCompra { get; set; }
        public DbSet<AprobacionConfig> AprobacionConfig { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }
        public DbSet<OrdenCompra> OrdenCompra { get; set; }
        public DbSet<FeriadoChile> FeriadoChile { get; set; }
        public DbSet<StoredProcedure> StoredProcedure { get; set; }
        public DbSet<PropertiesSystem> PropertiesSystem { get; set; }
        public DbSet<Convenio> Convenio{ get; set; }

    }
}
