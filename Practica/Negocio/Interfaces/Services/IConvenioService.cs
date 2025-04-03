

using Entidad.Interfaz.Models.ConvenioModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IConvenioService : IService<ConvenioModel, int>
    {

        ConvenioModel FindBySolicitudId(int Id);

        Task<int> AprobarCS(ConvenioModel conv);

        Task<List<ConvenioModel>> GetAllConvenios();
        int Guardar(ConvenioModel model);
    }
}
