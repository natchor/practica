using Microsoft.Extensions.Configuration;
using System;
using System.DirectoryServices;

namespace Biblioteca.Seguridad
{
    public class LdapUtils
    {
        public static bool ValidarLogin(string correo, string clave)
        {
            bool ret = false;

            try
            {
                string userLdap = correo.Split('@')[0]; // Extraer el nombre de usuario del correo electrónico

                using (DirectoryEntry objDE = new DirectoryEntry(LdapConfig.HostPath, userLdap, clave))
                {
                    objDE.AuthenticationType = AuthenticationTypes.Secure; // Usar autenticación segura

                    using (var directorySearcher = new DirectorySearcher(objDE))
                    {
                        directorySearcher.Filter = $"(&(objectCategory=person)({LdapConfig.UserParam}={userLdap}))";
                        directorySearcher.SearchScope = SearchScope.Subtree;
                        directorySearcher.ClientTimeout = TimeSpan.FromSeconds(LdapConfig.ConnectionTimeout); // Configurar timeout

                        SearchResult searchResult = directorySearcher.FindOne();

                        ret = searchResult != null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: puedes registrar el error o manejarlo según sea necesario
                Console.WriteLine($"Error al validar el login LDAP: {ex.Message}");
                ret = false;
            }

            return ret;
        }
    }

    public class LdapConfig
    {
        private static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static bool UseLoginLdap => bool.Parse(_configuration["Ldap:UseLoginLdap"]);
        public static string HostPath => _configuration["Ldap:HostPath"];
        public static string UserParam => _configuration["Ldap:UserParam"];
        public static int ConnectionTimeout => int.Parse(_configuration["Ldap:ConnectionTimeout"]);
    }
}