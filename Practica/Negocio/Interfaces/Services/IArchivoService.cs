
using Entidad.Interfaz.Models.ArchivoModels;
using System.Collections.Generic;

namespace Negocio.Interfaces.Services
{
    public interface IArchivoService : IService<ArchivoModel, int>
    {

        ArchivoModel FindById(int Id);
        int Guardar(ArchivoModel archivo);
        List<ArchivoModel> FindBySolicitudId(int solicitudId);
        List<ArchivoModel> FindCDPBySolicitudId(int solicitudId);
        
        void Delete(int archivoId);
        List<ArchivoTablaModel> GetForBitacora(int solicitudId);
        List<ArchivoModel> FindCSBySolicitudId(int id);
    }
}
