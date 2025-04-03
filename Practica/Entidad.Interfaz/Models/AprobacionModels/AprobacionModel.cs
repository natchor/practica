using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.ArchivoModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.UserModels;
using System;
using System.Collections.Generic;

namespace Entidad.Interfaz.Models.AprobacionModels
{
    public class AprobacionModel
    {

        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public int? AnalistaPresupuestoId { get; set; }
        public int AprobacionConfigId { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public int UserAprobadorId { get; set; }
        public bool EstaAprobado { get; set; }
        public string Observacion { get; set; }
        public int Orden { get; set; }
        public string NombreAprobador { get; set; }
        public string NombreAprobacionConfig { get; set; }
        public bool PuedeAprobar { get; set; }

        public bool Actual { get; set; }

        public UserModel UserAprobador { get; set; }
        public SolicitudModel Solicitud { get; set; }
        public AprobacionConfigModel AprobacionConfig { get; set; }

        public ICollection<ArchivoModel> Archivos { get; set; }
    }
}
