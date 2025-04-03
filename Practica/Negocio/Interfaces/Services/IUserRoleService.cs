using Entidad.Interfaz.Models.UserRoleModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IUserRoleService : IService<UserRoleModel, int>
    {

        UserRoleModel FindById(int Id);

        UserRoleModel FindByUserId(int IdUser);
        int Guardar(UserRoleModel userRole);
        Task<List<UserRoleModel>> GetAll();

    }
}