using System;
using System.Collections.Generic;
using System.Text;

namespace Entidad.Interfaz.Models.FeriadoChileModels
{
    public class FeriadoChileModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public int Region { get; set; }
        public int Estado { get; set; }
    }
}


