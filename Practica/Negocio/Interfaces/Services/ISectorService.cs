using Entidad.Interfaz.Models.SectorModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface ISectorService : IService<SectorModel, int>
    {
        SectorModel FindById(int Id);

        Task<List<SelectSectorModel>> GetForSelect();
        Task<List<SelectSectorModel>> GetForSelectConPresupuesto();

        Task<List<SectorModel>> GetAllSector();
        int Guardar(SectorModel model);
    }
}
