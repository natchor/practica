using Dato.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dato.Interfaces.Repositories
{
    public interface IAprobacionRepository : IRepository<Aprobacion, int>
    {
        Task<List<int>> GetAprobadoresIds(AprobacionConfig ac, int userId);
        Task<int> GetObtenerNumCDP();
        Task<int> GetObtenerNumCS();
        void UpdateAprobadoresIds(int orden, int solicitudId);
        void UpdateAprobadoresActualiza(int orden, int solicitudId);
        void UpdateMatrizAprobacion(int aprobConfigId, int solicitudId, int userId);
        void InsertMatrizAprobacion(int solicitudId);
        
    }
}
