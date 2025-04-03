using Dato.Respositories;
using antecedentes_salud_backend.Models;
using antecedentes_salud_backend.Interfaces.Repositories;



namespace antecedentes_salud_backend.Repositories
{
    public class QrRepository : BaseRepository<QR, int>, IQrRepository
    
    {
        public QrRepository(AppDbContext context) : base(context)
        {
        }
    }
}
