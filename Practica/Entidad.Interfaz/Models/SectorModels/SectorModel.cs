using System;


namespace Entidad.Interfaz.Models.SectorModels
{
    public class SectorModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public Boolean Estado { get; set; }
        public int? SectorPadreId { get; set; }
        public bool TienePresupuesto { get; set; }

    }
}

