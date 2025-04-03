
using Biblioteca.Librerias;
using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.AprobacionModels;
using Entidad.Interfaz.Models.BitacoraModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Microsoft.AspNetCore.Mvc;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
using Web.Attributes.Filters;
using Web.Controllers;

namespace Web.ReqCompra.Controllers
{
    [Route("[Controller]")]
    [Authorize(Role.Admin, Role.Presupuesto)]
    public class AprobacionController : BaseController
    {
        private readonly ISolicitudService _servSolicitud;
        private readonly IUserService _servUser;
        private readonly ISectorService _servSector;
        private readonly ITipoMonedaService _servTipoMoneda;
        private readonly IConceptoPresupuestarioService _servConceptoPresupuestario;
        private readonly IModalidadCompraService _servModalidadCompra;
        private readonly ITipoCompraService _servTipoCompra;
        private readonly IProgramaPresupuestarioService _servProgPres;
        private readonly IArchivoService _servArchivo;
        private readonly IBitacoraService _servBitacora;


        //private readonly ILogger<HomeController> _logger;

        public static readonly LogEvent _log = new LogEvent();


        public AprobacionController(ISolicitudService solicitudService,
            IUserService userService,
            ISectorService sectorService,
            IConceptoPresupuestarioService conceptoPresupuestarioService,
            ITipoMonedaService tipoMonedaService,
            IModalidadCompraService modalidadCompraService,
            ITipoCompraService tipoCompraService,
            IProgramaPresupuestarioService programaPresupuestarioService,
            IArchivoService archivoService,
            IBitacoraService bitacoraService
            )
        {
            //_logger = logger;
            _servSolicitud = solicitudService;
            _servUser = userService;
            _servConceptoPresupuestario = conceptoPresupuestarioService;
            _servTipoMoneda = tipoMonedaService;
            _servSector = sectorService;
            _servModalidadCompra = modalidadCompraService;
            _servTipoCompra = tipoCompraService;
            _servProgPres = programaPresupuestarioService;
            _servArchivo = archivoService;
            _servBitacora = bitacoraService;
        }

        private async Task DefaultValues()
        {
            List<TipoMonedaModel> monedalist = await _servTipoMoneda.GetAll();

            ViewBag.ModalidadCompraSelectModel = _servModalidadCompra.GetSelect();
            ViewBag.TipoCompraSelectModel = _servTipoCompra.GetSelect();
            ViewBag.ContraparteTecnicaSelectModel = await _servUser.GetForSelect();
            ViewBag.ProgPresSelectModel = _servProgPres.GetForSelect();

            ObtenerValorMonedas(monedalist);
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            await DefaultValues();

            ViewBag.PageName = "Ingresar solicitud";

            return View();
        }


        [HttpGet("Aprobacion/GetBuscaSolicitud")]
        public async Task<IActionResult> GetBuscaSolicitud(string id)
        {
            int solId = await formateaNumero(id);
            if (solId == 0)
                return BadRequest("Solicitud no encontrada");

            try
            {

                //List<AprobacionModel> sol = _servSolicitud.(solicitud.Id);

                //solicitudId._toInt();
                ViewBag.tablaAprobaciones = await _servSolicitud.GetAprobacionesBySolicitudId(solId);
                return PartialView("Shared/Tablas/_TablaAprobaciones");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task<int> formateaNumero(string id)
        {
            try
            {
                string[] words = id.Split('-');
                string anho = words[0].Trim();
                string num = String.Format("{0:00000}", Int32.Parse(words[1].Trim()));
                string valor = anho + " - " + num;

                SolicitudModel sol = await _servSolicitud.FindByNumSolicitud(valor);
                return sol.Id;
            }
            catch
            {
                return 0;
            }
        }

        [HttpGet("Aprobacion/GetEliminarAprob")]
        public IActionResult GetEliminarAprob(int id)
        {
            try
            {
                AprobacionModel aprob = _servSolicitud.EliminarAprobacion(id);

                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
                BitacoraModel model = guardarBitacora($"Se quitó al aprobador {aprob.UserAprobador.FullName} correspondiente a la configuración: {aprob.AprobacionConfig.Nombre}. ", userId, aprob.SolicitudId);
                _servBitacora.Guardar(model);

                return Ok(aprob.SolicitudId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Aprobacion/SetAprobadorActual")]
        public async Task<IActionResult> SetAprobadorActual(int sigAprobador, string nroSolicitud)
        {
            try
            {
                var solicitudId = await formateaNumero(nroSolicitud);

                int ret = await _servSolicitud.SetAprobadorActual(sigAprobador, solicitudId);
                
                
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Aprobacion/GetConfigAprobacion")]
        public async Task<IActionResult> GetConfigAprobacion()
        {
            try
            {

                List<SelectAprobacionConfigModel> AproConfiglist = await _servSolicitud.GetConfigForSelect();

                return Ok(AproConfiglist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }




        [HttpPost("Aprobacion/PostEditarAprob")]
        public IActionResult PostEditarAprob(AprobacionModel model)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Aprobacion/PostCrearAprob")]
        public async Task<IActionResult> PostCrearAprob(AprobacionModel model)
        {
            try
            {
                int solId = await formateaNumero(model.Observacion);
                if (model.SolicitudId > 0) 
                    solId = model.SolicitudId;
                              
                if (solId == 0)
                    return BadRequest("Solicitud no encontrada");

                model.SolicitudId = solId;
                model.Observacion = null;

                int ret = _servSolicitud.GuardarAprobacionEnMatriz(model);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("Aprobacion/GetModalAprobacion")]
        public IActionResult GetModalAprobacion(int id)
        {
            try
            {
                //List<AprobacionModel> sol = _servSolicitud.(solicitud.Id);
                //solicitudId._toInt();
                //ViewBag.tablaAprobaciones = await _servSolicitud.GetAprobacionesBySolicitudId(id);
                return PartialView("Shared/Tablas/_ModalEditar");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}



