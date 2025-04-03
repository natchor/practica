using System.Collections.Generic;

namespace Dato.Entities
{
    public class TipoCompra
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // compra agil, compra conjunta, convenio marco
        public bool Contrato { get; set; } //indica si el tipo de compra tiene o no contrato
        public bool Estado { get; set; }

        public ICollection<Solicitud> Solicitudes { get; set; }
    }
}
