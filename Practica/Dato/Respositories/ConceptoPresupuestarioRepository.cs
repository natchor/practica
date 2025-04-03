using Dato.Entities;

namespace Dato.Respositories
{
    public class ConceptoPresupuestarioRepository : BaseRepository<ConceptoPresupuestario, int>, IConceptoPresupuestarioRepository
    {
        public ConceptoPresupuestarioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}