using System.Collections.Generic;

namespace Dato.Entities
{
    public class ModalidadCompra
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // anual, o multianual


        public ICollection<Solicitud> Solicitudes { get; set; }
    }
}
