using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.BitacoraModels;

namespace Negocio.Profiles
{


    public class BitacoraProfile : Profile
    {
        public BitacoraProfile()
        {
            CreateMap<BitacoraModel, Bitacora>();
            CreateMap<Bitacora, BitacoraModel>();

        }
    }


}




