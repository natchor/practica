namespace Dato.Entities
{
    /// <summary>
    /// Programa presupuestario
    /// </summary>
    public class SectProgPre
    {
        public int SectorId { get; set; }
        public int ProgramaPresupuestarioId { get; set; }
        public int UserEncargadoId { get; set; }

        public Sector Sector { get; set; }
        public ProgramaPresupuestario ProgramaPresupuestario { get; set; }

    }
}
