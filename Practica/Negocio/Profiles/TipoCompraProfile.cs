using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.TipoCompraModels;

namespace Negocio.Profiles
{


    public class TipoCompraProfile : Profile
    {
        public TipoCompraProfile()
        {
            CreateMap<TipoCompraModel, TipoCompra>();
            CreateMap<TipoCompra, TipoCompraModel>();

            CreateMap<TipoCompra, TipoMonedaSelectModel>();

        }
    }


}
