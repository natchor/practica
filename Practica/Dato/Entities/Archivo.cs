using System;

namespace Dato.Entities
{
    public class Archivo
    {
        public int Id { get; set; }

        public int SolicitudId { get; set; }

        public string Nombre { get; set; }
        public string FullPath { get; set; }
        public string Ext { get; set; }
        public long Size { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? UsuarioId { get; set; }

        public Solicitud Solicitud { get; set; }
        public User Usuario { get; set; }

    }
}
