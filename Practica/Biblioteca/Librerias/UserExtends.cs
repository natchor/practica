using System;
using System.Security.Claims;

namespace Biblioteca.Librerias
{
    public static class ClaimsPrincipalExtends
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? claim.Value : null;
        }

        //public static string GetUserName(this ClaimsPrincipal principal)
        //{
        //    var fullName = principal.Claims.FirstOrDefault(c => c.Type == "userName");
        //    return fullName?.Value;
        //}

        //public static string GetRoleName(this ClaimsPrincipal principal)
        //{
        //    var roleName = principal.Claims.FirstOrDefault(c => c.Type == "roleName");
        //    return roleName?.Value;
        //}
    }


    public struct CustomClaims
    {
        public const string FullName = "FullName";
        public const string UserId = "UserId";
        public const string RoleStr = "RoleStr";
        public const string Cargo = "Cargo";
        public const string Sector = "Sector";
        public const string CargoId = "CargoId";
        public const string RoleId = "RoleId";
        public const string SectorId = "SectorId";
        public const string SectorDemandante = "SectorDemandante";
        public const string SectorPadreId = "SectorPadreId";

    }

    public static class PermissionsClaims
    {
        public const string IngresaSolicitud = "IngresaSolicitud";
        public const string ApruebaCDP = "ApruebaCDP";
        public const string PuedeAsignar = "PuedeAsignar";
        public const string AdmMantenedores = "AdmMantenedores";
        public const string VeGestionSolicitudes = "VeGestionSolicitudes";
        public const string VePorFinalizar = "VePorFinalizar";
        public const string IngresaOC = "IngresaOC";
        public const string FinalizaSolicitud = "FinalizaSolicitud";
        public const string ModificaMatrizAprobacion = "ModificaMatrizAprobacion";
        public const string AjustarCDP = "AjustarCDP";
        public const string Reportes = "Reportes";
        public const string VeMisGestiones = "VeMisGestiones";
    }

}
