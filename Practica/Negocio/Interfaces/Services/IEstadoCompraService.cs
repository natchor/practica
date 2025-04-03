
using Entidad.Interfaz.Models.EstadoCompraModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IEstadoCompraService : IService<EstadoCompraModel, int>
    {

        EstadoCompraModel FindById(int Id);
        Task<List<EstadoCompraModel>> GetForSelect();
        Task<List<EstadoCompraModel>> GetAllEstadoCompra();
        int Guardar(EstadoCompraModel model);
        Task<List<EstadoCompraModel>> GetForSelectbyTpComp(int tipoCompra);
    }
}
