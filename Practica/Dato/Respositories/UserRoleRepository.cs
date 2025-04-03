using Dato.Entities;
using Dato.Interfaces.Repositories;

namespace Dato.Respositories
{
    public class UserRoleRepository : BaseRepository<UserRole, int>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
