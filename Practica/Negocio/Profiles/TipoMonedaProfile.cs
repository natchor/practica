using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.TipoMonedaModels;

namespace Negocio.Profiles
{


    public class TipoMonedaProfile : Profile
    {
        public TipoMonedaProfile()
        {
            CreateMap<TipoMonedaModel, TipoMoneda>();
            CreateMap<TipoMoneda, TipoMonedaModel>();
            CreateMap<TipoMoneda, SelectTipoMonedaModel>();

        }
    }


}
