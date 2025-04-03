using System;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class SolicitudAnalistaCompraModel
    {
        public int Id { get; set; }
        public int AnalistaProcesoId { get; set; }
        public int EstadoId { get; set; }
        public int? AprobadorActualId { get; set; }
        public DateTime? FechaDerivacion { get; set; }

    }
}