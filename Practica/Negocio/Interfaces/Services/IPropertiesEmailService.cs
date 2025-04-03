using Entidad.Interfaz.Models.PropertiesEmailModels;

namespace Negocio.Interfaces.Services
{
    public interface IPropertiesEmailService : IService<PropertiesEmailModel, int>
    {

        PropertiesEmailModel FindById(int Id);
        PropertiesEmailModel FindByCodigo(string codigo);
        int Guardar(PropertiesEmailModel email);

    }
}
