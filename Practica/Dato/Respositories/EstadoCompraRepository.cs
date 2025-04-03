
using Dato.Entities;

namespace Dato.Respositories
{
    public class EstadoCompraRepository : BaseRepository<EstadoCompra, int>, IEstadoCompraRepository
    {
        public EstadoCompraRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}