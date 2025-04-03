using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.PropertiesSystemModels;

namespace Negocio.Profiles
{


    public class PropertiesSystemProfile : Profile
    {
        public PropertiesSystemProfile()
        {
            CreateMap<PropertiesSystemModel, PropertiesSystem>();
            CreateMap<PropertiesSystem, PropertiesSystemModel>();

        }
    }


}
