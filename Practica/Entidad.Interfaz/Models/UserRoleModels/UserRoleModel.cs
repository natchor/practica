using Entidad.Interfaz.Models.RoleModels;
using Entidad.Interfaz.Models.UserModels;

namespace Entidad.Interfaz.Models.UserRoleModels
{
    public class UserRoleModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public UserModel User { get; set; }
        public RoleModel Role { get; set; }
    } 
}
