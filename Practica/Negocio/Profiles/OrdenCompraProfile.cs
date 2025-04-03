using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.OrdenCompraModels;

namespace Negocio.Profiles
{


    public class OrdenCompraProfile : Profile
    {
        public OrdenCompraProfile()
        {
            CreateMap<OrdenCompra, OrdenCompraModel>();
            CreateMap<OrdenCompraModel, OrdenCompra>();

        }
    }


}
