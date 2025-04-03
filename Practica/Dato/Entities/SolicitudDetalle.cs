namespace Dato.Entities
{
    public class SolicitudDetalle
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public decimal MontoPresupuestado { get; set; }
        public int Anio { get; set; }
        public decimal MontoFinal { get; set; }
        public decimal MontoMonedaSel { get; set; }
        public decimal MontoMonedaSelFinal { get; set; }
        public bool EsAjuste{ get; set; }

        public Solicitud Solicitud { get; set; }

    }

}
