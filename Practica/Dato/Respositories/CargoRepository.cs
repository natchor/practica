using Dato.Entities;

namespace Dato.Respositories
{
    public class CargoRepository : BaseRepository<Cargo, int>, ICargoRepository
    {
        public CargoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}