using System;


namespace Entidad.Interfaz.Models.ConceptoPresupuestarioModels
{
    public class ConceptoPresupuestarioModel
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public Boolean Estado { get; set; }
        public int? SectorPertinenciaId { get; set; }

    }
}

