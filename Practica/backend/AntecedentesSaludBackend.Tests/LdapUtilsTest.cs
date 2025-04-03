using Microsoft.Extensions.Configuration;
using Moq;
using System.DirectoryServices;
using Xunit;

namespace AntecedentesSaludBackend.Tests
{
    public class LdapUtilsTests
    {
        private readonly IConfiguration _configuration;

        public LdapUtilsTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                        {"Ldap:UseLoginLdap", "true"},
                        {"Ldap:HostPath", "LDAP://10.0.0.81"},
                        {"Ldap:UserParam", "sAMAccountName"}
                    };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            LdapConfig.Configure(_configuration);
        }

    
        [Fact]
        public void ValidarLogin_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var mockDirectoryEntry = new Mock<DirectoryEntry>(MockBehavior.Strict);
            var mockDirectorySearcher = new Mock<DirectorySearcher>(mockDirectoryEntry.Object);
            var searchResult = new Mock<SearchResult>(MockBehavior.Strict);
            mockDirectorySearcher.Setup(ds => ds.FindOne()).Returns(searchResult.Object);

            // Act
            bool result = LdapUtils.ValidarLogin("valid_user", "valid_password");

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void ValidarLogin_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var mockDirectoryEntry = new Mock<DirectoryEntry>(MockBehavior.Strict);
            var mockDirectorySearcher = new Mock<DirectorySearcher>(mockDirectoryEntry.Object);
            mockDirectorySearcher.Setup(ds => ds.FindOne()).Returns((SearchResult)null);

            // Act
            bool result = LdapUtils.ValidarLogin("invalid_user", "invalid_password");

            // Assert
            Assert.False(result);
        }
    }

    // Assuming LdapConfig is a class that needs to be defined
    public static class LdapConfig
    {
        private static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetLdapHostPath()
        {
            return _configuration["Ldap:HostPath"];
        }

        public static string GetLdapUserParam()
        {
            return _configuration["Ldap:UserParam"];
        }
    }

    // Assuming LdapUtils is a class that needs to be defined
    public static class LdapUtils
    {
        public static bool ValidarLogin(string username, string password)
        {
            // Logic to validate login using LDAP
            return true; // Placeholder return value
        }
    }
}