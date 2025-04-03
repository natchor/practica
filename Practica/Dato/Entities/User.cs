using System.Collections.Generic;

namespace Dato.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? CargoId { get; set; }
        public int SectorId { get; set; }
        public int JefeDirectoId { get; set; }

        public bool Estado { get; set; } //variable nativa

        public ICollection<UserRole> UserRoles { get; set; }
        public Cargo Cargo { get; set; }
        public Sector Sector { get; set; }
        public User JefeDirecto { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
