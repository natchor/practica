using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Negocio.Profiles;
using Web.ReqCompra;

namespace Web.Configuration
{
    public class ProfileConfiguration
    {
        public static void Register(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<SolicitudProfile>();
                cfg.AddProfile<ArchivoProfile>();
                cfg.AddProfile<BitacoraProfile>();
                cfg.AddProfile<CargoProfile>();
                cfg.AddProfile<ConceptoPresupuestarioProfile>();
                cfg.AddProfile<EstadoProfile>();
                cfg.AddProfile<ModalidadCompraProfile>();
                cfg.AddProfile<OrdenCompraProfile>();
                cfg.AddProfile<ProgramaPresupuestarioProfile>();
                cfg.AddProfile<SectorProfile>();
                cfg.AddProfile<TipoCompraProfile>();
                cfg.AddProfile<TipoMonedaProfile>();
                cfg.AddProfile<RoleProfile>();
                cfg.AddProfile<UserRoleProfile>();
                cfg.AddProfile<PropertiesEmailProfile>();
                cfg.AddProfile<AprobacionProfile>();
                cfg.AddProfile<AprobacionConfigProfile>();
                cfg.AddProfile<FeriadoChileProfile>();
                cfg.AddProfile<EstadoCompraProfile>();
                cfg.AddProfile<PropertiesSystemProfile>();
                cfg.AddProfile<ConvenioProfile>();



            });

            var mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
            services.AddAutoMapper(typeof(Startup));
        }
    }
}
