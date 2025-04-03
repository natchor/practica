using System.Collections.Generic;

namespace Dato.Entities
{
    public class AprobacionConfig
    {
        public int Id { get; set; }
        //public int ConceptoPresupuestarioId { get; set; }
        public string Nombre { get; set; }
        public string Quien { get; set; }
        public long? MontoUTMDesde { get; set; }
        public long? MontoUTMHasta { get; set; }
        public int Orden { get; set; }
        public bool EstaActivo { get; set; }

        public bool EsParaTodoConceptoPre { get; set; }
        public int? AConfigRequeridaId { get; set; }

        public bool RequiereAsignacion { get; set; }

        // ver si es necesario cant de aprobaciones minimas para pasar a la siguiente etapa

        //public ConceptoPresupuestario ConceptoPresupuestario { get; set; }

        public ICollection<Solicitud> Solicitudes { get; set; }
        public ICollection<Aprobacion> Aprobaciones { get; set; }

        public AprobacionConfig AConfigRequerida { get; set; }
    }
}
