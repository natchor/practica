using Entidad.Interfaz.Models.RoleModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IRoleService : IService<RoleModel, int>
    {

        RoleModel FindById(int Id);
        int Guardar(RoleModel rol);

        Task<List<SelectRoleModel>> GetForSelect();
        Task<List<RoleModel>> GetAll();

    }
}