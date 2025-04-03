using System;
using System.Collections.Generic;

namespace Dato.Entities
{
    public class TipoMoneda
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool Estado { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaReferencia { get; set; }
        public string UrlCMF { get; set; }

        public ICollection<Solicitud> Solicitudes { get; set; }

    }
}
