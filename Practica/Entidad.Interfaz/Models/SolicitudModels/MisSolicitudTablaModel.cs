using System;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class MisSolicitudTablaModel
    {
        public int Id { get; set; }
        public string NroSolicitud { get; set; }
        public string FechaCreacion { get; set; }
        public string NombreCompra { get; set; }
        public string AprobadorActualStr { get; set; }
        public DateTime? FechaDerivacionAnalista { get; set; }
        public string OrdenCompra { get; set; }
        public string ContraparteTecnica { get; set; }
        public string Estado { get; set; }
        public int EstadoId { get; set; }
    }
}
