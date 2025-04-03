using System;
using System.Collections.Generic;
using System.Text;

namespace Dato.Entities
{
    public class Convenio
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        //public int CorrelativoAnual { get; set; }

        public int? AutorizadorFinId { get; set; }
        public int? AutorizadorPresId { get; set; }

        public string Antecedente { get; set; }
        public string CuentaCorriente { get; set; }
        public string SaldoCuenta { get; set; }
        public string Banco { get; set; }
        public string CertificadoSaldo { get; set; }
        public DateTime? FechaAutorizacionFin { get; set; }
        public DateTime? FechaAutorizacionPres { get; set; }

        public Solicitud Solicitud { get; set; }

    }
}
