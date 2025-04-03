using Entidad.Interfaz.Models.TipoMonedaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface ITipoMonedaService : IService<TipoMonedaModel, int>
    {

        TipoMonedaModel FindById(int Id);
        int Guardar(TipoMonedaModel solicitud);
        //List<TipoMonedaModel> GetAll();
        Task<List<SelectTipoMonedaModel>> GetForSelect();
        Task<List<TipoMonedaModel>> GetAll();

        TipoMonedaModel FindByCodigo(string codigo);
        object GetReporte();
    }
}
