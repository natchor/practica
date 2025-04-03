using Entidad.Interfaz.Models.CargoModels;
using Entidad.Interfaz.Models.RoleModels;
using Entidad.Interfaz.Models.UserModels;
using Entidad.Interfaz.Models.UserRoleModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Attributes.Filters;
using Web.Controllers;

namespace Web.ReqCompra.Controllers
{
    //[Route("[Controller]")]
    [Authorize(Role.Admin, Role.Presupuesto)]
    public class UsuarioController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _servUser;
        private readonly ICargoService _servCargo;
        private readonly IRoleService _servRol;
        private readonly IUserRoleService _servUserRole;

        public UsuarioController(ILogger<HomeController> logger, IUserService userService, ICargoService cargoService, IRoleService roleService, IUserRoleService userRoleService)
        {
            _logger = logger;
            _servUser = userService;
            _servCargo = cargoService;
            _servUserRole = userRoleService;
            _servRol = roleService;
        }

        // GET: UsuarioController
        public IActionResult Index()
        {
            // ViewBag.tablaUser = await _servUser.GetAllUser();

            return View();
        }

        [HttpGet("Usuario/GetUsuarios")]
        public async Task<IActionResult> GetUsuarios()
        {

            try
            {
                ViewBag.tablaUser = await _servUser.GetAllUser();

                return PartialView("Shared/_TablaUsuarios");

            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }




            return View();
        }


        [HttpGet("Usuario/EditarUsuario/{userId}")]
        public ActionResult EditarUsuario(string userId)
        {

            try
            {

                int id = Int32.Parse(userId);
                //ViewBag.User = _servUser.GetByUserId(id);
                ViewBag.userId = id;
                ViewBag.TituloUser = (id != 0) ? "Editar Usuario" : "Crear Usuario";

            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }

            return View();
        }

        [HttpGet("Usuario/GetFuncionariosForSelect")]
        public async Task<IActionResult> GetFuncionariosForSelect()
        {
            try
            {
                List<SelectUserModel> Userlist = await _servUser.GetForSelect();

                return Ok(Userlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Usuario/GetCargo")]
        public async Task<IActionResult> GetCargo()
        {
            try
            {
                List<SelectCargoModel> Sectlist = await _servCargo.GetForSelect();

                return Ok(Sectlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Usuario/GetRol")]
        public async Task<IActionResult> GetRol()
        {
            try
            {
                List<SelectRoleModel> Rollist = await _servRol.GetForSelect();

                return Ok(Rollist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Usuario/PostRol")]
        public ActionResult PostRol(string ids)
        {
            try
            {
                int id = Int16.Parse(ids);
                UserRoleModel user = _servUserRole.FindByUserId(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Usuario/PostUsuario")]
        public ActionResult PostUsuario(string ids)
        {
            try
            {
                int id = Int16.Parse(ids);
                UserModel user = _servUser.GetByUserId(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Usuario/PostValidaMail")]
        public ActionResult PostValidaMail(UserModel userm)
        {
            try
            {
                string msj = _servUser.ValidarUsuario(userm.Email);

                return Ok(msj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Usuario/PostGrabar")]
        public ActionResult PostGrabar(UserModel userm)
        {
            try
            {
                //var user = _servUser.GetByUserId(userm.Id);
                userm.JefeDirectoId = _servUser.BuscaJefe(userm.SectorId);
                //userm.UserName = user.UserName;
                //userm.Estado = user.Estado;
                int user = _servUser.Guardar(userm);

                //UserRoleModel uRole = new UserRoleModel();
                //uRole.RoleId = (int)userm.RolId;
                //uRole.UserId = user; 

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("Usuario/GetUsuarioCompra")]
        public async Task<ActionResult> GetUsuarioCompra()
        {
            try
            {

                List<UserModel> user = await _servUser.GetByUserCompra();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("Usuario/PostGrabarRol")]
        public ActionResult PostGrabarRol(UserRoleModel userR)
        {
            try
            {
                int user = _servUserRole.Guardar(userR);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
