using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.ConceptoPresupuestarioModels;

namespace Negocio.Profiles
{


    public class ConceptoPresupuestarioProfile : Profile
    {
        public ConceptoPresupuestarioProfile()
        {
            CreateMap<ConceptoPresupuestario, ConceptoPresupuestarioModel>();
            CreateMap<ConceptoPresupuestarioModel, ConceptoPresupuestario>();
            CreateMap<ConceptoPresupuestario, SelectConceptoPresupuestarioModel>();

        }
    }


}
