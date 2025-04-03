using System;
using System.Collections.Generic;

namespace Dato.Entities
{
    public class FeriadoChile
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public int Region { get; set; }
        public int Estado { get; set; }

    }
}

