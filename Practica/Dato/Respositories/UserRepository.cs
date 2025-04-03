using Dato.Entities;
using Dato.Interfaces.Repositories;

namespace Dato.Respositories
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
