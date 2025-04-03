using Entidad.Interfaz.Models.CargoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface ICargoService : IService<CargoModel, int>
    {

        CargoModel FindById(int Id);

        Task<List<SelectCargoModel>> GetForSelect();
        Task<List<CargoModel>> GetAllCargo();
        int Guardar(CargoModel model);
    }
}
