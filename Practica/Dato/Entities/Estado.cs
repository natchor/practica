using System.Collections.Generic;

namespace Dato.Entities
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool PermiteGenerarOC { get; set; }
        public string CodigoStr { get; set; }

        public ICollection<Solicitud> Solicitudes { get; set; }
    }
}
