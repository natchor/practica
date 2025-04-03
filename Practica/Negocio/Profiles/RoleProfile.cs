using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.RoleModels;

namespace Negocio.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleModel, Role>();
            CreateMap<Role, RoleModel>();
            CreateMap<Role, SelectRoleModel>();

        }
    }
}