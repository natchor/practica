using System;


namespace Entidad.Interfaz.Models.TipoMonedaModels
{
    public class TipoMonedaModel
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool Estado { get; set; }
        public decimal Valor { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaReferencia { get; set; }
        public string UrlCMF { get; set; }

    }
}

