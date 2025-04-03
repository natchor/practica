using Entidad.Interfaz.Models.FeriadoChileModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IFeriadoChileService : IService<FeriadoChileModel, int>
    {

        FeriadoChileModel FindById(int Id);
        List<FeriadoChileModel> GetAllFeriados();
        int Guardar(FeriadoChileModel model);

    }
}

