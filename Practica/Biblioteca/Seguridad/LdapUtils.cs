using Biblioteca.Librerias;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices;

namespace Biblioteca.Seguridad
{

    /// <summary>
    /// En configureServices del archivo "startup" ingesar -> "LdapConfig.Configure(Configuration.GetSection("Ldap"))" con el objetivo de parametrizar por appsettings 
    /// la configuracion
    /// </summary>
    public class LdapUtils
    {

        /// <summary>
        /// La funcion de este metodo es confirmar que el usuario y clave son correctos
        /// en caso de obtener atributos de LDAP es necesario otro metodo en esta misma libreria
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="clave"></param>
        /// <returns></returns>
        public static bool ValidarLogin(string usuario, string clave)
        {

            //string userParam = "sAMAccountName";

            //string hostPath = "LDAP://10.0.0.81";

            bool ret = false;

            using (DirectoryEntry objDE = new DirectoryEntry(LdapConfig.HostPath, usuario, clave))
            {
                using (var directorySearcher = new DirectorySearcher(objDE))
                {
                    string userLdap = usuario.ToUpper().Replace("@MINENERGIA.CL", "");

                    directorySearcher.Filter = $"(&(objectCategory=person)({LdapConfig.UserParam}={userLdap}))";
                    directorySearcher.SearchScope = SearchScope.Subtree;

                    SearchResult searchResult = directorySearcher.FindOne();

                    ret = searchResult != null;

                    //if (searchResult != null)
                    //{

                    //}
                };

            };


            return ret;
        }
    }

    public class LdapConfig
    {
        private static IConfigurationSection _configuration;
        public static void Configure(IConfigurationSection configuration)
        {
            _configuration = configuration;
        }


        public static bool UseLoginLdap => _configuration["UseLoginLdap"]._toBool();
        public static string HostPath => _configuration["HostPath"];
        public static string UserParam => _configuration["UserParam"];
    }
}
