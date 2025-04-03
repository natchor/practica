
using Dato.Entities;

namespace Dato.Respositories
{
    public class ConvenioRepository : BaseRepository<Convenio, int>, IConvenioRepository
    {
        public ConvenioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}