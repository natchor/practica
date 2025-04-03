using System;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class MisAprobacionesTablaModel
    {
        public int Semaforo;
        public DateTime FechaUltimaAproba;

        public int Id { get; set; }
        public string NroSolicitud { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NombreCompra { get; set; }
        public string OrdenCompra { get; set; }
        public string NombreContraparteTecnica { get; set; }
        public string NombreSolicitante { get; set; }

        public bool EstaAprobado { get; set; }

        public bool TieneQueAprobar { get; set; }

        public string AprobacionPendiente { get; set; }
        public string UnidadDemandanteStr { get; set; }

    }
}
