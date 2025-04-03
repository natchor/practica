using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.UserModels;

namespace Negocio.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, User>(); 
            CreateMap<User, UserModel>();

            CreateMap<User, UserTablaModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(x => $"{x.Nombre} {x.Apellido}"))
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(x => x.Cargo.Nombre))
                .ForMember(dest => dest.Sector, opt => opt.MapFrom(x => x.Sector.Nombre))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(x => x.Estado));


            CreateMap<User, SelectUserModel>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(x => $"{x.Nombre} {x.Apellido}"));

        }
    }
}
