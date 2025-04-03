using System;
using System.Collections.Generic;

namespace Dato.Entities
{
    public class Sector
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public Boolean Estado { get; set; }
        public int? SectorPadre { get; set; }
        public bool TienePresupuesto { get; set; }

        public ICollection<SectProgPre> SectProgPres { get; set; }
        public ICollection<User> Users { get; set; }

        public ICollection<Solicitud> SolicitudesUnidadesDemandantes { get; set; }

    }
}