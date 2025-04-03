using System;
using System.Collections.Generic;

namespace Dato.Entities
{
    public class ConceptoPresupuestario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public Boolean Estado { get; set; }
        public int? SectorPertinenciaId { get; set; }


        public Sector SectorPertinencia { get; set; }
        public ICollection<Solicitud> Solicitudes { get; set; }
    }
}
