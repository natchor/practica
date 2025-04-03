
using Dato.Entities;
using Dato.Repositories;

namespace Dato.Respositories
{
    public class FeriadoChileRepository : BaseRepository<FeriadoChile, int>, IFeriadoChileRepository
    {
        public FeriadoChileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
