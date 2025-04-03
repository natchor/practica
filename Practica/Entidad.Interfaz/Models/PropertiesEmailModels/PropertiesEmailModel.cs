namespace Entidad.Interfaz.Models.PropertiesEmailModels
{
    public class PropertiesEmailModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Asunto { get; set; }
        public string Cc { get; set; }
        public string Cco { get; set; }
        public string From { get; set; }
        public string FromNombre { get; set; }
        public string Mensaje { get; set; }
    }
}
