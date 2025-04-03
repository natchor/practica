using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.ConvenioModels;

namespace Negocio.Profiles
{


    public class ConvenioProfile : Profile
    {
        public ConvenioProfile()
        {
            CreateMap<ConvenioModel, Convenio>();
            CreateMap<Convenio, ConvenioModel>();

        }
    }


}
