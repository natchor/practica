using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.EstadoCompraModels;

namespace Negocio.Profiles
{


    public class EstadoCompraProfile : Profile
    {
        public EstadoCompraProfile()
        {
            CreateMap<EstadoCompraModel, EstadoCompra>();
            CreateMap<EstadoCompra, EstadoCompraModel>();


        }
    }


}
