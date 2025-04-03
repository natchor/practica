using Dato.Entities;
using Dato.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Dato.Respositories
{
    public interface ISolicitudRepository : IRepository<Solicitud, int>
    {

        Task<int> GetCorrelativoAnual();

    }
}

