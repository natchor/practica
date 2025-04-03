using System;
using System.Collections.Generic;
using System.Text;

namespace Dato.Entities
{
    public class EstadoCompra
    {
        public int Id { get; set; }
        public int EstadoId { get; set; }
        public int TipoCompraId { get; set; }

        public TipoCompra TipoCompra { get; set; }
        public Estado Estado { get; set; }
    }
}
