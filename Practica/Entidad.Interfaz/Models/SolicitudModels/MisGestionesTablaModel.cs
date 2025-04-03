using System;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class MisGestionesTablaModel
    {
        public int Id { get; set; }
        public string NroSolicitud { get; set; }
        public string FechaCreacion { get; set; }
        public string NombreCompra { get; set; }
        public string AprobadorActualStr { get; set; }
        public DateTime? FechaDerivacionAnalista { get; set; }
        public string OrdenCompra { get; set; }
        public string ContraparteTecnica { get; set; }
        public string CodEstadoLicitacion { get; set; }
        public int EstadoId { get; set; }
        public int TipoCompraId { get; set; }
        public decimal MontoAprox { get; set; }
        public string TipoCompra { get; set; }
        public string TipoMoneda { get; set; }
        public bool TieneContrato { get; set; }

    }
}
