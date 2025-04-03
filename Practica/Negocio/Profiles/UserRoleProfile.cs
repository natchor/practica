using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.UserRoleModels;

namespace Negocio.Profiles
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRoleModel, UserRole>();
            CreateMap<UserRole, UserRoleModel>();


        }
    }
}