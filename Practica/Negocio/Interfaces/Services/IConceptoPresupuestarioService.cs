using Entidad.Interfaz.Models.ConceptoPresupuestarioModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IConceptoPresupuestarioService : IService<ConceptoPresupuestarioModel, int>
    {

        ConceptoPresupuestarioModel FindById(int Id);

        Task<List<SelectConceptoPresupuestarioModel>> GetForSelect();
        Task<List<ConceptoPresupuestarioModel>> GetAllConcepto();
        int Guardar(ConceptoPresupuestarioModel model);
    }
}
