using Biblioteca.Librerias;
using Microsoft.Extensions.Configuration;

namespace DemoIntro.Models
{
    public class SiteKeys
    {
        private static IConfigurationSection _configuration;
        public static void Configure(IConfigurationSection configuration)
        {
            _configuration = configuration;
        }

        public static string WebSiteDomain => _configuration["WebSiteDomain"];
        public static int TiempoSesionMin => _configuration["TiempoSesionMin"]._toInt();

        public static string Token => _configuration["Secret"];

        public static string CMFKey => _configuration["CMFKey"];

        public static string MPKey => _configuration["MPKey"];

        public static string FilesPath => _configuration["FilesPath"];
        public static string ReportPath => _configuration["ReportPath"];

        //public static string LDAPUsuario => _configuration["LDAPUsuario"];
        //public static string LDAPClave => _configuration["LDAPClave"];
        //public static string LDAPDominio => _configuration["LDAPDominio"];



    }
}