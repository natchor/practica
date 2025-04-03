using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Entidad.Interfaz.Models.CargoModels;
using Entidad.Interfaz.Models.ConceptoPresupuestarioModels;
using Entidad.Interfaz.Models.EstadoModels;
using Entidad.Interfaz.Models.ProgramaPresupuestarioModels;
using Entidad.Interfaz.Models.RoleModels;
using Entidad.Interfaz.Models.SectorModels;
using Entidad.Interfaz.Models.SolicitudDetalleModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.TipoCompraModels;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Entidad.Interfaz.Models.UserModels;
using Microsoft.Extensions.Logging;
using Negocio.Interfaces.Services;
using System.Threading.Tasks;
using Web.Attributes.Filters;
using Web.Controllers;
using Entidad.Interfaz.Models.OrdenCompraModels;
using Biblioteca.Librerias;
using Entidad.Interfaz.Models.BitacoraModels;
using Biblioteca;
using Microsoft.AspNetCore.Http;
using Entidad.Interfaz.Models.UserRoleModels;
using Entidad.Interfaz.Models.AprobacionModels;
using Nancy.Json;

namespace Web.ReqCompra.Controllers
{

    [Route("[Controller]")]
    [Authorize(Role.Admin, Role.Presupuesto)]
    public class AjustarCDPController : BaseController
    {

        private readonly ILogger<HomeController> _logger;

        private readonly ICargoService _servCargo;
        private readonly IRoleService _servRol;
        private readonly IConceptoPresupuestarioService _servConcPres;
        private readonly IProgramaPresupuestarioService _servProgPres;
        private readonly ITipoCompraService _servTpCompra;
        private readonly ITipoMonedaService _servTpMoneda;
        private readonly ISectorService _servSector;
        private readonly IEstadoService _servEstado;
        private readonly IUserService _servUser;
        private readonly ISolicitudService _servSolicitud;
        private readonly IOrdenCompraService _servOrdenCompra;
        private readonly IBitacoraService _servBitacora;
        private readonly IEmailService _servEmail;
        private readonly IUserRoleService _servUserR;


        public AjustarCDPController(ILogger<HomeController> logger, ICargoService cargoService, IRoleService roleService, IConceptoPresupuestarioService conceptoService, IProgramaPresupuestarioService programaService, ITipoCompraService compraService, ITipoMonedaService monedaService, ISectorService sectorService, IEstadoService estadoService, ISolicitudService solicitudService, IUserService userService, IOrdenCompraService ordenCompraService, IBitacoraService bitaService, IEmailService emailService, IUserRoleService userRService)
        {
            _logger = logger;
            _servCargo = cargoService;
            _servRol = roleService;
            _servConcPres = conceptoService;
            _servProgPres = programaService;
            _servTpCompra = compraService;
            _servTpMoneda = monedaService;
            _servSector = sectorService;
            _servEstado = estadoService;
            _servSolicitud = solicitudService;
            _servUser = userService;
            _servUserR = userRService;
            _servOrdenCompra = ordenCompraService;
            _servEmail = emailService;
            _servBitacora = bitaService;

        }

        [HttpGet("AjustaCDP")]
        public IActionResult Ajusta()
        {
            //SolicitudModel solic = _servSolicitud.FindById(60);
            return View("AjustaCDP");
        }
        [HttpGet("AutorizaCDP")]
        public async Task<IActionResult> AutorizaAsync()
        {
            try
            {
                int userId = User.FindFirst(CustomClaims.UserId).Value._toInt();
                UserRoleModel model = _servUserR.FindByUserId(userId);

                ViewBag.tablaPorAutorizarCDP =  await _servSolicitud.GetPorAutorizar();

               
                return View("AutorizaCDP");
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

        [HttpPost("GetAjustaSolicitud")]
        public IActionResult AjustaSolicitud(SolicitudModel solicitud)
        {
            try
            {
                List<SolicitudDetalleModel> detalleList = (List<SolicitudDetalleModel>)solicitud.Detalle;
                SolicitudModel solicOrig = _servSolicitud.FindById(solicitud.Id);
                List<SolicitudDetalleModel> detalleListOrig = (List<SolicitudDetalleModel>)solicOrig.Detalle;
                UserModel funcionario = _servUser.GetByUserId((int)User.FindFirst(CustomClaims.UserId).Value._toInt());
                string valordivisa = "1";
                int cant = detalleList.Count;
                int cantOrig = detalleListOrig.Count;
                if (solicitud.FechaOrdenCompra != null)
                {

                    DateTime FechaOC = (DateTime)solicitud.FechaOrdenCompra;
                    string anho = FechaOC.Year.ToString();
                    string mes = FechaOC.Month.ToString();
                    string dia = FechaOC.Day.ToString();

                    //string moneda = solicitud.TipoMoneda.Codigo.ToUpper();
                    string url = solicitud.TipoMoneda.UrlCMF;

                    string fecha = @"/" + anho + "/" + mes + "/dias/" + dia + "?";
                    url = (url != null) ? url.Replace("?", fecha) : null;

                    if (solicitud.TipoMoneda.Codigo != "CLP")
                    {
                        valordivisa = ObtenerValorMonedadiaEspecifico(url, solicitud.TipoMoneda);
                    }
                }

                bool modifica = false;
                solicOrig.ValorDivisaFinaliza = Convert.ToDecimal(valordivisa);

                if (solicitud.MontoAprox != solicOrig.MontoAprox)
                {
                    solicOrig.FuncionarioCambioCDPId = funcionario.Id;//idUsuario
                    modifica = true;

                }
                bool arrastre = false;
                if (cant > cantOrig) { 
                    arrastre = true;
                    SolicitudDetalleModel detalleModel = detalleList[cant-1];
                    detalleListOrig.Add(detalleModel);
                }

                if (arrastre){
                    solicOrig.ModalidadCompraId = 2;
                    solicOrig.FaseCDP = "PREVIO FASE 1";
                    solicOrig.Arrastre = true;
                    BitacoraModel model = guardarBitacora($"Ajuste CDP con Arrastre, compra cambia a multianual y CDP a previo fase 1", funcionario.Id, solicitud.Id);
                    _servBitacora.Guardar(model);

                }
                for (int i = 0; i < cant; i++)
                {
                    if (detalleList[i].Anio == detalleListOrig[i].Anio && detalleList[i].MontoMonedaSelFinal != detalleListOrig[i].MontoMonedaSel)
                    {
                        detalleListOrig[i].EsAjuste = true;
                        detalleListOrig[i].MontoMonedaSelFinal = Convert.ToDecimal(detalleList[i].MontoMonedaSelFinal);
                        detalleListOrig[i].MontoFinal = Convert.ToDecimal(detalleList[i].MontoMonedaSelFinal) * solicitud.ValorDivisaFinaliza;
                        modifica = true;
                    }
                    else { 
                        detalleListOrig[i].MontoMonedaSelFinal = Convert.ToDecimal(detalleList[i].MontoMonedaSelFinal);
                        detalleListOrig[i].MontoFinal = Convert.ToDecimal(detalleList[i].MontoMonedaSelFinal) * solicitud.ValorDivisaFinaliza;
                    }


                }
                if (modifica)
                {
                    solicOrig.Detalle = detalleListOrig;
                    solicOrig.FechaAjusteCDP = null;
                    solicOrig.FuncionarioAjusteCDPId = null;
                    var res = _servSolicitud.GuardarAsync(solicOrig);
                    string obs = !string.IsNullOrEmpty(solicitud.ObservacionGeneral) ? $" con la siguiente observación: {solicitud.ObservacionGeneral}" : " sin observaciones";

                    BitacoraModel model = guardarBitacora($"Ajuste CDP hecho por: {funcionario.FullName} {obs}", funcionario.Id, solicitud.Id);
                    _servBitacora.Guardar(model);

                    EmailMessage mail = _servEmail.ArmaMensaje(funcionario.Email, funcionario.UserName, "AJUSTACDP", solicitud);
                    _servEmail.Send(mail);
                    
                    // ViewBag.tablaCargo = await _servCargo.GetAllCargo();
                    
                }
                return Ok("OK");
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al ajustar CDP";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        
        [HttpPost("urlRechazarAjusteCDP")]
        public IActionResult RechazarCDP(AprobacionModel id)
        {
            try
            {
                UserModel funcionario = _servUser.GetByUserId((int)User.FindFirst(CustomClaims.UserId).Value._toInt());
                DateTime fecha = DateTime.Now;
                SolicitudModel solicOrig = _servSolicitud.FindById(id.SolicitudId);
                List<SolicitudDetalleModel> detalleListOrig = (List<SolicitudDetalleModel>)solicOrig.Detalle;
                List<SolicitudDetalleModel> detalleListFinal =  new List<SolicitudDetalleModel>();
                //solicOrig.FechaAjusteCDP = fecha;
                solicOrig.FuncionarioCambioCDPId = null;

                foreach (SolicitudDetalleModel detalleList in detalleListOrig)
                {
                    detalleList.EsAjuste = false;
                    detalleListFinal.Add(detalleList);
                }

                solicOrig.Detalle=detalleListFinal;

                string obs = !string.IsNullOrEmpty(id.Observacion) ? $" con la siguiente observación: {id.Observacion}" : " sin observaciones";

                // ViewBag.tablaCargo = await _servCargo.GetAllCargo();
                var res = _servSolicitud.GuardarAsync(solicOrig);
                BitacoraModel model = guardarBitacora($"CDP rechazado por: {funcionario.FullName} {obs}", funcionario.Id, id.SolicitudId);
                _servBitacora.Guardar(model);
                EmailMessage mail = _servEmail.ArmaMensaje(funcionario.Email, funcionario.UserName, "AUTORIZACDP", solicOrig);
                _servEmail.Send(mail);
                return Ok("OK");
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al ajustar CDP";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }


        [HttpPost("urlAprobacionAjusteCDP")]
        public IActionResult AutorizaCDP(AprobacionModel id)
        {
            try
            {
                UserModel funcionario = _servUser.GetByUserId((int)User.FindFirst(CustomClaims.UserId).Value._toInt());
                DateTime fecha = DateTime.Now;
                SolicitudModel solicOrig = _servSolicitud.FindById(id.SolicitudId);
                solicOrig.FechaAjusteCDP = fecha;
                solicOrig.FuncionarioAjusteCDPId = funcionario.Id;


                string obs = !string.IsNullOrEmpty(id.Observacion) ? $" con la siguiente observación: {id.Observacion}" : " sin observaciones";
               
                // ViewBag.tablaCargo = await _servCargo.GetAllCargo();
                var res = _servSolicitud.GuardarAsync(solicOrig);
                BitacoraModel model = guardarBitacora($"CDP autorizado por: {funcionario.FullName} {obs}", funcionario.Id, id.SolicitudId);
                _servBitacora.Guardar(model);
                EmailMessage mail = _servEmail.ArmaMensaje(funcionario.Email, funcionario.UserName, "AUTORIZACDP", solicOrig);
                _servEmail.Send(mail);
                return Ok("OK");
            }
            catch (Exception)
            {
                ViewBag.StatusMessage = "Error al ajustar CDP";
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpGet("GetBuscaSolicitud")]
        public async Task<IActionResult> GetBuscaSolicitud(string id)
        {
            int solId = await formateaNumero(id);
            if (solId == 0)
            {
                string mensaje=("Solicitud no encontrada");
               
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                var TheJson = TheSerializer.Serialize(mensaje);
                return Ok(TheJson);
            }
                

            try
            {
                //SolicitudModel solic = _servSolicitud.FindTieneOCById(solId);
                SolicitudModel solic = _servSolicitud.FindSolicitudConCDP(solId);


                solic.TipoMoneda = _servTpMoneda.FindById(solic.TipoMonedaId);
            solic.TipoCompra = _servTpCompra.FindById(solic.TipoCompraId);
            solic.ConceptoPresupuestario = _servConcPres.FindById(solic.ConceptoPresupuestarioId);
            solic.ProgramaPresupuestario = _servProgPres.FindById(solic.ProgramaPresupuestarioId);
            solic.ContraparteTecnica = _servUser.GetByUserId(solic.ContraparteTecnicaId);
            solic.UnidadDemandante = _servSector.FindById(solic.UnidadDemandanteId);
            //solic.Detalle = _servSolicitud.get

            OrdenCompraModel Oc = _servOrdenCompra.FindByOC(solic.OrdenCompra);

            solic.MontoAnhoActual = (Oc!=null)? Oc.Total :0;

            // ViewBag.tablaCargo = await _servCargo.GetAllCargo();
            return Ok(solic);

            }
            catch (Exception)
            {
                string mensaje = "Solicitud no tiene OC por lo que no se puede Ajustar";
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                var TheJson = TheSerializer.Serialize(mensaje);
                return Ok(TheJson);
                //return Ok("");
            }
        }

    }
}
