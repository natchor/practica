using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.ProgramaPresupuestarioModels;

namespace Negocio.Profiles
{


    public class ProgramaPresupuestarioProfile : Profile
    {
        public ProgramaPresupuestarioProfile()
        {
            CreateMap<ProgramaPresupuestarioModel, ProgramaPresupuestario>();
            CreateMap<ProgramaPresupuestario, ProgramaPresupuestarioModel>();

            CreateMap<ProgramaPresupuestario, ProgPresSelectModel>();


        }
    }


}
