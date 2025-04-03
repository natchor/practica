using Dato.Entities;

namespace Dato.Respositories
{
    public class SectorRepository : BaseRepository<Sector, int>, ISectorRepository
    {
        public SectorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
