using System;
using System.Collections.Generic;
using System.Text;

namespace Entidad.Interfaz.Models.ConvenioModels
{
    public class ConvenioModel
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
       
        public int? AutorizadorFinId { get; set; }
        public int? AutorizadorPresId { get; set; }

        public string Antecedente { get; set; }
        public string CuentaCorriente { get; set; }
        public string SaldoCuenta { get; set; }
        public string Banco { get; set; }
        public string CertificadoSaldo { get; set; }
        public DateTime? FechaAutorizacionFin { get; set; }
        public DateTime? FechaAutorizacionPres { get; set; }


      
    }
}
