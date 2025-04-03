using Entidad.Interfaz.Models.EstadoCompraModels;
using Entidad.Interfaz.Models.EstadoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IEstadoService : IService<EstadoModel, int>
    {

        EstadoModel FindById(int Id);
        Task<List<EstadoModel>> GetAllEstado();
        int Guardar(EstadoModel model);
        Task<List<EstadoSelectModel>> GetForSelect();
        Task<EstadoModel> FindByCodStr(string estado);
  
    }
}
