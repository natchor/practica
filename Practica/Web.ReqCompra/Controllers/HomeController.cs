using Biblioteca.Librerias;
using DemoIntro.Models;
using Entidad.Interfaz.Models.EstadoModels;
using Entidad.Interfaz.Models.EstadoCompraModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Entidad.Interfaz.Models.BitacoraModels;
using Entidad.Interfaz.Models.UserRoleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Web.Attributes.Filters;
using Web.Controllers;
using Web.ReqCompra.Models;

using Dato.Respositories;


namespace Web.ReqCompra.Controllers
{

    //[Route("[Controller]")]
    [Authorize(Role.Admin, Role.Presupuesto)]
    public class HomeController : BaseController
    {

        private readonly IEmailService _servEmail;

        private readonly ILogger<HomeController> _logger;
        private readonly ISolicitudService _servSolicitud;
        private readonly ITipoMonedaService _servTipoMoneda;
        private readonly IEstadoService _servEstado;
        private readonly IEstadoCompraService _servEstadoCompra;
        private readonly IUserRoleService _servUser;
        private readonly IStoredProcedureRepository _repProcedure;
        private readonly IBitacoraService _servBitacora;

        public HomeController(ILogger<HomeController> logger
            , ISolicitudService solicitudService
            , ITipoMonedaService tipoMonedaService
            , IEmailService emailService
            , IEstadoService estadoService
            , IEstadoCompraService estadoCompraService
            , IStoredProcedureRepository procedureRepo
            , IBitacoraService bitacoraService
            , IUserRoleService userService)
        {
            _logger = logger;
            _servSolicitud = solicitudService;
            _servTipoMoneda = tipoMonedaService;
            _servEmail = emailService;
            _servEstado = estadoService;
            _servEstadoCompra = estadoCompraService;
            _servUser = userService;
            _servBitacora = bitacoraService;
            _repProcedure = procedureRepo;
        }

        public async Task<IActionResult> Index()
        {
            int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
           // GetItemOC("ejem");
            //ViewBag.tabla = await _servSolicitud.GetBySolicitanteId(userId);
            //ViewBag.tablaMisAprobaciones = await _servSolicitud.GetMisAprobaciones(userId);

            List<TipoMonedaModel> monedalist = await _servTipoMoneda.GetAll();
           // var ruta = "C://firmas//Plantilla.xlsx";
            
            //CrearReporte(); 

            ObtenerValorMonedas(monedalist);
            DateTime fecha = DateTime.Now;
            ViewData["Today"]= fecha;
            //SolicitudModel model = _servSolicitud.FindById(43);
            foreach (TipoMonedaModel item in monedalist)
            {
                string valor = string.Empty;
                if (item.FechaSolicitud != null && item.FechaSolicitud.ToString("yyyyMMdd") != fecha.ToString("yyyyMMdd"))
                {
                    try
                    {
                        if (item.Codigo != "CLP")
                        {
                            item.FechaSolicitud = fecha;
                            _servTipoMoneda.Guardar(item);
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
            }



            return View();
            //return File(ruta, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "plantilla.xlsx");
            
        }


        [HttpGet("Home/GetSolicitudes")]
        public async Task<IActionResult> GetSolicitudes()
        {
            try
            {
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();


                ViewBag.tablaMisSolictudes = await _servSolicitud.GetBySolicitanteId(userId);


                return PartialView("Shared/Tablas/_TablaSolicitudes");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Home/GetPorFinalizar")]
        public async Task<IActionResult> GetPorFinalizar()
        {
            try
            {
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
                UserRoleModel model = _servUser.FindByUserId(userId);

                ViewBag.tablaPorFinalizar = await _servSolicitud.GetPorFinalizar(model);

                return PartialView("Shared/Tablas/_TablaPorFinalizar");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("Home/GetMisAprobaciones")]
        public async Task<IActionResult> GetMisAprobaciones()
        {
            try
            {
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
                ViewBag.tablaMisAprobaciones = await _servSolicitud.GetMisAprobaciones(userId);




                return PartialView("Shared/Tablas/_TablaMisAprobaciones");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Home/SetFechaContrato")]
        public async Task<IActionResult> SetFechaContrato(string fechaI, string fechaF, int solicitudid)
        {
            try
            {
                DateTime iDate = DateTime.Parse(fechaI);
                DateTime fDate = DateTime.Parse(fechaF);
                SolicitudModel solicitud = _servSolicitud.FindById(solicitudid);
                solicitud.FechaFinContrato = iDate;
                solicitud.FechaInicioContrato = fDate;

                _servSolicitud.GuardarAsync(solicitud);

                


                return Ok(solicitud);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        

        [HttpGet("Home/GetListEstadoLicitacion")]
        public async Task<IActionResult> GetListEstadoLicitacion(int tipoCompra)
        {
            try
            {
                List<EstadoCompraModel> estados = await _servEstadoCompra.GetForSelectbyTpComp(tipoCompra);

                List<EstadoSelect2Model> estadoModel = new List<EstadoSelect2Model>();

                foreach (EstadoCompraModel estadoCompra in estados) {
                    EstadoSelect2Model estadoSelect2 = new EstadoSelect2Model();
                    estadoSelect2.CodigoStr = estadoCompra.Estado.CodigoStr;
                    estadoSelect2.Nombre = estadoCompra.Estado.Nombre;
                    estadoModel.Add(estadoSelect2);
                }

                return Ok(estadoModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Home/SetEstadoLicitacion")]
        public async Task<IActionResult> SetEstadoLicitacion(string estado, int solicitudid, string obs)
        {
            try
            {
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
                EstadoModel estados = await _servEstado.FindByCodStr(estado);

                SolicitudModel solicitud = await _servSolicitud.SetEstadoSolicitud(estado, solicitudid);

                string observacion = !string.IsNullOrEmpty(obs) ? $" con la siguiente observación: {obs}" : " sin observaciones";

                BitacoraModel model = guardarBitacora("Cambia estado a "+ estados.Nombre +", " + observacion, userId, solicitudid,1);
                _servBitacora.Guardar(model);
                
                return Ok(estados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Home/GetEstadosForSelect")]
        public async Task<IActionResult> GetEstadosForSelect()
        {
            try
            {
                List<EstadoSelectModel> estados = await _servEstado.GetForSelect();


                return Ok(estados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Home/GetGestion")]
        public async Task<IActionResult> GetGestion(GSFiltros filtros = null)
        {
            try
            {

                ViewBag.tablaGestionSolictudes = await _servSolicitud.GetTblGestionSolicitudes(filtros);


                return PartialView("Shared/Tablas/_TablaGestionSolicitudes");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Home/GetMisGestiones")]
        public async Task<IActionResult> GetMisGestiones()
        {
            try
            {
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
                ViewBag.tablaMisGestiones = await _servSolicitud.GetTblMisGestiones(userId);


                return PartialView("Shared/Tablas/_TablaMisGestiones");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpGet("Home/GetGestion/{estadoId?}")]
        //public async Task<IActionResult> GetGestion(int? estadoId = null)
        //{
        //    try
        //    {

        //        ViewBag.tablaGestionSolictudes = await _servSolicitud.GetTblGestionSolicitudes(estadoId._toInt());


        //        return PartialView("Shared/Tablas/_TablaGestionSolicitudes");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Tabla()
        {

            return View();
        }

        //[HttpPost("FormSubmit")]
        //[HttpPost]
        //public ActionResult FormSubmit()
        //{
        //    return RedirectToAction("Index", "Solicitud");


        //}

        [HttpGet]
        public ActionResult GetValidarSolicitud()
        {
            //var model = _modalValidar;
            return PartialView("Shared/Modal/_ModalValidaSolicitud");
        }


        [HttpGet("Home/GetConf")]
        public ActionResult GetConf()
        {
            return Json(new { sitio = SiteKeys.WebSiteDomain, tiemposesion = SiteKeys.TiempoSesionMin } );
        }
       

    }
}
