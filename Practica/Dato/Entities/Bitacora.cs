using System;

namespace Dato.Entities
{
    public class Bitacora
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Observacion { get; set; }
        public int UserId { get; set; }
        public int SolicitudId { get; set; }
        public int? TipoBitacora { get; set; }
        public Solicitud Solicitud { get; set; }
        public User User { get; set; }
    }
}
