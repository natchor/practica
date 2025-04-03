using Dato.Interfaces.Repositories;
using Dato.Repositories;
using Dato.Respositories;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Configuration
{
    public class RepositoryConfiguration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IArchivoRepository, ArchivoRepository>();
            services.AddScoped<ISolicitudRepository, SolicitudRepository>();
            services.AddScoped<IBitacoraRepository, BitacoraRepository>();
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IConceptoPresupuestarioRepository, ConceptoPresupuestarioRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IModalidadCompraRepository, ModalidadCompraRepository>();
            services.AddScoped<IOrdenCompraRepository, OrdenCompraRepository>();
            services.AddScoped<IProgramaPresupuestarioRepository, ProgramaPresupuestarioRepository>();
            services.AddScoped<ISectorRepository, SectorRepository>();
            services.AddScoped<ITipoCompraRepository, TipoCompraRepository>();
            services.AddScoped<ITipoMonedaRepository, TipoMonedaRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAprobacionConfigRepository, AprobacionConfigRepository>();
            services.AddScoped<IAprobacionRepository, AprobacionRepository>();
            services.AddScoped<IPropertiesEmailRepository, PropertiesEmailRepository>();
            services.AddScoped<ISectProgPreRepository, SectProgPreRepository>();
            services.AddScoped<ISolicitudDetalleRepository, SolicitudDetalleRepository>();
            services.AddScoped<IFeriadoChileRepository, FeriadoChileRepository>();
            services.AddScoped<IEstadoCompraRepository, EstadoCompraRepository>();
            services.AddScoped<IStoredProcedureRepository, StoredProcedureRepository>();
            services.AddScoped<IPropertiesSystemRepository, PropertiesSystemRepository>();
            services.AddScoped<IConvenioRepository, ConvenioRepository>();




        }
    }
}
