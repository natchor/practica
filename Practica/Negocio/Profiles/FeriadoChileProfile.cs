

using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.FeriadoChileModels;

namespace Negocio.Profiles
{


    public class FeriadoChileProfile : Profile
    {
        public FeriadoChileProfile()
        {
            CreateMap<FeriadoChileModel, FeriadoChile>();
            CreateMap<FeriadoChile, FeriadoChileModel>();


        }
    }


}
