using Entidad.Interfaz.Models.CargoModels;
using Entidad.Interfaz.Models.SectorModels;
using Entidad.Interfaz.Models.UserRoleModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidad.Interfaz.Models.UserModels
{
    public class UserModel
    {

        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }


        [Required(ErrorMessage = "Ingrese Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ingrese Clave")]
        public string Password { get; set; }

        public string FullName
        {
            get { return $"{Nombre} {Apellido}"; }
        }
        public string Email { get; set; }
        public bool Estado { get; set; }
        public int? SectorId { get; set; }

        public int? RolId { get; set; }
        public int? CargoId { get; set; }
        public int? JefeDirectoId { get; set; }
        public CargoModel Cargo { get; set; }
        public SectorModel Sector { get; set; }

        public List<UserRoleModel> UserRoles { get; set; }
    }
}
