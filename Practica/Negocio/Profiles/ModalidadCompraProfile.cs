using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.ModalidadCompraModels;

namespace Negocio.Profiles
{


    public class ModalidadCompraProfile : Profile
    {
        public ModalidadCompraProfile()
        {
            CreateMap<ModalidadCompra, ModalidadCompraModel>();
            CreateMap<ModalidadCompraModel, ModalidadCompra>();

            CreateMap<ModalidadCompra, ModalidadCompraSelectModel>();
        }
    }


}
