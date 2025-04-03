using System;
using System.Collections.Generic;
using System.Text;

namespace Dato.Entities
{
    public class StoredProcedure
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public int Parametros { get; set; }

    }
}
