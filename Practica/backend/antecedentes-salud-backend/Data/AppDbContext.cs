using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using antecedentes_salud_backend.Models;

namespace antecedentes_salud_backend.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Antecedentes> Fichas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QR> QRs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Antecedentes>().HasKey(f => f.RutCon);
            modelBuilder.Entity<QR>().HasKey(q => q.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}