using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.SectorModels;

namespace Negocio.Profiles
{


    public class SectorProfile : Profile
    {
        public SectorProfile()
        {
            CreateMap<SectorModel, Sector>()
                .ForMember(dest => dest.SectorPadre, opt => opt.MapFrom(x => x.SectorPadreId));
            
            CreateMap<Sector, SectorModel>()
                .ForMember(dest => dest.SectorPadreId, opt => opt.MapFrom(x => x.SectorPadre));
            
            CreateMap<Sector, SelectSectorModel>();

        }
    }


}
