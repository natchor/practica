using System.Collections.Generic;

namespace Dato.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public bool IngresaSolicitud { get; set; }
        public bool ApruebaCDP { get; set; }
        public bool PuedeAsignar { get; set; }
        public bool AdmMantenedores { get; set; }
        public bool VeGestionSolicitudes { get; set; }
        public bool VerPorFinalizar { get; set; }
        public bool IngresaOC { get; set; }
        public bool FinalizaSolicitud { get; set; }
        public bool ModificaMatrizAprobacion { get; set; }
        public bool AjustarCDP { get; set; }
        public bool Reportes { get; set; }
        public bool VerMisGestiones { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
