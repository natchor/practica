using Dato.Entities;
using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.UserModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface IAprobacionConfigService : IService<AprobacionConfig, int>
    {
        List<AprobacionConfigModel> GetActivos();
        int? GetPrimerId();
        int? GetAsignacionConfigId();

        Task GetUsersAprobadoresAsync(Solicitud solic);
        AprobacionConfigModel FindById(int aprobacionConfigId);
        List<UserModel> GetSiguienteAprobador(int solicitudId, int ConfigId);
    }
}
