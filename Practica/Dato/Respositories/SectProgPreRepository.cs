using Dato.Entities;
using Dato.Interfaces.Repositories;

namespace Dato.Respositories
{
    public class SectProgPreRepository : BaseRepository<SectProgPre, int>, ISectProgPreRepository
    {
        public SectProgPreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
