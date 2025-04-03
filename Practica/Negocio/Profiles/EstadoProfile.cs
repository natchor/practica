using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.EstadoModels;

namespace Negocio.Profiles
{


    public class EstadoProfile : Profile
    {
        public EstadoProfile()
        {
            CreateMap<EstadoModel, Estado>();
            CreateMap<Estado, EstadoModel>();



            CreateMap<EstadoSelectModel, Estado>();
            CreateMap<Estado, EstadoSelectModel>();

            CreateMap<EstadoSelect2Model, Estado>();
            CreateMap<Estado, EstadoSelect2Model>();

        }
    }


}
