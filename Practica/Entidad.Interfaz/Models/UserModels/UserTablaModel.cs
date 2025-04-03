using System;

namespace Entidad.Interfaz.Models.UserModels
{
    public class UserTablaModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Cargo { get; set; }
        public string Sector { get; set; }
        public bool Estado { get; set; }
    }
}
