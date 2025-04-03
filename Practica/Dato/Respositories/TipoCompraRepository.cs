using Dato.Entities;

namespace Dato.Respositories
{
    public class TipoCompraRepository : BaseRepository<TipoCompra, int>, ITipoCompraRepository
    {
        public TipoCompraRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}