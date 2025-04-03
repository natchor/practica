using Entidad.Interfaz.Models.ProgramaPresupuestarioModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IProgramaPresupuestarioService : IService<ProgramaPresupuestarioModel, int>
    {
        ProgramaPresupuestarioModel FindById(int Id);
        Task<ProgramaPresupuestarioModel> FindByIdAsync(int Id);
        List<ProgPresSelectModel> GetForSelect();
        Task<List<ProgPresSelectModel>> GetForSelectAsync();
        Task<List<ProgPresSelectModel>> GetForSelectBySectorAsync(int sectId);
        Task<List<ProgramaPresupuestarioModel>> GetAllPrograma();
        int Guardar(ProgramaPresupuestarioModel model);
        List<ProgPresSelectModel> GetBySectorIdForSelect(int sectorId);
    }
}
