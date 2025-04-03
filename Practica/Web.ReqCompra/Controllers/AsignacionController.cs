using Biblioteca;
using Biblioteca.Librerias;
using Entidad.Interfaz;
using Entidad.Interfaz.Models.AprobacionModels;
using Entidad.Interfaz.Models.BitacoraModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.UserModels;
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
    public class AsignacionController : BaseController
    {
        private readonly ISolicitudService _servSolicitud;
        private readonly IUserService _servUser;
        private readonly ISectorService _servSector;
        private readonly ITipoMonedaService _servTipoMoneda;
        private readonly IConceptoPresupuestarioService _servConceptoPresupuestario;
        private readonly IModalidadCompraService _servModalidadCompra;
        private readonly ITipoCompraService _servTipoCompra;
        private readonly IProgramaPresupuestarioService _servProgPres;
        private readonly IAprobacionConfigService _servAprobacionConfig;
        private readonly IArchivoService _servArchivo;
        private readonly IBitacoraService _servBitacora;
        private readonly IEmailService _servEmail;



        //private readonly ILogger<HomeController> _logger;

        public static readonly LogEvent _log = new LogEvent();


        public AsignacionController(ISolicitudService solicitudService,
            IUserService userService,
            ISectorService sectorService,
            IConceptoPresupuestarioService conceptoPresupuestarioService,
            ITipoMonedaService tipoMonedaService,
            IModalidadCompraService modalidadCompraService,
            ITipoCompraService tipoCompraService,
            IProgramaPresupuestarioService programaPresupuestarioService,
            IAprobacionConfigService aprobacionConfigService,
            IArchivoService archivoService,
            IEmailService emailService,
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
            _servAprobacionConfig = aprobacionConfigService;
            _servArchivo = archivoService;
            _servEmail = emailService;
            _servBitacora = bitacoraService;
        }


        [HttpGet("Asignacion")]
        public IActionResult Asignacion()
        {
            return View(nameof(Asignacion));
        }


        [HttpGet("Asignacion/GetSolicitudesCompra")]
        public async Task<IActionResult> GetSolicitudesCompra(int id)
        {

            try
            {
                //int id = userId._toInt();
                List<SolicitudModel> sol = await _servSolicitud.FindByUserId(id);

                return Ok(sol);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("Asignacion/PostAsignarSolicitudesCompra")]
        public async Task<IActionResult> PostAsignarSolicitudesCompra(SolicitudAnalistaCompraModel model)
        {

            try
            {
                SolicitudModel solicitud = _servSolicitud.FindById(model.Id);
                UserModel usu = _servUser.GetByUserId(model.AnalistaProcesoId);
                bool tieneAnalistaCompra = _servSolicitud.TieneAnalistaDeCompra(solicitud.Id);

                bool permiteReasignacion = (solicitud.EstadoId == Estados.Aprobada
                    || solicitud.EstadoId == Estados.GenerandoOC
                    || solicitud.EstadoId == Estados.ProcesoAprobacion
                    || solicitud.EstadoId == Estados.Finalizada) && tieneAnalistaCompra;

                model.FechaDerivacion = DateTime.Now;
                model.EstadoId = Estados.Asignada;

                if (permiteReasignacion)
                {
                    model.FechaDerivacion = solicitud.FechaDerivacionAnalista;
                    model.EstadoId = solicitud.EstadoId;
                    model.AprobadorActualId = solicitud.AprobadorActualId;
                }

                var sol = await _servSolicitud.AsingarSolicitudAsync(model);

                if (permiteReasignacion)
                {

                    BitacoraModel bitacora = guardarBitacora("Solicitud reasignada a: " + usu.UserName, User.FindFirst(CustomClaims.UserId).Value._toInt(), sol.Id);
                    _servBitacora.Guardar(bitacora);

                    EmailMessage email = _servEmail.ArmaMensaje(usu.Email, usu.UserName, "Asignacion", sol);
                    _servEmail.Send(email);

                    return Ok(sol.Id);
                }

                AprobacionModel aprob = _servSolicitud.FindAprobacion(sol.Id, sol.AprobadorActualId._toInt());

                if (aprob == null)
                {
                    aprob = new AprobacionModel();
                }


                aprob.AprobacionConfigId = sol.AprobadorActualId._toInt(); //1012;
                aprob.SolicitudId = sol.Id;
                aprob.UserAprobadorId = model.AnalistaProcesoId;
                aprob.Observacion = null;
                aprob.FechaAprobacion = null;
                aprob.EstaAprobado = false;
                aprob.Orden = 1;



                int res = _servSolicitud.GuardarAprobacionEnMatriz(aprob);
                BitacoraModel bita = guardarBitacora("Solicitud asignada a: " + usu.UserName, User.FindFirst(CustomClaims.UserId).Value._toInt(), sol.Id);
                _servBitacora.Guardar(bita);
                //agregar aprobador que selecciono susana para la solicitud 
                // lamar a la funcion getaprobadores async

                EmailMessage mail = _servEmail.ArmaMensaje(usu.Email, usu.UserName, "Asignacion", sol, usu.UserName);
                _servEmail.Send(mail);

                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(-1);
            }

        }


    }
}



