using Dato.Entities;

namespace Dato.Respositories
{
    public class ArchivoRepository : BaseRepository<Archivo, int>, IArchivoRepository
    {
        public ArchivoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
