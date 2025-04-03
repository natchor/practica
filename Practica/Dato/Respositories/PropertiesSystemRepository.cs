using Dato.Entities;

namespace Dato.Respositories
{
    public class PropertiesSystemRepository : BaseRepository<PropertiesSystem, int>, IPropertiesSystemRepository
    {
        public PropertiesSystemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}