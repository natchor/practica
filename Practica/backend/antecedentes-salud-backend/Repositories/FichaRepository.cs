
using Dato.Respositories;
using antecedentes_salud_backend.Models;

using antecedentes_salud_backend.Interfaces.Repositories;

namespace antecedentes_salud_backend.Repositories
{
    public class FichaRepository : BaseRepository<Antecedentes, int>, IFichaRepository
    {
        public FichaRepository(AppDbContext context) : base(context)
        {
        }
    }
}
