using Dato.Entities;

namespace Dato.Respositories
{
    public class ModalidadCompraRepository : BaseRepository<ModalidadCompra, int>, IModalidadCompraRepository
    {
        public ModalidadCompraRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}