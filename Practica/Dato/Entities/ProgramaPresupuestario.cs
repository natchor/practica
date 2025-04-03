using System.Collections.Generic;

namespace Dato.Entities
{
    public class ProgramaPresupuestario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public bool SinCDP { get; set; }
        public bool ConCS { get; set; }
        public ICollection<SectProgPre> SectProgPres { get; set; }
    }
}
