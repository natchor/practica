using System.ComponentModel.DataAnnotations;

namespace antecedentes_salud_backend.Models
{
    public class QR
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public string Rut { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string estado { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaEliminacion { get; set; }
    }
}