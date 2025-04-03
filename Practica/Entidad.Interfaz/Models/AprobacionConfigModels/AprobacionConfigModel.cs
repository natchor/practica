namespace Entidad.Interfaz.Models.AprobacionConfigModels
{
    public class AprobacionConfigModel
    {

        public int Id { get; set; }
        public int ConceptoPresupuestarioId { get; set; }
        public string Nombre { get; set; }
        public string Quien { get; set; }
        public long? MontoUTMDesde { get; set; }
        public long? MontoUTMHasta { get; set; }
        public int Orden { get; set; }
        public bool EstaActivo { get; set; }

        public bool EsParaTodoConceptoPre { get; set; }
        public int? AConfigRequeridaId { get; set; }
        public bool RequiereAsignacion { get; set; }
    }
}
