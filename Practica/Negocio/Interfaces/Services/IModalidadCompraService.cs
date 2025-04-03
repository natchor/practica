using Entidad.Interfaz.Models.ModalidadCompraModels;
using System.Collections.Generic;

namespace Negocio.Interfaces.Services
{
    public interface IModalidadCompraService : IService<ModalidadCompraModel, int>
    {

        ModalidadCompraModel FindById(int Id);
        List<ModalidadCompraSelectModel> GetSelect();
    }
}
