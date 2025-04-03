using Entidad.Interfaz.Models.UserModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IUserService : IService<UserModel, int>
    {

        UserModel Login(string userName, string password);
        Task<List<SelectUserModel>> GetForSelect();
        Task<List<UserTablaModel>> GetAllUser();
        UserModel GetByUserId(int id);
        int Guardar(UserModel userm);
        string ValidarUsuario(string mail);
        UserModel ExistUser(string userName);
        Task<List<UserModel>> GetByUserCompra();
        Task<List<UserModel>> GetPresupuestoUserIds();
        Task<bool> EsPresupuesto(int userId);
        int? BuscaJefe(int? sectorId);
        //Task<UserModel> GetByUserId(int id);
    }
}
