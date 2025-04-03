
using antecedentes_salud_backend.Models;
using antecedentes_salud_backend.Interfaces.Repositories;

using Dato.Respositories;


namespace antecedentes_salud_backend.Repositories;

    public class UserRepository : BaseRepository<User, int>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}



