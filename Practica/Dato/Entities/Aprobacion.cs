using System;

namespace Dato.Entities
{
    public class Aprobacion
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public int AprobacionConfigId { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public int UserAprobadorId { get; set; }
        public bool EstaAprobado { get; set; }
        public string Observacion { get; set; }
        public int Orden { get; set; }




        public AprobacionConfig AprobacionConfig { get; set; }
        public User UserAprobador { get; set; }
        public Solicitud Solicitud { get; set; }
    }
}
