using Entidad.Interfaz.Models.SolicitudModels;
using System;

namespace Entidad.Interfaz.Models.ArchivoModels
{
    public class ArchivoModel
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public string Nombre { get; set; }
        public string FullPath { get; set; }
        public string Ext { get; set; }
        public long Size { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }

        public SolicitudModel Solicitud { get; set; }

    }
}
