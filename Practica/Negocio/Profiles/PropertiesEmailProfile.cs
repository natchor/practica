
using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.PropertiesEmailModels;

namespace Negocio.Profiles
{


    public class PropertiesEmailProfile : Profile
    {
        public PropertiesEmailProfile()
        {
            CreateMap<PropertiesEmailModel, PropertiesEmail>();
            CreateMap<PropertiesEmail, PropertiesEmailModel>();

        }
    }


}
