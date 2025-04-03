using Dato.Entities;

namespace Dato.Respositories
{
    public class EstadoRepository : BaseRepository<Estado, int>, IEstadoRepository
    {
        public EstadoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}