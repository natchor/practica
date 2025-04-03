using Entidad.Interfaz.Models.ArchivoModels;
using System;
using System.Collections.Generic;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class OCSolicitudModel
    {
        public int SolicitudId { get; set; }
        public string NumOrdenCompra { get; set; }
        public string RutProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public DateTime? FechaOrdenCompra { get; set; }
        public string EstadoStr { get; set; }

        public decimal MontoDivisa { get; set; }
        public decimal? MontoOC { get; set; }
        //public decimal? TipoCambioDivisa { get; set; }
        public DateTime? FechaInicioContrato { get; set; }
        public DateTime? FechaFinContrato { get; set; }

        public ICollection<ArchivoModel> Archivos { get; set; }
    }
}
