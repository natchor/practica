using Entidad.Interfaz.Models.BitacoraModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IBitacoraService : IService<BitacoraModel, int>
    {

        List<BitacoraModel> FindById(int Id);
        void Guardar(BitacoraModel bitacora);
        Task<List<BitacoraModel>> FindBySolicitudId(int id);
        Task<List<BitacoraModel>> FindBitaEstadosBySolicitudId(int id);
        
    }
}
