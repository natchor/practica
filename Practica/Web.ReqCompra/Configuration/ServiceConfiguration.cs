using Microsoft.Extensions.DependencyInjection;
using Negocio.Interfaces.Services;
using Negocio.Services;

namespace Web.Configuration
{
    public class ServiceConfiguration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IArchivoService, ArchivoService>();
            services.AddScoped<IBitacoraService, BitacoraService>();
            services.AddScoped<ISolicitudService, SolicitudService>();
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IConceptoPresupuestarioService, ConceptoPresupuestarioService>();
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<IModalidadCompraService, ModalidadCompraService>();
            services.AddScoped<IOrdenCompraService, OrdenCompraService>();
            services.AddScoped<IProgramaPresupuestarioService, ProgramaPresupuestarioService>();
            services.AddScoped<ITipoCompraService, TipoCompraService>();
            services.AddScoped<ITipoMonedaService, TipoMonedaService>();
            services.AddScoped<ISectorService, SectorService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IPropertiesEmailService, PropertiesEmailService>();
            services.AddScoped<IAprobacionConfigService, AprobacionConfigService>();
            services.AddScoped<IFeriadoChileService, FeriadoChileService>();
            services.AddScoped<IEstadoCompraService, EstadoCompraService>();
            services.AddScoped<IPropertiesSystemService, PropertiesSystemService>();
            services.AddScoped<IConvenioService, ConvenioService>();

        }
    }
}
