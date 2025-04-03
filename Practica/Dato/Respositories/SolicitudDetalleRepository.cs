using Dato.Entities;
using Dato.Interfaces.Repositories;

namespace Dato.Respositories
{
    public class SolicitudDetalleRepository : BaseRepository<SolicitudDetalle, int>, ISolicitudDetalleRepository
    {
        public SolicitudDetalleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
