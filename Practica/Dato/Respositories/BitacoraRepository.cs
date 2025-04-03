using Dato.Entities;

namespace Dato.Respositories
{
    public class BitacoraRepository : BaseRepository<Bitacora, int>, IBitacoraRepository
    {
        public BitacoraRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}