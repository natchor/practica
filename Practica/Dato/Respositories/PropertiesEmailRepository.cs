using Dato.Entities;

namespace Dato.Respositories
{
    public class PropertiesEmailRepository : BaseRepository<PropertiesEmail, int>, IPropertiesEmailRepository
    {
        public PropertiesEmailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}