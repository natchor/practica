using System;
using System.Collections.Generic;
using System.Text;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class SolicitudPorFinalizarModel
    {
     
        public int Id { get; set; }
        public string NroSolicitud { get; set; }
        public string FechaCreacion { get; set; }
        public string NombreCompra { get; set; }
        public string AprobadorActualStr { get; set; }
        public string EstadoStr { get; set; }
        public string FaseCDP { get; set; }
        public string NombreSolicitante { get; set; }
        public string AprobadorCDPStr { get; set; }


    }
}
