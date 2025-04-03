using Entidad.Interfaz.Models.TipoCompraModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface ITipoCompraService : IService<TipoCompraModel, int>
    {

        TipoCompraModel FindById(int Id);
        List<TipoMonedaSelectModel> GetSelect();
        Task<List<TipoCompraModel>> GetAllCompra();
        int Guardar(TipoCompraModel model);
    }
}
