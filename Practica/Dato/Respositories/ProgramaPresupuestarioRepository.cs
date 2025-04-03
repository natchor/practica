using Dato.Entities;

namespace Dato.Respositories
{
    public class ProgramaPresupuestarioRepository : BaseRepository<ProgramaPresupuestario, int>, IProgramaPresupuestarioRepository
    {
        public ProgramaPresupuestarioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}