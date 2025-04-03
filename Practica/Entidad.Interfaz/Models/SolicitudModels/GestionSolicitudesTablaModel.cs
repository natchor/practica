using Entidad.Interfaz.Models.UserModels;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class GestionSolicitudesTablaModel
    {
        public int Id { get; set; }
        public string NroSolicitud { get; set; }
        public string FechaCreacion { get; set; }
        public string NombreCompra { get; set; }
        public string AprobadorActualStr { get; set; }
        public string EstadoStr { get; set; }
        public string FaseCDP { get; set; }
        public string NombreSolicitante { get; set; }
        public string FolioRequerimientoSIGFE { get; set; }

        public int? AnalistaProcesoId { get; set; }
        public int? FuncionarioValidacionCDPId { get; set; }

        public string AnalistaProcesoStr { get; set; }
        public string FuncionarioValidacionCDPStr { get; set; }
        public string FuncionarioPresupuestoStr { get; set; }
        public UserModel AnalistaProceso { get; set; }
        public UserModel FuncionarioValidacionCDP { get; set; }

    }
}
