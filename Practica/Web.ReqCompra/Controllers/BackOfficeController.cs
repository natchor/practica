using Entidad.Interfaz.Models.CargoModels;
using Entidad.Interfaz.Models.ConceptoPresupuestarioModels;
using Entidad.Interfaz.Models.EstadoModels;
using Entidad.Interfaz.Models.OrdenCompraModels;
using Entidad.Interfaz.Models.ProgramaPresupuestarioModels;
using Entidad.Interfaz.Models.RoleModels;
using Entidad.Interfaz.Models.SectorModels;
using Entidad.Interfaz.Models.SolicitudDetalleModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.TipoCompraModels;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Entidad.Interfaz.Models.PropertiesSystemModels;
using Entidad.Interfaz.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Attributes.Filters;
using Web.Controllers;
using Entidad.Interfaz.Models.ConvenioModels;
using System.Globalization;

namespace Web.ReqCompra.Controllers
{
    //[Route("[Controller]")] 
    
    public class BackOfficeController : Controller
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
        private readonly IPropertiesSystemService _servPropSystem;
        private readonly IConvenioService _servConvenio;



        public BackOfficeController(ILogger<HomeController> logger, ICargoService cargoService, IRoleService roleService, IConceptoPresupuestarioService conceptoService, IProgramaPresupuestarioService programaService, ITipoCompraService compraService, ITipoMonedaService monedaService, ISectorService sectorService, IEstadoService estadoService, ISolicitudService solicitudService, IUserService userService, IOrdenCompraService ordenCompraService,IPropertiesSystemService propSystemService, IConvenioService convenioService)
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
            _servOrdenCompra = ordenCompraService;
            _servPropSystem = propSystemService;
            _servConvenio = convenioService;

        }

        public IActionResult Index()
        {
            return View();
        }

        //mover a otro controlador 
        #region CDP  

        [HttpGet("AjustarCDP")]
        public IActionResult AjustarCDP()
        {
            // ViewBag.tablaCargo = await _servCargo.GetAllCargo();
            return View("../AjustarCDP/AjustarCDP");
        }

        [HttpGet("CDP/{solicitudId}")]
        public async Task<IActionResult> CDP(string solicitudId)
        {

            try
            {
                ViewData["CDP1"] =  _servPropSystem.FindByCodigo("CDP1").Valor.ToString();
                ViewData["CDP2"] =  _servPropSystem.FindByCodigo("CDP2").Valor.ToString();
                ViewData["CDPARRASTRE"] =  _servPropSystem.FindByCodigo("CDPARRASTRE").Valor.ToString();
                int id = Int32.Parse(solicitudId);
                //ViewBag.Solicitud = _servSolicitud.FindById(id);
                SolicitudModel solicitud = _servSolicitud.FindById(id);


                List<SolicitudDetalleModel> DetalleSolicitud = await _servSolicitud.FindDetalleBySolicitudId(id);

                ProgramaPresupuestarioModel prog = _servProgPres.FindById(solicitud.ProgramaPresupuestarioId);
                ConceptoPresupuestarioModel concep = _servConcPres.FindById(solicitud.ConceptoPresupuestarioId);
                SectorModel sector = _servSector.FindById((int)solicitud.UnidadDemandanteId);
                string anio = solicitud.FechaCreacion.Date.ToString("yyyy");
                if (solicitud.ValidacionCDP)
                {
                    UserModel usuCDP = _servUser.GetByUserId((int)solicitud.FuncionarioValidacionCDPId);
                    ViewData["cargoCDP"] = usuCDP.Cargo.Nombre;
                    ViewData["nombreCDP"] = usuCDP.FullName;
                }
                int index = 0;
                string anyos = "";
                bool esAjuste = false;
                decimal suma = 0;
                foreach (SolicitudDetalleModel item in DetalleSolicitud)
                {
                    suma += item.MontoMonedaSelFinal;
                    if (item.EsAjuste)
                        esAjuste = true;
                    if (index > 0)
                    {

                        anyos = anyos + item.Anio + ", ";
                    }
                    index++;


                }

                if (esAjuste) {
                    UserModel usuCCDP = _servUser.GetByUserId((int)solicitud.FuncionarioCambioCDPId);
                    @ViewData["cambiaCDP"] = (solicitud.FuncionarioCambioCDPId == 0) ? "Req-Compras" : usuCCDP.FullName;
                    OrdenCompraModel oC = _servOrdenCompra.FindByOC(solicitud.OrdenCompra);
                    @ViewData["MontoOC"] = (oC!=null)?oC.Total: suma;

                    if (solicitud.FuncionarioAjusteCDPId != null)
                    {
                        UserModel usuACDP = _servUser.GetByUserId((int)solicitud.FuncionarioAjusteCDPId);

                        @ViewData["nombreAjustaCDP"] = (solicitud.FuncionarioAjusteCDPId != null) ? usuACDP.FullName : "";
                        @ViewData["fechaAjusteCDP"] = solicitud.FechaAjusteCDP;
                        @ViewData["cargoAjusteCDP"] = (solicitud.FuncionarioAjusteCDPId != null) ? usuACDP.Cargo.Nombre : "";
                    }
                }

                ViewData["esAjuste"] = esAjuste;
                ViewData["divisa"] = solicitud.TipoMoneda.Codigo;
                if (anyos != "")
                {
                    ViewData["cant"] = anyos.Substring(0, anyos.Length - 2);
                }

                //const int pesoChileno = 5; // peso chileno
                //const int modalidad = 2; // multianual
                ViewData["fase"] = (solicitud.FaseCDP.IndexOf('1') != -1) ? solicitud.FaseCDP + " (*)" : solicitud.FaseCDP;

                ViewBag.tablaDetalleSolicitud = DetalleSolicitud;
                ViewData["anio"] = anio;
                ViewData["aniosig"] = int.Parse(anio) + 1;
                ViewData["programa"] = prog.Nombre;
                ViewData["programaId"] = prog.Id;
                ViewData["imputacion"] = concep.Nombre;
                ViewData["unidadDemandante"] = sector.Nombre;

                return View(nameof(CDP), solicitud);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }

            return View();
        }

        [HttpGet("CS/{solicitudId}")]
        public async Task<IActionResult> CS(string solicitudId)
        {

            try
            {


                

                int id = Int32.Parse(solicitudId);
                //ViewBag.Solicitud = _servSolicitud.FindById(id);
                SolicitudModel solicitud = _servSolicitud.FindById(id);

                
                ViewData["CDPARRASTRE"] = _servPropSystem.FindByCodigo("CDPARRASTRE").Valor.ToString();

                ConvenioModel convenio = _servConvenio.FindBySolicitudId(id);
                ViewData["Glosa1"] = (convenio.Antecedente != null) ? convenio.Antecedente : "";
                ViewData["Glosa2"] = "Faltan datos relacionados al Convenio";

                ViewData["CS"] = convenio.CertificadoSaldo ?? "";
                ViewData["Fecha"] = (convenio.FechaAutorizacionFin != null) ? convenio.FechaAutorizacionFin.ToString() : "";
                ViewData["FechaPresupuesto"] = (convenio.FechaAutorizacionPres != null) ? convenio.FechaAutorizacionPres.ToString() : "";
                ViewData["FechaFinanzas"] = (convenio.FechaAutorizacionFin != null) ? convenio.FechaAutorizacionFin.ToString() : "";


                ViewData["ApFinanzas"] = "Sin aprobación Finanzas";
                ViewData["ApPresupuesto"] = "Sin aprobación Presupuesto";
                ViewData["cargoFinanzas"] = "";
                ViewData["cargoPresupuesto"] = "";


                if (solicitud.ValidacionCDP)
                {
                    if (convenio.AutorizadorFinId != null) {
                        UserModel finanzas = _servUser.GetByUserId((int)convenio.AutorizadorFinId);
                        ViewData["ApFinanzas"] = finanzas.FullName ?? "";
                        ViewData["cargoFinanzas"] = finanzas.Cargo.Nombre;
                    }
                    
                    if( convenio.AutorizadorPresId!= null){
                        UserModel presupuesto = _servUser.GetByUserId((int)convenio.AutorizadorPresId);
                        ViewData["cargoPresupuesto"] = presupuesto.Cargo.Nombre;
                        ViewData["ApPresupuesto"] = presupuesto.FullName ?? "";
                    }

                   
                }

                 

                if (convenio.SaldoCuenta != null && convenio.Banco != null && convenio.CuentaCorriente != null ) 
                {
                    var stringNumber = convenio.SaldoCuenta;
                    int numericValue;
                    bool isNumber = int.TryParse(stringNumber, out numericValue);
                    string monto = (isNumber) ? numericValue.ToString("N", new CultureInfo("es-CL")).Split(',')[0] : convenio.SaldoCuenta;

                    ViewData["Glosa2"] = "El Departamento de Finanzas certifica que a la fecha  de emisión de este documento se cuenta con un saldo de $"+ monto + " en la cuenta N°: "+convenio.CuentaCorriente+" del Banco "+convenio.Banco+" perteneciente a este convenio.";
                }





                List<SolicitudDetalleModel> DetalleSolicitud = await _servSolicitud.FindDetalleBySolicitudId(id);

                ProgramaPresupuestarioModel prog = _servProgPres.FindById(solicitud.ProgramaPresupuestarioId);
                ConceptoPresupuestarioModel concep = _servConcPres.FindById(solicitud.ConceptoPresupuestarioId);
                SectorModel sector = _servSector.FindById((int)solicitud.UnidadDemandanteId);
                string anio = solicitud.FechaCreacion.Date.ToString("yyyy");

               

                int index = 0;
                string anyos = "";
                bool esAjuste = false;
                decimal suma = 0;
                foreach (SolicitudDetalleModel item in DetalleSolicitud)
                {
                    suma += item.MontoMonedaSelFinal;
                    if (item.EsAjuste)
                        esAjuste = true;
                    if (index > 0)
                    {

                        anyos = anyos + item.Anio + ", ";
                    }
                    index++;


                }

                if (esAjuste)
                {
                    UserModel usuCCDP = _servUser.GetByUserId((int)solicitud.FuncionarioCambioCDPId);
                    @ViewData["cambiaCDP"] = (solicitud.FuncionarioCambioCDPId == 0) ? "Req-Compras" : usuCCDP.FullName;
                    OrdenCompraModel oC = _servOrdenCompra.FindByOC(solicitud.OrdenCompra);
                    @ViewData["MontoOC"] = (oC != null) ? oC.Total : suma;

                    if (solicitud.FuncionarioAjusteCDPId != null)
                    {
                        UserModel usuACDP = _servUser.GetByUserId((int)solicitud.FuncionarioAjusteCDPId);

                        @ViewData["nombreAjustaCDP"] = (solicitud.FuncionarioAjusteCDPId != null) ? usuACDP.FullName : "";
                        @ViewData["fechaAjusteCDP"] = solicitud.FechaAjusteCDP;
                        @ViewData["cargoAjusteCDP"] = (solicitud.FuncionarioAjusteCDPId != null) ? usuACDP.Cargo.Nombre : "";
                    }
                }

                ViewData["esAjuste"] = esAjuste;
                ViewData["divisa"] = solicitud.TipoMoneda.Codigo;
                if (anyos != "")
                {
                    ViewData["cant"] = anyos.Substring(0, anyos.Length - 2);
                }

                //const int pesoChileno = 5; // peso chileno
                //const int modalidad = 2; // multianual
                ViewData["fase"] = (solicitud.FaseCDP.IndexOf('1') != -1) ? solicitud.FaseCDP + " (*)" : solicitud.FaseCDP;

                ViewBag.tablaDetalleSolicitud = DetalleSolicitud;
                ViewData["anio"] = anio;
                ViewData["aniosig"] = int.Parse(anio) + 1;
                ViewData["programa"] = prog.Nombre;
                ViewData["programaId"] = prog.Id;
                ViewData["imputacion"] = concep.Nombre;
                ViewData["unidadDemandante"] = sector.Nombre;

                return View(nameof(CS), solicitud);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }

            return View();
        }

        #endregion
        [Authorize(Role.Admin, Role.Presupuesto)]
        #region Proceso  
        [HttpGet("Anular")]
        public async Task<IActionResult> Anular()
        {
            // ViewBag.tablaCargo = await _servCargo.GetAllCargo();
            return View("Proceso/Proceso");
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

        [HttpGet("BuscaSolicitud/GetBuscaSolicitud")]
        public async Task<IActionResult> GetBuscaSolicitud(string id)
        {
            int solId = await formateaNumero(id);
            if (solId == 0)
                return BadRequest("Solicitud no encontrada");


            SolicitudModel solic = _servSolicitud.FindById(solId);
            EstadoModel estado = _servEstado.FindById(solic.EstadoId);
            solic.Estado = estado;
            // ViewBag.tablaCargo = await _servCargo.GetAllCargo();
            return Ok(solic);
        }



        #endregion
        #region Cargo  
        [HttpGet("Cargo")]
        public async Task<IActionResult> Cargo()
        {
            ViewBag.tablaCargo = await _servCargo.GetAllCargo();
            return View("Cargo/Cargo");
        }


        [HttpGet("Cargo/EditarCargo/{cargoId}")]
        public ActionResult EditarCargo(string cargoId)
        {
            string nombreVista = "Error";
            try
            {
                nombreVista = VistaMantenedor(cargoId, "Cargo");
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }
            return View(nombreVista);
        }

        #endregion
        #region Sector  
        [HttpGet("Sector")]
        public async Task<IActionResult> Sector()
        {
            ViewBag.tablaSector = await _servSector.GetAllSector();
            return View("Sector/Sector");
        }

        [HttpGet("Sector/EditarSector/{sectorId}")]
        public ActionResult EditarSector(string sectorId)
        {
            string nombreVista = "Error";
            nombreVista = VistaMantenedor(sectorId, "Sector");
            return View(nombreVista);
        }
        #endregion
        #region Concepto Presupuestario 
        
        [HttpGet("Concepto")]
        public async Task<IActionResult> Concepto()
        {

            ViewBag.tablaConcepto = await _servConcPres.GetAllConcepto();
            return View("Concepto/ConceptoPre");
        }
        
        [HttpGet("Concepto/EditarConcepto/{conceptoId}")]
        public ActionResult EditarConcepto(string conceptoId)
        {
            string nombreVista = "Error";
            nombreVista = VistaMantenedor(conceptoId, "Concepto");
            return View(nombreVista);
        }

        #endregion
        #region Programa Presupuestario  
        [HttpGet("Programa")]
        public async Task<IActionResult> Programa()
        {

            ViewBag.tablaPrograma = await _servProgPres.GetAllPrograma();
            return View("Programa/ProgramaPre");
        }
        [HttpGet("Programa/EditarPrograma/{programaId}")]
        public ActionResult EditarPrograma(string programaId)
        {
            string nombreVista = "Error";
            nombreVista = VistaMantenedor(programaId, "Programa");
            return View(nombreVista);
        }

        #endregion
        #region Tipo Compra  
        [HttpGet("Compra")]
        public async Task<IActionResult> Compra()
        {
            ViewBag.tablaCompra = await _servTpCompra.GetAllCompra();
            return View("Compra/TpCompra");
        }
        [HttpGet("Compra/EditarCompra/{compraId}")]
        public ActionResult EditarCompra(string compraId)
        {
            string nombreVista = "Error";
            nombreVista = VistaMantenedor(compraId, "Compra");
            return View(nombreVista);
        }
        #endregion
        #region Rol  
        [HttpGet("Rol")]
        public async Task<IActionResult> Rol()
        {
            ViewBag.tablaRol = await _servRol.GetAll();
            return View("Rol/Rol");
        }
        [HttpGet("Rol/EditarRol/{rolId}")]
        public ActionResult EditarRol(string rolId)
        {
            string nombreVista = "Error";
            try
            {
                nombreVista = VistaMantenedor(rolId, "Rol");
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{input}'");
            }
            return View(nombreVista);
        }


        #endregion
        #region Tipo Moneda 
        [HttpGet("Moneda")]
        public async Task<IActionResult> Moneda()
        {

            ViewBag.tablaMoneda = await _servTpMoneda.GetAll();
            return View("Moneda/TpMoneda");
        }

        [HttpGet("Moneda/EditarMoneda/{monedaId}")]
        public ActionResult EditarMoneda(string monedaId)
        {
            string nombreVista = "Error";
            nombreVista = VistaMantenedor(monedaId, "Moneda");
            return View(nombreVista);
        }

        #endregion
        #region Estado  
        [HttpGet("Estado")]
        public async Task<IActionResult> Estado()
        {
            ViewBag.tablaEstado = await _servEstado.GetAllEstado();
            return View("Estado/Estado");
        }

        [HttpGet("Estado/EditarEstado/{estadoId}")]
        public ActionResult EditarEstado(string estadoId)
        {
            string nombreVista = "Error";
            nombreVista = VistaMantenedor(estadoId, "Estado");
            return View(nombreVista);
        }
        #endregion
        #region Cargar General  
        [HttpPost("BackOffice/PostCargar")]
        public ActionResult PostCargar(string ids, string tipo)
        {
            try
            {
                int id = Int16.Parse(ids);
                Object resul = null;

                switch (tipo)
                {
                    case "Cargo":
                        resul = _servCargo.FindById(id);
                        break;
                    case "Rol":
                        resul = _servRol.FindById(id);
                        break;
                    case "Concepto":
                        resul = _servConcPres.FindById(id);
                        break;
                    case "Programa":
                        resul = _servProgPres.FindById(id);
                        break;
                    case "Compra":
                        resul = _servTpCompra.FindById(id);
                        break;
                    case "Moneda":
                        resul = _servTpMoneda.FindById(id);
                        break;
                    case "Sector":
                        resul = _servSector.FindById(id);
                        break;
                    case "Estado":
                        resul = _servEstado.FindById(id);
                        break;
                    case "System":
                        resul = _servPropSystem.FindById(id);
                        break;
                    default:
                        BadRequest("Mantenedor " + tipo + " aún no esta modelado");
                        break;
                }
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
        #region Cargar Vista General
        private string VistaMantenedor(string Id, string tipo)
        {
            int id = Int32.Parse(Id);
            ViewBag.Id = id;

            string result = tipo + "/Editar" + tipo;
            switch (tipo)
            {
                case "Concepto":
                case "Programa":
                    ViewBag.Titulo = (id != 0) ? "Editar " + tipo + " Presupuestario" : "Crear " + tipo + " Presupuestario";
                    break;
                case "Compra":
                case "Moneda":
                    ViewBag.Titulo = (id != 0) ? "Editar Tipo " + tipo : "Crear Tipo " + tipo;
                    break;
                default:
                    ViewBag.Titulo = (id != 0) ? "Editar " + tipo : "Crear " + tipo;
                    break;
            }

            return result;

        }

        #endregion
        #region Grabar Generar  



        //////Sector
        [HttpPost("BackOffice/PostGrabarSector")]
        public ActionResult PostGrabarSector(SectorModel model)
        {
            try
            {
                int resul = 0;
                resul = _servSector.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        ///////CARGO		
        [HttpPost("BackOffice/PostGrabarCargo")]
        public ActionResult PostGrabarCargo(CargoModel model)
        {
            try
            {
                int resul = 0;
                resul = _servCargo.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        ////////////ROL
        [HttpPost("BackOffice/PostGrabarRol")]
        public ActionResult PostGrabarRol(RoleModel model)
        {
            try
            {
                int resul = 0;
                resul = _servRol.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /////////Estado
        [HttpPost("BackOffice/PostGrabarEstado")]
        public ActionResult PostGrabarEstado(EstadoModel model)
        {
            try
            {
                int resul = 0;
                resul = _servEstado.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        ////////Concepto
        [HttpPost("BackOffice/PostGrabarConcepto")]
        public ActionResult PostGrabarConcepto(ConceptoPresupuestarioModel model)
        {
            try
            {
                int resul = 0;
                resul = _servConcPres.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /////////////Programa
        [HttpPost("BackOffice/PostGrabarPrograma")]
        public ActionResult PostGrabarPrograma(ProgramaPresupuestarioModel model)
        {
            try
            {
                int resul = 0;
                resul = _servProgPres.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /////////////Moneda
        [HttpPost("BackOffice/PostGrabarMoneda")]
        public ActionResult PostGrabarMoneda(TipoMonedaModel model)
        {
            try
            {
                int resul = 0;
                resul = _servTpMoneda.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /////////////Compra
        [HttpPost("BackOffice/PostGrabarCompra")]
        public ActionResult PostGrabarCompra(TipoCompraModel model)
        {
            try
            {
                int resul = 0;
                resul = _servTpCompra.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("BackOffice/PostGrabarSystem")]
        public ActionResult PostGrabarSystem(PropertiesSystemModel model)
        {
            try
            {
                int resul = 0;
                resul = _servPropSystem.Guardar(model);
                return Ok(resul);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        #endregion
        #region System  
        [HttpGet("System")]
        public async Task<IActionResult> System()
        {
            ViewBag.tablaSystem = await _servPropSystem.GetAllProperties();
            return View("System/System");
        }

        [HttpGet("System/EditarSystem/{systemId}")]
        public ActionResult EditarAnho(string systemId)
        {
            string nombreVista = "Error";
            nombreVista = VistaMantenedor(systemId, "System");
            return View(nombreVista);
        }
        #endregion

    }
}
