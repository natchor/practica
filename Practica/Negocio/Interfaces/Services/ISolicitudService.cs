using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.AprobacionModels;
using Entidad.Interfaz.Models.ArchivoModels;
using Entidad.Interfaz.Models.SolicitudDetalleModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.UserRoleModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Negocio.Interfaces.Services
{
    public interface ISolicitudService : IService<SolicitudModel, int>
    {

        SolicitudModel FindById(int Id);
        Task<SolicitudModel> FindByIdAsync(int Id);
        Task<int> GuardarAsync(SolicitudModel solicitud);
        ArchivoModel GuardarPDF(int Id, byte[] pdfData, string nombre);
        //byte[][] ObtenerPDFs(string folderPath, int solicitudId);
        Task<SolicitudModel> AsingarSolicitudAsync(SolicitudAnalistaCompraModel asignacion);
        Task<List<MisSolicitudTablaModel>> GetBySolicitanteId(int id);
        Task<List<MisAprobacionesTablaModel>> GetMisAprobaciones(int userId);
        int Aprobar(AprobacionModel solicitud);
        Task<List<GestionSolicitudesTablaModel>> GetTblGestionSolicitudes(GSFiltros estadoId);

        Task<List<AprobacionModel>> GetAprobacionesBySolicitudId(int solId);
        Task<List<SolicitudModel>> FindByUserId(int? id);
        int GuardarAprobacionEnMatriz(AprobacionModel aprob);
        AprobacionModel FindAprobacion(int id, int configId);
        Task<int> RechazarSolicitud(AprobacionModel aprobacionEntity);
        int GenerarOC(OCSolicitudModel oCSolicitud);
        Task<int> AprobarCDP(SolicitudModel solicitud);
        AprobacionModel EliminarAprobacion(int id);
        Task<List<SelectAprobacionConfigModel>> GetConfigForSelect();
        Task<List<SolicitudDetalleModel>> FindDetalleBySolicitudId(int solicitudId);
        Task<List<SolicitudPorFinalizarModel>> GetPorFinalizar(UserRoleModel userId);
        Task<SolicitudModel> FindByNumSolicitud(string id);
        string GetNroSolicitud(int ret);
        Task<int> GuardarFoliosAsync(SolicitudPresupuestoModel sol);
        bool TieneAnalistaDeCompra(int solicitudId);
        Task<List<MisGestionesTablaModel>> GetTblMisGestiones(int userId);
        Task<SolicitudModel> SetEstadoSolicitud(string estado, int solicitudid);
        Task<List<SolicitudPorFinalizarModel>> GetPorAutorizar();
        SolicitudModel FindTieneOCById(int solId);
        SolicitudDetalleModel FindDetalleAnioActualById(int solicitudId);
        Task<int> SetAprobadorActual(int sigAprobador, int solicitudId);
        SolicitudModel FindSolicitudConCDP(int solId);
    }
}
