using Biblioteca;
using Biblioteca.Librerias;
using Biblioteca.Seguridad;
using DemoIntro.Models;
using Entidad.Interfaz;
using Entidad.Interfaz.Models.OrdenCompraModels;
using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.AprobacionModels;
using Entidad.Interfaz.Models.ArchivoModels;
using Entidad.Interfaz.Models.BitacoraModels;
using Entidad.Interfaz.Models.ConceptoPresupuestarioModels;
using Entidad.Interfaz.Models.EstadoModels;
using Entidad.Interfaz.Models.ProgramaPresupuestarioModels;
using Entidad.Interfaz.Models.SectorModels;
using Entidad.Interfaz.Models.SolicitudDetalleModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.TipoCompraModels;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Entidad.Interfaz.Models.UserModels;
using LogLevel = Biblioteca.Librerias.LogLevel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
//using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes.Filters;
using Web.Controllers;
using Web.ReqCompra.Models;
using Entidad.Interfaz.Models.TipoCompraModels;
using Entidad.Interfaz.Models.ConvenioModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Services;
using System.Runtime.CompilerServices;
using Org.BouncyCastle.Crypto.Tls;

namespace Web.ReqCompra.Controllers
{
	[Route("[Controller]")]
    [Authorize(Role.Admin, Role.Presupuesto)]
    public class SolicitudController : BaseController
    {
        private readonly ISolicitudService _servSolicitud;
        private readonly IUserService _servUser;
        private readonly ISectorService _servSector;
        private readonly ITipoMonedaService _servTipoMoneda;
        private readonly IConceptoPresupuestarioService _servConceptoPresupuestario;
        private readonly IModalidadCompraService _servModalidadCompra;
        private readonly IOrdenCompraService _servOrdenCompra;
        private readonly ITipoCompraService _servTipoCompra;
        private readonly IProgramaPresupuestarioService _servProgPres;
        private readonly IAprobacionConfigService _servAprobacionConfig;
        private readonly IArchivoService _servArchivo;
        private readonly IBitacoraService _servBitacora;
        private readonly IEstadoService _servEstado;
        private readonly IEmailService _servEmail;
        private readonly IPropertiesSystemService _servPropSystem;
        private readonly IConvenioService _servConvenio;



		//private readonly ILogger<HomeController> _logger;

		public static readonly LogEvent _log = new LogEvent();


		public SolicitudController(ISolicitudService solicitudService,
            IUserService userService,
            ISectorService sectorService,
            IConceptoPresupuestarioService conceptoPresupuestarioService,
            ITipoMonedaService tipoMonedaService,
            IModalidadCompraService modalidadCompraService,
            IOrdenCompraService ordenCompraService,
            ITipoCompraService tipoCompraService,
            IProgramaPresupuestarioService programaPresupuestarioService,
            IArchivoService archivoService,
            IEmailService emailService,
            IAprobacionConfigService aprobacionConfigService,
            IEstadoService estadoService,
            IBitacoraService bitacoraService,
            IPropertiesSystemService propSystemService,
            IConvenioService convenioService
            )
        {
            //_logger = logger;
            _servSolicitud = solicitudService;
            _servUser = userService;
            _servConceptoPresupuestario = conceptoPresupuestarioService;
            _servTipoMoneda = tipoMonedaService;
            _servSector = sectorService;
            _servModalidadCompra = modalidadCompraService;
            _servOrdenCompra = ordenCompraService;
            _servTipoCompra = tipoCompraService;
            _servProgPres = programaPresupuestarioService;
            _servArchivo = archivoService;
            _servAprobacionConfig = aprobacionConfigService;
            _servEstado = estadoService;
            _servBitacora = bitacoraService;
            _servEmail = emailService;
            _servPropSystem = propSystemService;
            _servConvenio= convenioService;
        }

		private async Task DefaultValues(int sectorPadreId = 0)
        {
            List<TipoMonedaModel> monedalist = await _servTipoMoneda.GetAll();

            ViewBag.ModalidadCompraSelectModel = _servModalidadCompra.GetSelect();
            ViewBag.TipoCompraSelectModel = _servTipoCompra.GetSelect();
            ViewBag.ContraparteTecnicaSelectModel = await _servUser.GetForSelect();
            ViewBag.TipoMonedaSelect = await _servTipoMoneda.GetForSelect();

            //if (solicitudSectorDemandante == 0)
            //    solicitudSectorDemandante = User.FindFirst(CustomClaims.SectorDemandante).Value._toInt();


            //ViewBag.ProgPresSelectModel = _servProgPres.GetBySectorIdForSelect(solicitudSectorDemandante);

            if (sectorPadreId == 0)
                sectorPadreId = User.FindFirst(CustomClaims.SectorPadreId).Value._toInt();

            var sectorPadre = _servSector.FindById(sectorPadreId);

            ViewBag.SectorPadreStr = sectorPadre == null ? User.FindFirst(CustomClaims.Sector).Value : sectorPadre.Nombre;



            ViewBag.ProgPresSelectModel = _servProgPres.GetForSelect();

            ObtenerValorMonedas(monedalist);
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            await DefaultValues();
            ViewData["ANHO"] = _servPropSystem.FindByCodigo("ANHO").Valor.ToString();
            ViewBag.SectorDemandanteId = User.FindFirst(CustomClaims.SectorDemandante).Value;
            ViewBag.Accion = Acciones.Ingresar;
            ViewBag.PageName = "Ingresar solicitud";
            ViewBag.CertificadoSaldo = false;

            return View();
        }

        [HttpGet("FindDetalleBySolicitudId/{solicitudId}")]
        public async Task<IActionResult> FindDetalleBySolicitudId(int solicitudId)
        {

            List<SolicitudDetalleModel> detalle = await _servSolicitud.FindDetalleBySolicitudId(solicitudId);

            return Ok(detalle);
        }

        [HttpPost("ActualizaSolicitud")]
        public async Task<IActionResult> ActualizaSolicitud(SolicitudModel soli)
        {
            try
            {
                int ret  = soli.Id;
                string cambios = "Datos anteriores [";
                
                DateTime fecha = DateTime.Now;
                SolicitudModel solicitud = await _servSolicitud.FindByIdAsync(soli.Id);

                if (solicitud.TipoCompraId != soli.TipoCompraId)
                {
                    cambios += " Tipo compra: " + solicitud.TipoCompra.Nombre + ",";
                    solicitud.TipoCompraId = soli.TipoCompraId;
                    solicitud.TipoCompra = null;
                }
                if (solicitud.ObservacionGeneral != soli.ObservacionGeneral)
                {
                    cambios += " Comentarios: " + solicitud.ObservacionGeneral + ",";
                    solicitud.ObservacionGeneral = soli.ObservacionGeneral;
                }
                if (solicitud.UnidadDemandanteId != soli.UnidadDemandanteId)
                {
                    cambios += " Unidad Demandante: " + solicitud.UnidadDemandante.Nombre + ",";
                    solicitud.UnidadDemandanteId = soli.UnidadDemandanteId;
                    solicitud.UnidadDemandante = null;
                }
                if (solicitud.ProgramaPresupuestarioId != soli.ProgramaPresupuestarioId)
                {
                    cambios += " Programa Presupuestario: " + solicitud.ProgramaPresupuestario.Nombre + ",";
                    solicitud.ProgramaPresupuestarioId = soli.ProgramaPresupuestarioId;
                    solicitud.ProgramaPresupuestario = null;
                }
                if (solicitud.ConceptoPresupuestarioId != soli.ConceptoPresupuestarioId)
                {
                    cambios += " Concepto Presupuestario: " + solicitud.ConceptoPresupuestario.Nombre + ",";
                    solicitud.ConceptoPresupuestarioId = soli.ConceptoPresupuestarioId;
                    solicitud.ConceptoPresupuestario = null;
                }
                if (solicitud.ContraparteTecnicaId != soli.ContraparteTecnicaId)
                {
                    cambios += " Contraparte Técnica: " + solicitud.ContraparteTecnica.UserName + ",";
                    solicitud.ContraparteTecnicaId = soli.ContraparteTecnicaId;
                    solicitud.ContraparteTecnica = null;
                }
                if (solicitud.FoliocompromisoSIGFE != soli.FoliocompromisoSIGFE)
                {
                    cambios += " Folio Compromiso SIGFE: " + solicitud.FoliocompromisoSIGFE + ",";
                    solicitud.FoliocompromisoSIGFE = soli.FoliocompromisoSIGFE;
                }
                if (solicitud.FolioRequerimientoSIGFE != soli.FolioRequerimientoSIGFE)
                {
                    cambios += " Folio Requerimiento SIGFE: " + solicitud.FolioRequerimientoSIGFE + ",";
                    solicitud.FolioRequerimientoSIGFE = soli.FolioRequerimientoSIGFE;
                }
                if (solicitud.IniciativaVigente != soli.IniciativaVigente)
                {
                    cambios += " Iniciativa Vigente: " + solicitud.IniciativaVigente + ",";
                    solicitud.IniciativaVigente = soli.IniciativaVigente;
                }
                if (solicitud.IniciativaVigenteId != soli.IniciativaVigenteId)
                {
                    cambios += " ID Iniciativa Vigente: " + solicitud.IniciativaVigenteId + ",";
                    solicitud.IniciativaVigenteId = soli.IniciativaVigenteId;
                }
                if (solicitud.Archivos.Count() < soli.Archivos.Count()) {
                    solicitud.Archivos = soli.Archivos;
                }

                int funcId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario
                UserModel funcionario = _servUser.GetByUserId(funcId);
                cambios= cambios.Remove(cambios.Length - 1, 1) + " ] ";
                int res = await _servSolicitud.GuardarAsync(solicitud);

                BitacoraModel model = guardarBitacora("Solicitud de compra actualizada "+ cambios + "observación adicional: "+soli.ObjetivoJustificacion, funcionario.Id, ret);
                _servBitacora.Guardar(model);
                EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, funcionario.UserName, "ACTUALIZA", solicitud, solicitud.Solicitante.UserName);
                _servEmail.Send(mail);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                //ViewBag.StatusMessage = "Error al crear solicitud";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }



        [HttpPost("IngresarSolicitud")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IngresarSolicitud(SolicitudModel solicitud)
        {
            DateTime fecha = DateTime.Now;

            solicitud.SolicitanteId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario
            solicitud.FechaCreacion = fecha;
            UserModel funcionario = _servUser.GetByUserId((int)solicitud.SolicitanteId);
            try
            {
                if (solicitud.Id != 0)
                {
                    solicitud.AprobadorActualId = _servAprobacionConfig.GetPrimerId();
                    solicitud.EstadoId = Estados.Creada;
                }

                int ret = await _servSolicitud.GuardarAsync(solicitud);

                if (solicitud.Id == 0)
                {
                    string nroSolicitud = _servSolicitud.GetNroSolicitud(ret); 

                    solicitud.NroSolicitud = nroSolicitud;
                    solicitud.Id = ret;
                    BitacoraModel model = guardarBitacora("Solicitud de compra creada", solicitud.SolicitanteId, ret);
                    _servBitacora.Guardar(model);
                    EmailMessage mail = _servEmail.ArmaMensaje(funcionario.Email, funcionario.UserName, "Creacion", solicitud, funcionario.UserName);
                    _servEmail.Send(mail);



                }
                else
                {
                    solicitud.Id = ret;
                    BitacoraModel model = guardarBitacora("Solicitud de compra modificada", solicitud.SolicitanteId, ret);
                    _servBitacora.Guardar(model);
                    EmailMessage mail = _servEmail.ArmaMensaje(funcionario.Email, funcionario.UserName, "Modificada", solicitud);
                    _servEmail.Send(mail);
                }
                //ViewBag.StatusMessage = "Solicitud creada con exito";

                ProgramaPresupuestarioModel progPRes= _servProgPres.FindById(solicitud.ProgramaPresupuestarioId);

                if (progPRes.ConCS)
                {

                    ConvenioModel convenio = new ConvenioModel();
                    convenio.SolicitudId = solicitud.Id;
                    _servConvenio.Guardar(convenio);
                }



                return Ok(ret);
            }
            catch (Exception e)
            {
                //ViewBag.StatusMessage = "Error al crear solicitud";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost("Solicitud/GlosaCS")]
        public async Task<IActionResult> GlosaCS(SolicitudPresupuestoModel solicitud)
        {


            try
            {

                //sol.FoliocompromisoSIGFE = solicitud.FoliocompromisoSIGFE;
                //sol.FolioRequerimientoSIGFE = solicitud.FolioRequerimientoSIGFE;

                //int ret = await _servSolicitud.GuardarAsync(sol);
                int ret = await _servSolicitud.GuardarFoliosAsync(solicitud);

                if (solicitud.FoliocompromisoSIGFE == null)
                    return Ok(ret);

                var currentUserId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario
                var currentUser = _servUser.GetByUserId(currentUserId);

                string obs = !string.IsNullOrEmpty(solicitud.Observacion) ? $" con la siguiente observación: {solicitud.Observacion}" : " sin observaciones";

                BitacoraModel model = guardarBitacora($"Folio de compromiso SIGFE {solicitud.FoliocompromisoSIGFE} agregado por: {currentUser.UserName} {obs}", currentUserId, solicitud.Id);
                _servBitacora.Guardar(model);
                SolicitudModel sol = _servSolicitud.FindById(solicitud.Id);
                if (sol.FaseCDP.ToUpper() != "FASE 2")
                {
                    UserModel user = _servUser.GetByUserId((int)sol.AnalistaProcesoId);
                    EmailMessage mail = _servEmail.ArmaMensaje(user.Email, currentUser.UserName, "COMPROMISOFASE1", sol);
                    _servEmail.Send(mail);
                }


                return Ok(ret);
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al crear solicitud";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost("Solicitud/ExtraPresupuestario")]
        public async Task<IActionResult> ExtraPresupuestario(int id)
        {


            try
            {
                ConvenioModel conModel = _servConvenio.FindBySolicitudId(id);
               
                return Ok(conModel);
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al crear solicitud";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }


        [HttpPost("Solicitud/GlosaTransferencia")]
        public async Task<IActionResult> GlosaTransferencia(ConvenioModel convenio)
        {


            try
            {
                ConvenioModel conModel = _servConvenio.FindBySolicitudId(convenio.SolicitudId);
                conModel.Antecedente=convenio.Antecedente;

                var ret = _servConvenio.Guardar(conModel);

                return Ok(ret);
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al guardar GlosaCS";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost("Solicitud/DatoPresupuesto")]
        public async Task<IActionResult> DatoPresupuesto(SolicitudPresupuestoModel solicitud)
        {


            try
            {
                
                //sol.FoliocompromisoSIGFE = solicitud.FoliocompromisoSIGFE;
                //sol.FolioRequerimientoSIGFE = solicitud.FolioRequerimientoSIGFE;

                //int ret = await _servSolicitud.GuardarAsync(sol);
                int ret = await _servSolicitud.GuardarFoliosAsync(solicitud);

                if (solicitud.FoliocompromisoSIGFE == null)
                    return Ok(ret);

                var currentUserId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario
                var currentUser = _servUser.GetByUserId(currentUserId);

                string obs = !string.IsNullOrEmpty(solicitud.Observacion) ? $" con la siguiente observación: {solicitud.Observacion}" : " sin observaciones";

                BitacoraModel model = guardarBitacora($"Folio de compromiso SIGFE {solicitud.FoliocompromisoSIGFE} agregado por: {currentUser.UserName} {obs}", currentUserId, solicitud.Id);
                _servBitacora.Guardar(model);
                SolicitudModel sol = _servSolicitud.FindById(solicitud.Id);
                if (sol.FaseCDP.ToUpper() != "FASE 2") {
                    UserModel user = _servUser.GetByUserId((int)sol.AnalistaProcesoId);
                    EmailMessage mail = _servEmail.ArmaMensaje(user.Email, currentUser.UserName, "COMPROMISOFASE1", sol);
                    _servEmail.Send(mail);
                }


                return Ok(ret);
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al crear solicitud";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost("AprobarSolicitud")]
        [ValidateAntiForgeryToken]
        public IActionResult AprobarSolicitud(AprobacionModel aprobacion)
        {

            int currentUserId = User.FindFirst(CustomClaims.UserId).Value._toInt();
            aprobacion.UserAprobadorId = currentUserId;//idUsuario
            int userRoleId = User.FindFirst(CustomClaims.RoleId).Value._toInt();

            if (userRoleId == Roles.AnalistaPresupuesto)
            {
                aprobacion.AnalistaPresupuestoId = aprobacion.UserAprobadorId;
            }


            try
            {
                int ret = _servSolicitud.Aprobar(aprobacion);

                var nuvoArchivos = aprobacion.Archivos?.Where(a => a.Id == 0).ToList();

                if (nuvoArchivos != null)
                    foreach (var item in nuvoArchivos)
                    {
                        item.SolicitudId = aprobacion.SolicitudId;
                        _servArchivo.Guardar(item);
                    }
                

                string mensaje = "";

                switch (aprobacion.AprobacionConfigId)
                {
                    case 1012://1012    1   Analista de Compra
                        mensaje = "Valida factibilidad Técnica, " + aprobacion.Observacion;

                        break;
                    case 1015:// Obtencion del CDP
                    case 1:
                        mensaje = "Valida disponibilidad Presupuestaria, " + aprobacion.Observacion;

                        break;
                    case 1016:
                        mensaje = "Aprobación excepcional de solicitud, " + aprobacion.Observacion;
                        break;
                    default:
                        mensaje = "Solicitud aprobada, " + aprobacion.Observacion;
                        break;
                }

                //string mensaje = "Valida disponibilidad Presupuestaria";
                //string mensaje = 


                //UserModel usu = _servUser.GetByUserId(aprobacion.UserAprobadorId);
                BitacoraModel model = guardarBitacora(mensaje, aprobacion.UserAprobadorId, aprobacion.SolicitudId);
                _servBitacora.Guardar(model);


                SolicitudModel solicitud = _servSolicitud.FindById(model.SolicitudId);
                List<UserModel> siguiente = _servAprobacionConfig.GetSiguienteAprobador(model.SolicitudId, (int)solicitud.AprobadorActualId);
                var currentUser = siguiente.Where(s => s.Id == currentUserId).FirstOrDefault();
                siguiente.Remove(currentUser);

                string correos = obtenerCorreos(siguiente);

                UserModel funcionarioAprueba = _servUser.GetByUserId((int)aprobacion.UserAprobadorId);
                //ViewBag.StatusMessage = "Solicitud aprobada con exito";
                EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, funcionarioAprueba.UserName, "Aprobacion", solicitud, solicitud.Solicitante.UserName);
                if (aprobacion.AprobacionConfigId == 1012)
                {
                    mail.Content = mail.Content.Replace("aprobada", "ratificada");
                    mail.Subject = mail.Subject.Replace("Aprobación", "Factibilidad Técnica ");
                }
                _servEmail.Send(mail);
                EmailMessage mail2 = _servEmail.ArmaMensaje(correos, "", "Siguiente", solicitud);
                _servEmail.Send(mail2);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        private string obtenerCorreos(List<UserModel> siguiente)
        {
            string correos = "";
            foreach (var item in siguiente)
            {
                correos = correos + item.Email + ",";
            }
            return correos;
        }

        

        [HttpPost("ValidarOC")]
        public IActionResult ValidarOC(OCSolicitudModel OCSolicitud)
        {

            string ret = GetItemOC(OCSolicitud.NumOrdenCompra);
            // OrdenCompraModel ordenCompra =   JsonSerializer.Deserialize<OrdenCompraModel>(ret);

            return Ok(ret);
            //return !string.IsNullOrEmpty(ret) ? Ok(ret) : Error();

        }

        [HttpPost("GuardarOC")]
        public IActionResult GuardarOC(OrdenCompraModel OCModel)
        {
            int user= User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario
            //string ret = GetItemOC(OCSolicitud.NumOrdenCompra);
            // OrdenCompraModel ordenCompra =   JsonSerializer.Deserialize<OrdenCompraModel>(ret);
            OrdenCompraModel OC = _servOrdenCompra.FindByOC(OCModel.CodigoOC);
            if (OC != null)
                OCModel.Id = OC.Id;
            int ret = _servOrdenCompra.Guardar(OCModel);

            if (ret > 0)
            {
                BitacoraModel model = guardarBitacora("Se ha guardado la Orden de compra " + OCModel.CodigoOC + " desde mercado público", user, OCModel.SolicitudId);
                _servBitacora.Guardar(model);

            }

            return Ok(ret);
            //return !string.IsNullOrEmpty(ret) ? Ok(ret) : Error();

        }




        [HttpPost("GuardarArchivo")]
        //[ValidateAntiForgeryToken]
        public IActionResult GuardarArchivo(OCSolicitudModel OCSolicitud)
        {

            int ret = 0;

            List<ArchivoModel> nuevos = OCSolicitud.Archivos.Where(x => x.Id == 0).ToList();
            int solicitudId = OCSolicitud.Archivos.Where(x => x.Id != 0).ToList().First().SolicitudId;

            foreach (var item in nuevos)
            {
                item.SolicitudId = solicitudId;
                ret += _servArchivo.Guardar(item);
            }

            
            //int user = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario
            //UserModel usu = _servUser.GetByUserId(aprobacion.UserAprobadorId);




            return ret > 0 ? Ok(ret) : Error();

        }

        [HttpPost("GenerarOC")]
        [ValidateAntiForgeryToken]
        public IActionResult GenerarOC(OCSolicitudModel OCSolicitud)
        {
            int user = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario
            SolicitudModel Solicitud = _servSolicitud.FindById(OCSolicitud.SolicitudId);
            //if (Solicitud.FaseCDP == "FASE 2")
            //{
            //    OrdenCompraModel OrdenCompra = _servOrdenCompra.FindByOC(OCSolicitud.NumOrdenCompra);
                
            //    if ((decimal)Solicitud.MontoAprox != OrdenCompra.Total)
            //    {
            //        OCSolicitud.MontoOC = OrdenCompra.Total;
            //        BitacoraModel model = guardarBitacora("CDP Fase 2 ajustado por Sistema, Monto Presupuestado = "+ Solicitud.MontoAprox +" Monto OC = " + OrdenCompra.Total.ToString(), user, OCSolicitud.SolicitudId);
            //        _servBitacora.Guardar(model);
            //    }
            //}

            int ret = _servSolicitud.GenerarOC(OCSolicitud);
            SolicitudModel solicitud = _servSolicitud.FindById(OCSolicitud.SolicitudId);
            //UserModel usu = _servUser.GetByUserId(aprobacion.UserAprobadorId);  
            //string oc=GetItemOC(OCSolicitud.NumOrdenCompra);
            if (OCSolicitud.EstadoStr == "FINALIZADA")
            {
                BitacoraModel model = guardarBitacora("Solicitud de compra Finalizada", user, OCSolicitud.SolicitudId);
                _servBitacora.Guardar(model);


                UserModel contraparteTecnica = _servUser.GetByUserId(solicitud.ContraparteTecnicaId);
                string correo = solicitud.Solicitante.Email + "," + contraparteTecnica.Email ;
                EmailMessage mail = _servEmail.ArmaMensaje(correo, solicitud.Solicitante.UserName, "FINALIZA", solicitud);
                _servEmail.Send(mail);

                //mail = _servEmail.ArmaMensaje(solicitud.ContraparteTecnica.Email, solicitud.Solicitante.UserName, "FINALIZA", solicitud);
                //_servEmail.Send(mail);

            } else if (solicitud.FaseCDP.ToUpper() != "FASE 2")
            {
                SolicitudDetalleModel detalle = _servSolicitud.FindDetalleAnioActualById(OCSolicitud.SolicitudId);
                string observaciones = "Con los siguientes datos: <br/>Nombre Proveedor: " + solicitud.ProveedorNombre + " <br/>Rut Proveedor: " + solicitud.ProveedorRut + " <br/>Orden de compra: " + solicitud.OrdenCompra + " <br/>Monto Año "+detalle.Anio+": "+detalle.MontoMonedaSel + " "+ Solicitud.TipoMoneda.Nombre;
                UserModel funcionario = _servUser.GetByUserId(user);
                solicitud.ObservacionGeneral = "0-" + observaciones;
                EmailMessage mail = _servEmail.ArmaMensaje("", funcionario.FullName, "GeneraOC", solicitud);
                _servEmail.Send(mail);

            }



            return ret > 0 ? Ok(ret) : Error();

        }

        [HttpGet("GetArchivos/{solicitudId}")]
        public ActionResult GetArchivos(int solicitudId)
        {
            ////Get the images list from repository
            //var attachmentsList = new List<AttachmentsModel>
            //{
            //    new AttachmentsModel {AttachmentID = 1,
            //    FileName = "/images/wallimages/dropzonelayout.png",
            //    Path = "/images/wallimages/dropzonelayout.png"},
            //    new AttachmentsModel {AttachmentID = 1,
            //    FileName = "/images/wallimages/imageslider-3.png",
            //    Path = "/images/wallimages/imageslider-3.png"}
            //}.ToList();
            List<ArchivoModel> archivos = _servArchivo.FindBySolicitudId(solicitudId);



            //return Json(new { Data = attachmentsList }, JsonRequestBehavior.AllowGet);
            return Json(new { Data = archivos });
        }

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")] // for Zip files with form data
        [HttpPost("SubirArchivo")]
        public JsonResult SubirArchivo(ICollection<IFormFile> files)
        {
            string relativePath = string.Empty;
            //List<string> paths = new List<string>();
            ArchivoModel archivo = null;
            try
            {
                string path = SiteKeys.FilesPath;
                int cantSubidos = 0;


                using (WindowsLogin wl = new WindowsLogin())
                {
                    System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                    {
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        foreach (var file in files)
                        {
                            var newName = $"{DateTime.Now.ToString("HHmmssffff")}_{file.FileName}";
                            relativePath = Path.Combine(path, newName);

                            using (var stream = System.IO.File.Create(relativePath))
                            {
                                file.CopyTo(stream);

                                //paths.Add(relativePath);
                                cantSubidos++;
                            };
                            
                            archivo = new ArchivoModel
                            {
                                Ext = file.FileName._getExtension(),
                                FullPath = relativePath,
                                Nombre = file.FileName,
                                Size = file.Length,
                                UsuarioId = User.FindFirst(CustomClaims.UserId).Value._toInt(),
                                FechaCreacion = DateTime.Now
                            };

                        }

                    });
                };

            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), $"Error al subir archivo {relativePath}", LogLevel.Error);
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);
            }

            //return relativePath;
            return Json(archivo);

        }

        [HttpGet("DescargarArchivo/{archivoId}")]
        public IActionResult DescargarArchivo(int archivoId)
        {
            ArchivoModel archivo = _servArchivo.FindById(archivoId);

            if (archivo == null)
            {
                return BadRequest("Archivo no registrado en base de datos");
            }

            string filePath = archivo.FullPath;
            string fileName = archivo.Nombre;
            byte[] fileBytes = null;
            string contentType = string.Empty;

            using (WindowsLogin wl = new WindowsLogin())
            {
                System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                {
                    fileBytes = System.IO.File.ReadAllBytes(filePath);

                    if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType))
                        contentType = "application/force-download";

                });
            };

            return File(fileBytes, contentType, fileName);
        }

        [HttpGet("VerArchivo/{archivoId}")]
        public IActionResult VerArchivo(int archivoId)
        {
            ArchivoModel archivo = _servArchivo.FindById(archivoId);

            if (archivo == null)
            {
                return BadRequest("Archivo no registrado en base de datos");
            }

            string filePath = archivo.FullPath;
            string fileName = archivo.Nombre;
            byte[] fileBytes = null;
            string contentType = string.Empty;

            using (WindowsLogin wl = new WindowsLogin())
            {
                System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                {
                    fileBytes = System.IO.File.ReadAllBytes(filePath);

                    if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType))
                        contentType = "application/force-download";

                });
            };

            Stream stream = new MemoryStream(fileBytes);

            return new FileStreamResult(stream, contentType);
        }





        [HttpPost("RechazarSolicitud")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RechazarSolicitud(AprobacionModel aprobacionEntity)
        {

            try
            {
                aprobacionEntity.UserAprobadorId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario

                int result = await _servSolicitud.RechazarSolicitud(aprobacionEntity);

                var nuvoArchivos = aprobacionEntity.Archivos?.Where(a => a.Id == 0).ToList();

                if (nuvoArchivos != null)
                    foreach (var item in nuvoArchivos)
                    {
                        item.SolicitudId = aprobacionEntity.SolicitudId;
                        _servArchivo.Guardar(item);
                    }

                UserModel usu = _servUser.GetByUserId(aprobacionEntity.UserAprobadorId);

                BitacoraModel bita = guardarBitacora("Solicitud Rechazada por: " + usu.UserName + ", observación: " + aprobacionEntity.Observacion, usu.Id, aprobacionEntity.SolicitudId);
                _servBitacora.Guardar(bita);

                SolicitudModel solicitud = _servSolicitud.FindById(bita.SolicitudId);
                EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, usu.UserName, "Rechaza", solicitud);
                _servEmail.Send(mail);
                

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception ex)
            {
                return Error();
            }

        }

        [HttpPost("AprobacionesBita")]
        public async Task<IActionResult> AprobacionesBita(int id)
        {

            try
            {


                List<AprobacionModel> aprobaciones = await _servSolicitud.GetAprobacionesBySolicitudId(id);


                return Ok(aprobaciones);
            }
            catch (Exception)
            {
                return Error();
            }

        }

        [HttpPost("Bitacora")]
        public async Task<IActionResult> Bitacora(int id)
        {

            try
            {

                List<BitacoraModel> bitacora = await _servBitacora.FindBySolicitudId(id);


                return Ok(bitacora);
            }
            catch (Exception ex)
            {
                return Error();
            }

        }


        [HttpPost("BitacoraEstado")]
        public async Task<IActionResult> BitacoraEstado(int id)
        {

            try
            {

                List<BitacoraModel> bitacora = await _servBitacora.FindBitaEstadosBySolicitudId(id);


                return Ok(bitacora);
            }
            catch (Exception ex)
            {
                return Error();
            }

        }

        [HttpPost("BitacoraArchivo")]
        public async Task<IActionResult> BitacoraArchivo(int id)
        {

            try
            {

                List<ArchivoTablaModel> bitacoraArchivo = _servArchivo.GetForBitacora(id);


                return Ok(bitacoraArchivo);
            }
            catch (Exception ex)
            {
                return Error();
            }

        }

        [HttpPost("AprobarCS2")]
        public async Task<String> AprobarCS2(ConvenioModel obj)
        {

            DateTime fecha = DateTime.Now;

            int user = User.FindFirst(CustomClaims.UserId).Value._toInt();
            int cargo = User.FindFirst(CustomClaims.CargoId).Value._toInt();
            int rol = User.FindFirst(CustomClaims.RoleId).Value._toInt();
           
            //if (!perfilCorrecto)
            //    return "Perfil incorrecto no puede aprobar CDP";


            try
            {
                ConvenioModel certificadoSaldo = _servConvenio.FindBySolicitudId(obj.SolicitudId);
                SolicitudModel solicitud = _servSolicitud.FindById(obj.SolicitudId);
                certificadoSaldo.FechaAutorizacionPres = fecha;
                certificadoSaldo.AutorizadorPresId = user;


                int result = _servConvenio.Guardar(certificadoSaldo);
                //certificadoSaldo = _servConvenio.FindBySolicitudId(obj.SolicitudId);
                UserModel usu = _servUser.GetByUserId(user);

                BitacoraModel bita = guardarBitacora(certificadoSaldo.CertificadoSaldo + " Aprobado por: " + usu.UserName, user, certificadoSaldo.SolicitudId);
                _servBitacora.Guardar(bita);

                EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, usu.UserName, "CS", solicitud, solicitud.Solicitante.UserName);
                _servEmail.Send(mail);

                return result.ToString();
            }
            catch (Exception)
            {
                return "Error en Validación CS";
            }

        }

        [HttpPost("AprobarCS1")]
        public async Task<String> AprobarCS1(ConvenioModel obj)
        {

            DateTime fecha = DateTime.Now;

            int user = User.FindFirst(CustomClaims.UserId).Value._toInt();
            int cargo = User.FindFirst(CustomClaims.CargoId).Value._toInt();
            int rol = User.FindFirst(CustomClaims.RoleId).Value._toInt();
            bool perfilCorrecto = (rol == 9  );
            //if (!perfilCorrecto)
            //    return "Perfil incorrecto no puede aprobar CDP";


            try
            {
                ConvenioModel certificadoSaldo = _servConvenio.FindBySolicitudId(obj.SolicitudId);
                SolicitudModel solicitud = _servSolicitud.FindById(obj.SolicitudId);
                UserModel usu = _servUser.GetByUserId(user);

                certificadoSaldo.FechaAutorizacionFin = fecha;
                certificadoSaldo.AutorizadorFinId = user;
                certificadoSaldo.Banco=obj.Banco;
                certificadoSaldo.SaldoCuenta = obj.SaldoCuenta;
                certificadoSaldo.CuentaCorriente = obj.CuentaCorriente;
                int result = 0;

                if (certificadoSaldo.CertificadoSaldo == null)
                {
                    solicitud.FechaValidacionCDP = fecha;
                    solicitud.FuncionarioValidacionCDPId = user;
                    solicitud.ValidacionCDP = true;


                    await _servSolicitud.GuardarAsync(solicitud);

                    result = await _servConvenio.AprobarCS(certificadoSaldo);
                    certificadoSaldo = _servConvenio.FindBySolicitudId(obj.SolicitudId);

                    BitacoraModel bita = guardarBitacora(certificadoSaldo.CertificadoSaldo + " Aprobado por: " + usu.UserName, user, certificadoSaldo.SolicitudId);
                    _servBitacora.Guardar(bita);

                    EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, usu.UserName, "CS", solicitud, solicitud.Solicitante.UserName);
                    _servEmail.Send(mail);

                }
                else {
                    solicitud.FechaValidacionCDP = fecha;
                    solicitud.FuncionarioValidacionCDPId = user;
                    solicitud.ValidacionCDP = true;


                    await _servSolicitud.GuardarAsync(solicitud);
                }
                

                

                return result.ToString();
            }
            catch (Exception)
            {
                return "Error en Validación CS";
            }

        }

        [HttpPost("AprobarCDP")]
        public async Task<String> AprobarCDP(int id)
        {

            DateTime fecha = DateTime.Now;

            int user = User.FindFirst(CustomClaims.UserId).Value._toInt();    
            int cargo = User.FindFirst(CustomClaims.CargoId).Value._toInt();  
            int rol = User.FindFirst(CustomClaims.RoleId).Value._toInt(); 
            bool perfilCorrecto = (rol == 2 || rol == 3 || rol == 4);
            //if (!perfilCorrecto)
            //    return "Perfil incorrecto no puede aprobar CDP";


            try
            {
                SolicitudModel solicitud = _servSolicitud.FindById(id);  
                solicitud.FechaValidacionCDP = fecha; 
                solicitud.FuncionarioValidacionCDPId = user; 
                solicitud.ValidacionCDP = true;

                int result = await _servSolicitud.AprobarCDP(solicitud); 
                UserModel usu = _servUser.GetByUserId(user); 

                BitacoraModel bita = guardarBitacora(solicitud.CDPNum + " Aprobado por: " + usu.UserName, user, solicitud.Id); 
                _servBitacora.Guardar(bita); 
                EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, usu.UserName, "CDP", solicitud, solicitud.Solicitante.UserName); 
                _servEmail.Send(mail); 
                //_servSolicitud.CrearPDF(id);
                return result.ToString(); 

            }

            catch (Exception)
            {
                return "Error en Validación CDP";

            }

        }

        [HttpGet("GetDescargaCDP")] //falta que al aprobar el CDP también se visualice el botón de descargar CDP
        public IActionResult GetDescargaCDP(int id)
        {
            try
            {
                List<ArchivoModel> CDPS = _servArchivo.FindCDPBySolicitudId(id);

                if (CDPS != null && CDPS.Count > 0)
                {

                    string filePath = CDPS[0].FullPath;
                    string fileName = CDPS[0].Nombre;
                    byte[] fileBytes = null;
                    string contentType = string.Empty;

                    using (WindowsLogin wl = new WindowsLogin())
                    {
                        System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                        {
                            fileBytes = System.IO.File.ReadAllBytes(filePath);

                            if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType))
                                contentType = "application/force-download";

                        });
                    };

                    return File(fileBytes, contentType, fileName);
                }

                else
                {
                    //Método con libreria SelectPDF
                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                    //SelectPdf.PdfDocument doc = converter.ConvertUrl($"http://localhost:5000/CDP/{id}"); //parametrizar la url
                    string parameterized_path = SiteKeys.WebSiteDomain; //url del dominio parametrizada
                    SelectPdf.PdfDocument doc = converter.ConvertUrl($"{parameterized_path}/CDP/{id}");

                    // Convierte el PDF a bytes
                    byte[] pdfBytes;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        doc.Save(stream);
                        pdfBytes = stream.ToArray();
                    }
                    doc.Close();

                    string nombre = $"CDP_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";

                    ArchivoModel archivo = _servSolicitud.GuardarPDF(id, pdfBytes, nombre);
                    archivo.SolicitudId = id;
                    _servArchivo.Guardar(archivo);

                    return File(pdfBytes, "application/pdf", nombre);
                }

            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), $"Error al descargar pdf para solicitudId {id}", LogLevel.Error);
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);
                return Content("Error al descargar el archivo.");
            }
        }


        [HttpGet("GetDescargaCS")] 
        public IActionResult GetDescargaCS(int id)
        {
            try
            {
                List<ArchivoModel> CDPS = _servArchivo.FindCSBySolicitudId(id);

                if (CDPS != null && CDPS.Count > 0)
                {

                    string filePath = CDPS[0].FullPath;
                    string fileName = CDPS[0].Nombre;
                    byte[] fileBytes = null;
                    string contentType = string.Empty;

                    using (WindowsLogin wl = new WindowsLogin())
                    {
                        System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                        {
                            fileBytes = System.IO.File.ReadAllBytes(filePath);

                            if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType))
                                contentType = "application/force-download";

                        });
                    };

                    return File(fileBytes, contentType, fileName);
                }

                else
                {
                    //Método con libreria SelectPDF
                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                    //SelectPdf.PdfDocument doc = converter.ConvertUrl($"http://localhost:5000/CDP/{id}"); //parametrizar la url

                    string parameterized_path = SiteKeys.WebSiteDomain; //url del dominio parametrizada
                    SelectPdf.PdfDocument doc = converter.ConvertUrl($"{parameterized_path}/CS/{id}");

                    // Convierte el PDF a bytes
                    byte[] pdfBytes;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        doc.Save(stream);
                        pdfBytes = stream.ToArray();
                    }
                    doc.Close();
                    string nombre = $"{id}-CS_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";

                    ArchivoModel archivo = _servSolicitud.GuardarPDF(id, pdfBytes,nombre);
                    archivo.SolicitudId = id;
                    _servArchivo.Guardar(archivo);

                    return File(pdfBytes, "application/pdf", nombre );
                }

            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), $"Error al descargar pdf para solicitudId {id}", LogLevel.Error);
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);
                return Content("Error al descargar el archivo.");
            }
        }


        /*[HttpPost("GetDescargaCDP")]
        public IActionResult GetDescargaCDP(int id)
        {
            try
            {
                string pdfFilePath = Path.Combine(SiteKeys.FilesPath, $"{id}-CDP.pdf");
                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                SelectPdf.PdfDocument doc = converter.ConvertUrl($"http://localhost:5000/CDP/{id}");

                // Guarda el PDF en el servidor
                doc.Save(pdfFilePath);
                doc.Close();

                // Verifica si el archivo existe
                if (System.IO.File.Exists(pdfFilePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                    return File(fileBytes, "application/pdf", $"{id}-CDP.pdf");
                }
                else
                {
                    return Content("El archivo no existe.");
                }
            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), $"Error al descargar pdf para solicitudId {id}", LogLevel.Error);
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);
                return Content("Error al descargar el archivo.");
            }
        }
        */

        [HttpPost("DeshacerEstado")]
        public async Task<IActionResult> DeshacerEstado(AprobacionModel id)
        {

            try
            {
                string obs = !string.IsNullOrEmpty(id.Observacion) ? $" con la siguiente observación: {id.Observacion}" : " sin observaciones";
                SolicitudModel solicitud = _servSolicitud.FindById(id.SolicitudId);
                solicitud.EstadoId = 3; //anulada este dato podria cambiar
                solicitud.OrdenCompra = null;
                solicitud.FechaOrdenCompra = null;
                solicitud.ProveedorNombre = null;
                solicitud.ProveedorRut = null;
                solicitud.FoliocompromisoSIGFE = null;



                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario

                int res = await _servSolicitud.GuardarAsync(solicitud);
                UserModel funcionario = _servUser.GetByUserId(userId);
                
                BitacoraModel model = guardarBitacora($"Se quita estado finalizada por: {funcionario.FullName} {obs}", funcionario.Id, id.SolicitudId);
                _servBitacora.Guardar(model);
                EmailMessage mail = _servEmail.ArmaMensaje(funcionario.Email, funcionario.UserName, "DESTERMINA", solicitud);
                _servEmail.Send(mail);



                return Ok("OK");
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al ajustar CDP";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost("RechazarProceso")]
        public async Task<IActionResult> RechazarProceso(AprobacionModel id)
        {

            try
            {
                
                SolicitudModel solicitud = _servSolicitud.FindById(id.SolicitudId);
                //AprobacionConfigModel config = _servAprobacionConfig.FindById((int)solicitud.AprobadorActualId);

                List<UserModel> model = _servAprobacionConfig.GetSiguienteAprobador(solicitud.Id, (int)solicitud.AprobadorActualId);
                id.UserAprobadorId = model[0].Id;
                int result = await _servSolicitud.RechazarSolicitud(id);


                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario


                UserModel usu = _servUser.GetByUserId(userId);


                BitacoraModel bita = guardarBitacora("Solicitud Rechazada por: " + usu.UserName + ", observación: " + id.Observacion, usu.Id, id.SolicitudId);
                _servBitacora.Guardar(bita);

                
                EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, usu.UserName, "Rechaza", solicitud);
                _servEmail.Send(mail);








                return Ok("OK");
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al rechazar solicitud";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost("Anular")]
        public async Task<IActionResult> Anular(AprobacionModel id)
        {

            try
            {
                string obs = !string.IsNullOrEmpty(id.Observacion) ? $" con la siguiente observación: {id.Observacion}" : " sin observaciones";
                SolicitudModel solicitud = _servSolicitud.FindById(id.SolicitudId);
                solicitud.EstadoId = 11; //anulada este dato podria cambiar
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();//idUsuario

                int res = await _servSolicitud.GuardarAsync(solicitud);
                UserModel funcionario = _servUser.GetByUserId(userId);

                BitacoraModel model = guardarBitacora($"Solicitud Anulada por: {funcionario.FullName} {obs}", funcionario.Id, id.SolicitudId);
                _servBitacora.Guardar(model);
                EmailMessage mail = _servEmail.ArmaMensaje(solicitud.Solicitante.Email, funcionario.UserName, "ANULA", solicitud);
                _servEmail.Send(mail);
                


                return Ok("OK");
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al ajustar CDP";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost("EliminarArchivo/{archivoId}")]
        public IActionResult EliminarArchivo(int archivoId, string fullpath)
        {
            if (archivoId == 0 && string.IsNullOrEmpty(fullpath))
                return NotFound();

            ArchivoModel archivo = _servArchivo.FindById(archivoId);
            string filePath = string.Empty;

            if (archivo == null)
                filePath = fullpath;
            else
                filePath = archivo.FullPath;



            using (WindowsLogin wl = new WindowsLogin())
            {
                System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                {
                    System.IO.File.Delete(filePath);
                });
            };

            if (archivoId > 0)
                _servArchivo.Delete(archivoId);

            return Ok();


        }

        [HttpGet("Solicitud/GetSector")]
        public async Task<IActionResult> GetSector()
        {
            try
            {
                List<SelectSectorModel> Sectlist = await _servSector.GetForSelect();

                return Ok(Sectlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Solicitud/GetSectorConPresupuesto")]
        public async Task<IActionResult> GetSectorConPresupuesto()
        {
            try
            {
                List<SelectSectorModel> Sectlist = await _servSector.GetForSelectConPresupuesto();

                return Ok(Sectlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Detalle/{solicitudId}")]
        public ActionResult Detalle(string solicitudId)
        {

            try
            {
                int id = Int32.Parse(solicitudId);
                ViewBag.Solicitud = _servSolicitud.FindById(id);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }




            return View();
        }




        [HttpGet("Ver/{solicitudId}")]
        public async Task<ActionResult> Ver(int solicitudId, string rutaOrigen)
        {

            try
            {
                ViewBag.rutaOrigen = rutaOrigen;
                ViewData["ANHO"] = _servPropSystem.FindByCodigo("ANHO").Valor.ToString();
                AprobacionConfigModel apobConfigModel = _servAprobacionConfig.FindById(1015);
                ViewData["MontoCDP"] = apobConfigModel.MontoUTMDesde;
                ViewData["hdnPuedeActualizar"] = 0;


                



                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
                int roleId = User.FindFirst(CustomClaims.RoleId).Value._toInt();

                SolicitudModel solicitudModel = _servSolicitud.FindById(solicitudId);
                
                
                ViewData["Extrapresupuestario"] = solicitudModel.ProgramaPresupuestario.SinCDP;
                //ViewData["hdnCertificadoSaldo"] = solicitudModel.ProgramaPresupuestario.ConCS;
                ViewBag.CertificadoSaldo = solicitudModel.ProgramaPresupuestario.ConCS;
                bool userEsPresupuesto = await _servUser.EsPresupuesto(userId);


                bool userEsAnalistaCompra = userId == solicitudModel.AnalistaProcesoId;
                EstadoModel estadoEntidad = _servEstado.FindById(solicitudModel.EstadoId);
                ViewBag.estaFinalizada = estadoEntidad.Id == Estados.Finalizada;
                ViewBag.estado = estadoEntidad.Nombre;


                bool ingresaCompromisoSIGFE = false;

                ingresaCompromisoSIGFE = solicitudModel.FaseCDP == "FASE 2" ? userEsAnalistaCompra : userEsPresupuesto;

                if (solicitudModel.EstadoId == Estados.Finalizada)
                {
                    ingresaCompromisoSIGFE = false;
                    //userEsPresupuesto = false;
                }

                if (solicitudModel.EstadoId < 10 && rutaOrigen == null)
                {
                    switch (roleId)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            ViewData["hdnPuedeActualizar"] = 1;
                            break;
                        default:
                            ViewData["hdnPuedeActualizar"] = 0;
                            // code block
                            break;
                    }
                }
                solicitudModel.MontoUTM = ActualizaMontoUtm(solicitudModel.TipoMonedaId, solicitudModel.MontoAprox._toDecimal());

                ViewBag.IngresaComprimisoSIGFE = ingresaCompromisoSIGFE;
                ViewBag.EsPresupuesto = userEsPresupuesto;
                TipoCompraModel tipoCompra = _servTipoCompra.FindById(solicitudModel.TipoCompraId);
                if(tipoCompra.Contrato)
                    ViewBag.tieneContrato = true;
                else
                    ViewBag.tieneContrato = false;


                if ((userEsAnalistaCompra || userEsPresupuesto) && estadoEntidad.PermiteGenerarOC)
                {
                    ViewBag.PageName = "Generar OC";
                    ViewBag.Accion = Acciones.GenerarOC;

                    ViewBag.OCSolicitudEntidad = new OCSolicitudModel
                    {
                        SolicitudId = solicitudModel.Id,
                        FechaOrdenCompra = solicitudModel.FechaOrdenCompra,
                        NombreProveedor = solicitudModel.ProveedorNombre,
                        NumOrdenCompra = solicitudModel.OrdenCompra,
                        RutProveedor = solicitudModel.ProveedorRut,
                        FechaInicioContrato = solicitudModel.FechaInicioContrato,
                        FechaFinContrato = solicitudModel.FechaFinContrato
                    };
                }
                else
                {
                    ViewBag.PageName = "Ver solicitud";
                    ViewBag.Accion = Acciones.Ver;

                    ViewBag.OCSolicitudEntidad = new OCSolicitudModel
                    {
                        SolicitudId = solicitudModel.Id,
                        FechaOrdenCompra = solicitudModel.FechaOrdenCompra,
                        NombreProveedor = solicitudModel.ProveedorNombre,
                        NumOrdenCompra = solicitudModel.OrdenCompra,
                        RutProveedor = solicitudModel.ProveedorRut,
                        FechaInicioContrato = solicitudModel.FechaInicioContrato,
                        FechaFinContrato = solicitudModel.FechaFinContrato
                    };

                }


                await DefaultValues(solicitudModel.Solicitante.Sector.SectorPadreId._toInt());



                return View(nameof(Index), solicitudModel);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }




            return View();
        }

        private decimal ActualizaMontoUtm(int tipoMonedaId, decimal montoAprox)
        {
            decimal valor = 0;
            TipoMonedaModel moneda = _servTipoMoneda.FindById(tipoMonedaId);
            TipoMonedaModel UTM = _servTipoMoneda.FindByCodigo("UTM");

            switch (moneda.Codigo)
            {
                case "UTM":
                    valor = montoAprox;
                    break;
                case "EUR":
                case "UF":
                case "USD":
                    valor = montoAprox * moneda.Valor / UTM.Valor;
                    break;
                case "CLP":
                    valor = (montoAprox / UTM.Valor);
                    break;


            }

            return valor;

        }

        [HttpGet("Editar/{solicitudId}")]
        public async Task<ActionResult> Editar(int solicitudId)
        {

            try
            {
                SolicitudModel solicitudModel = _servSolicitud.FindById(solicitudId);


                ViewBag.PageName = "Editar solicitud";
                ViewBag.Accion = Acciones.Editar;
                ViewBag.estado = solicitudModel.EstadoId.ToString();
                ViewData["ANHO"] = _servPropSystem.FindByCodigo("ANHO").Valor.ToString();
                ViewBag.CertificadoSaldo = solicitudModel.ProgramaPresupuestario.ConCS;

                await DefaultValues();


                return View(nameof(Index), solicitudModel);

            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }




            return View();
        }


        [HttpGet("Aprobacion/{solicitudId}")]
        public async Task<ActionResult> Aprobacion(string solicitudId)
        {

            try
            {
                AprobacionConfigModel apobConfigModel = _servAprobacionConfig.FindById(1015);
                ViewData["MontoCDP"] = apobConfigModel.MontoUTMDesde;
                ViewData["ANHO"] = _servPropSystem.FindByCodigo("ANHO").Valor.ToString();
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();


                int id = Int32.Parse(solicitudId);
                SolicitudModel solicitudModel = _servSolicitud.FindById(id);

                //ViewBag.NombreAprobacion = "Nombre aprobacion de prueba";

                solicitudModel.AprobacionActual.SolicitudId = solicitudId._toInt();
                solicitudModel.AprobacionActual.AprobacionConfigId = solicitudModel.AprobadorActualId._toInt();
                solicitudModel.MontoUTM = ActualizaMontoUtm(solicitudModel.TipoMonedaId, solicitudModel.MontoAprox._toDecimal());

                ViewBag.AprobacionConfig = _servAprobacionConfig.FindById(solicitudModel.AprobadorActualId._toInt());

                bool userEsPresupuesto = await _servUser.EsPresupuesto(userId);
                ViewBag.IngresaRequerimientoSIGFE = solicitudModel.EstadoId == Estados.ProcesoAprobacion ? userEsPresupuesto : false;


                ViewBag.PageName = "Aprobar solicitud";
                ViewBag.Accion = Acciones.Aprobar;
                ViewBag.CertificadoSaldo = solicitudModel.ProgramaPresupuestario.ConCS;

                await DefaultValues(solicitudModel.Solicitante.Sector.SectorPadreId._toInt());

                return View(nameof(Index), solicitudModel);

            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }

            return View();
        }

        [HttpGet("ProgramaPresupuestarioForSelect")]
        public async Task<IActionResult> ProgramaPresupuestarioForSelect()
        {
            try
            {
                List<ProgPresSelectModel> Conceptlist = await _servProgPres.GetForSelectAsync();

                return Ok(Conceptlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("GetProgramaPresupuestario/{sectorId}")]
        public async Task<IActionResult> GetProgramaPresupuestario(int sectorId)
        {
            try
            {
                List<ProgPresSelectModel> Conceptlist = await _servProgPres.GetForSelectBySectorAsync(sectorId);

                return Ok(Conceptlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("Solicitud/GetConceptoPresupuestario")]
        public async Task<IActionResult> GetConceptoPresupuestario()
        {
            try
            {
                List<SelectConceptoPresupuestarioModel> Conceptlist = await _servConceptoPresupuestario.GetForSelect();

                return Ok(Conceptlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("Solicitud/GetContraparteTecnica")]
        public async Task<IActionResult> GetContraparteTecnica()
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

        [HttpGet("Solicitud/GetTipoMoneda")]
        public async Task<IActionResult> GetTipoMoneda()
        {
            try
            {
                List<SelectTipoMonedaModel> Monedalist = await _servTipoMoneda.GetForSelect();

                return Ok(Monedalist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


