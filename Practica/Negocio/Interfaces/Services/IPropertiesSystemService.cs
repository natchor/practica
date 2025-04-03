using System.Collections.Generic;
using System.Threading.Tasks;
using Entidad.Interfaz.Models.PropertiesSystemModels;

namespace Negocio.Interfaces.Services
{
    public interface IPropertiesSystemService : IService<PropertiesSystemModel, int>
    {

        PropertiesSystemModel FindById(int Id);

        Task<List<PropertiesSystemModel>> GetAllProperties();
        PropertiesSystemModel FindByCodigo(string codigo);
        int Guardar(PropertiesSystemModel email);

    }
}
