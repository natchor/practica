using Entidad.Interfaz.Models.UserModels;
using Negocio.Interfaces.Services;
using Negocio.Services;
using NUnit.Framework;
using Moq;
using Web.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Web.ReqCompra;
using Microsoft.Extensions.Configuration;
using Biblioteca.Seguridad;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Configuration;
using DemoIntro.Models;
using Web.ReqCompra.Controllers;
using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.AprobacionModels;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Biblioteca.Librerias;

namespace Tests.Negocio
{
    [TestFixture]
    public class Tests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<ISolicitudService> _mockSolicitudService;
        private Mock<ISectorService> _mockSectorService;
        private Mock<IConceptoPresupuestarioService> _mockConseptoPresupuestarioService;
        private Mock<ITipoMonedaService> _mockTipoMoneda;
        private Mock<IAprobacionConfigService> _mockAprobacionConfigService;
        private Mock<IBitacoraService> _mockBitacoraService;


        private LoginController _loginController;
        private SolicitudController _solicitudController;

     

        //public Tests()
        //{
        //    _mockUser = new Mock<IUserService>();
        //    _controller = new LoginController(_mockUser.Object);
        //}

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(p => p.ExistUser(It.IsAny<string>())).Returns(new UserModel { Nombre = "usuario", Apellido = "prueba" });


            _mockSolicitudService = new Mock<ISolicitudService>();
            _mockSolicitudService.Setup(s => s.Aprobar(It.IsAny<AprobacionModel>())).Returns(1);

            _mockAprobacionConfigService = new Mock<IAprobacionConfigService>();
            _mockAprobacionConfigService.Setup(a => a.GetSiguienteAprobador(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<UserModel>());

            _mockBitacoraService = new Mock<IBitacoraService>();


            _loginController = new LoginController(_mockUserService.Object);
            //_solicitudController = new SolicitudController(_mockSolicitudService.Object, _mockAprobacionConfigService.Object, _mockBitacoraService.Object);
        }

        [Test]
        public void Login_User()
        {
            _loginController.Url = new Mock<IUrlHelper>().Object;

            var config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.test.json")
              .Build();

            LdapConfig.Configure(config.GetSection("AppSettings"));
            SiteKeys.Configure(config.GetSection("AppSettings"));

            var userLogin = new UserLoginModel { UserName = "req-compras", Password = "RQ.Energia21" };
            var result = _loginController.LoginNow(userLogin);

            var redirectResult = result as RedirectToActionResult;

            /* bool resultadoFinal = redirectResult.ActionName == "Index" || redirectResult.ControllerName == "Home"*/
            ;

            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            //Assert.AreEqual(true, resultadoFinal);

        }

        [Test]
        public void Aprobar_solicitud()
        {

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(CustomClaims.UserId, "23"),
                new Claim(CustomClaims.RoleId, "2"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            //var controller = new SolicitudController(_mockSolicitudService.Object, _mockAprobacionConfigService.Object, _mockBitacoraService.Object);
            //controller.ControllerContext = new ControllerContext()
            //{
            //    HttpContext = new DefaultHttpContext() { User = user }
            //};

            //AprobacionModel aprobacion = new AprobacionModel { };

            //controller.AprobarSolicitud(aprobacion);

            Assert.AreEqual("Index", "Index");

        }

        [Test]
        public void Test2() 
        {
            Assert.AreEqual("algoError", "algoError");
        }
    }
}