using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.AprobacionConfigModels;

namespace Negocio.Profiles
{
    public class AprobacionConfigProfile : Profile
    {
        public AprobacionConfigProfile()
        {

            CreateMap<AprobacionConfigModel, AprobacionConfig>();
            CreateMap<AprobacionConfig, AprobacionConfigModel>();
            CreateMap<AprobacionConfig, SelectAprobacionConfigModel>();



        }
    }
}
