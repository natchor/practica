

using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.UserModels;
using System;


namespace Entidad.Interfaz.Models.BitacoraModels
{
    public class BitacoraModel
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public int UserId { get; set; }

        public int SolicitudId { get; set; }
        public int? TipoBitacora { get; set; }
        public SolicitudModel Solicitud { get; set; }
        public UserModel User { get; set; }

    }
}
