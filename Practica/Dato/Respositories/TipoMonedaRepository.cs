using Dato.Entities;

namespace Dato.Respositories
{
    public class TipoMonedaRepository : BaseRepository<TipoMoneda, int>, ITipoMonedaRepository
    {
        public TipoMonedaRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}