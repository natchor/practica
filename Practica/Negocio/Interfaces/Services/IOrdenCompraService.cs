
using Entidad.Interfaz.Models.OrdenCompraModels;
using System.Collections.Generic;

namespace Negocio.Interfaces.Services
{
    public interface IOrdenCompraService : IService<OrdenCompraModel, int>
    {

        OrdenCompraModel FindById(int Id);
        OrdenCompraModel FindByOC(string oc);
        int Guardar(OrdenCompraModel OrdenCompra);
        //List<ModalidadCompraSelectModel> GetSelect();
    }
}

