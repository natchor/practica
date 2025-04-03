namespace antecedentes_salud_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string rut { get; set; }
        public string userName { get; set; }
        public string nombre { get; set; }
        public string Correo { get; set; }
        public int estado { get; set; }
        public string rol { get; set; }
        public string NombreFuncionario { get; set; }
        public string TelefonoFuncionario { get; set; }
        public string DireccionMinisterio { get; set; }
    }
}