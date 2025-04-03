

using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.AprobacionModels;

namespace Negocio.Profiles
{


    public class AprobacionProfile : Profile
    {
        public AprobacionProfile()
        {
            CreateMap<AprobacionModel, Aprobacion>();
            CreateMap<Aprobacion, AprobacionModel>();


        }
    }


}
