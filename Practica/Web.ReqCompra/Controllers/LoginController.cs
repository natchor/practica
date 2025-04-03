using Biblioteca.Seguridad;
using Entidad.Interfaz.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces.Services;
using System;
using Web.ReqCompra.Controllers;

namespace Web.Controllers
{
    public class LoginController : BaseController
    {

        private readonly IUserService _servUser;
        private Biblioteca.Librerias.LogEvent _log = new Biblioteca.Librerias.LogEvent();

        public LoginController(IUserService userService)
        {
            _servUser = userService;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public IActionResult LoginNow(UserLoginModel model, string returnUrl = null)
        {

            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");

                UserModel user = null;

                if (LdapConfig.UseLoginLdap)
                {
                    if (LdapUtils.ValidarLogin(model.UserName, model.Password))
                        user = _servUser.ExistUser(model.UserName);
                }
                else
                    user = _servUser.Login(model.UserName, model.Password);


                if (user != null)
                {
                    _ = CreateAuthenticationTicket(user);

                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction(nameof(HomeController.Index), "Home");


                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Usuario o clave no validos.");
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), ex, Biblioteca.Librerias.LogLevel.Error);

                ModelState.AddModelError(string.Empty, "Error al conectar, contacte al administrador o intentelo mas tarde.");
                return View("Index", model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
