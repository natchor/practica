using Dato.Entities;
using Dato.Interfaces.Repositories;

namespace Dato.Respositories
{
    public class AprobacionConfigRepository : BaseRepository<AprobacionConfig, int>, IAprobacionConfigRepository
    {
        public AprobacionConfigRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
