using Microsoft.Extensions.Configuration;
using Xunit;

namespace AntecedentesSaludBackend.Tests
{
    public class WindowsLoginTests
    {
        private readonly IConfiguration _configuration;

        public WindowsLoginTests()
        {
            var inMemorySettings = new Dictionary<string, string?> {
                {"Ldap:LDAPUsuario", "tu_usuario"},
                {"Ldap:LDAPDominio", "tu_dominio"},
                {"Ldap:LDAPClave", "tu_clave"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public void Configuration_IsLoaded_Success()
        {
            // Act
            var usuario = _configuration["Ldap:LDAPUsuario"];
            var dominio = _configuration["Ldap:LDAPDominio"];
            var clave = _configuration["Ldap:LDAPClave"];

            // Assert
            Assert.Equal("tu_usuario", usuario);
            Assert.Equal("tu_dominio", dominio);
            Assert.Equal("tu_clave", clave);
        }
    }
}