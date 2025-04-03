using AutoMapper;
using Biblioteca.Librerias;
using Biblioteca.Seguridad;
//using ceTe.DynamicPDF;
//using ceTe.DynamicPDF.Conversion;
//using ceTe.DynamicPDF.HtmlConverter;
//using ceTe.DynamicPDF.Merger;
//using ceTe.DynamicPDF.PageElements;
using Dato.Entities;
using Dato.Interfaces.Repositories;
using Dato.Repositories;
using Dato.Respositories;
using DemoIntro.Models;
using Entidad.Interfaz;
using Entidad.Interfaz.Models.ArchivoModels;
using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.AprobacionModels;
using Entidad.Interfaz.Models.FeriadoChileModels;
using Entidad.Interfaz.Models.OrdenCompraModels;
using Entidad.Interfaz.Models.SolicitudDetalleModels;
using Entidad.Interfaz.Models.SolicitudModels;
using Entidad.Interfaz.Models.UserRoleModels;
using LogLevel = Biblioteca.Librerias.LogLevel;
//using GroupDocs.Watermark.Options.Pdf;
//using GroupDocs.Watermark;
//using GroupDocs.Watermark.Search;
//using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
//using System.Security.AccessControl;
//using System.Text;
using System.Threading.Tasks;
//using System.Net.Http;


namespace Negocio.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudRepository _repoSolicitud;
        private readonly IAprobacionConfigRepository _repoAprobacionConfig;
        private readonly ISectorRepository _repoSector;
        private readonly IRoleRepository _repoRole;
        private readonly IUserRepository _repoUser;
        private readonly IAprobacionRepository _repoAprobacion;
        private readonly IUserRoleRepository _repoUserRole;
        private readonly IConceptoPresupuestarioRepository _repoConceptoPres;
        private readonly IAprobacionConfigService _servAprobacionConfig;
        private readonly ISolicitudDetalleRepository _repoSolicitudDetalle;
        private readonly ITipoMonedaRepository _repoTipoMoneda;
        private readonly IFeriadoChileService _servFeriado;
        private readonly IEstadoRepository _repoEstado;
        private readonly IPropertiesSystemService _servProp;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private Dictionary<int, string> _dicSector;
        private Dictionary<int, string> _dicRole;
        private Dictionary<int, decimal> _dicTipoMoneda;



        private readonly IMapper _mapper;

        public static readonly LogEvent _log = new LogEvent();

        public SolicitudService(ISolicitudRepository repoSolicitud
            , IAprobacionConfigRepository aprobacionConfigRepository
            , ISectorRepository sectorRepository
            , IRoleRepository roleRepository
            , IUserRepository userRepository
            , IAprobacionRepository aprobacionRepository
            , IUserRoleRepository userRoleRepository
            , IConceptoPresupuestarioRepository conceptoPresupuestarioRepository
            , IAprobacionConfigService aprobacionConfigService
            , ISolicitudDetalleRepository solicitudDetalleRepository
            , ITipoMonedaRepository tipoMonedaRepository
            , IFeriadoChileService feriadoChileService
            , IEstadoRepository estadoRepository
            , IPropertiesSystemService servPropSystemRepository
            , IMapper mapper
            , IHttpContextAccessor httpContextAccessor)
        {
            _repoSolicitud = repoSolicitud;
            _repoAprobacionConfig = aprobacionConfigRepository;
            _repoSector = sectorRepository;
            _repoRole = roleRepository;
            _repoUser = userRepository;
            _repoAprobacion = aprobacionRepository;
            _repoUserRole = userRoleRepository;
            _repoConceptoPres = conceptoPresupuestarioRepository;
            _repoSolicitudDetalle = solicitudDetalleRepository;
            _repoEstado = estadoRepository;
            _servAprobacionConfig = aprobacionConfigService;
            _servProp = servPropSystemRepository;

            _repoTipoMoneda = tipoMonedaRepository;
            _servFeriado = feriadoChileService;
            //_dicSector = _repoSector.Query().ToDictionary(s => s.Id, s => s.Nombre);
            //_dicRole = _repoRole.Query().ToDictionary(r => r.Id, r => r.Nombre);
            _dicTipoMoneda = _repoTipoMoneda.Query().ToDictionary(r => r.Id, r => r.Valor._toDecimal());

            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

    public ArchivoModel GuardarPDF(int solicitudId, byte[] pdfData,string nombre )
        {
            string path = SiteKeys.FilesPath;
            ArchivoModel archivo = null;

            try
            { 
                using (WindowsLogin wl = new WindowsLogin())
                {
                    System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                    {
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        var newName = nombre;//$"{solicitudId}-CDP_{DateTime.Now.ToString("HHmmssffff")}.pdf";
                        var relativePath = Path.Combine(path, newName);

                        System.IO.File.WriteAllBytes(relativePath, pdfData);

                        archivo = new ArchivoModel
                        {
                            Ext = ".pdf",
                            FullPath = relativePath,
                            Nombre = newName,
                            Size = pdfData.Length,
                            UsuarioId = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaims.UserId).Value._toInt(),
                            FechaCreacion = DateTime.Now
                        };
                    });
                }
            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), $"Error al subir archivos", LogLevel.Error);
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);
            }
            return archivo; 
        }

 
        /*
        public byte[][] ObtenerPDFs(string folderPath, int solicitudId)
        {
            using (WindowsLogin wl = new WindowsLogin())
            {
                return System.Security.Principal.WindowsIdentity.RunImpersonated(wl.Identity.AccessToken, () =>
                {
                    if (Directory.Exists(folderPath))
                    {
                        string[] pdfFiles = Directory.GetFiles(folderPath, $"*-{solicitudId}.pdf");
                        List<byte[]> pdfBytesList = new List<byte[]>();

                        foreach (string pdfFile in pdfFiles)
                        {
                            byte[] pdfBytes = File.ReadAllBytes(pdfFile);
                            pdfBytesList.Add(pdfBytes);
                        }

                        return pdfBytesList.ToArray();
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("La carpeta especificada no existe.");
                    }
                });
            }
        }
        */
        public SolicitudModel FindById(int Id)
        {

            var sol = _repoSolicitud.Query().Where(e => e.Id == Id)
                .Include(s => s.Solicitante).ThenInclude(s => s.Sector)
                .Include(m => m.TipoMoneda)
                .Include(m => m.SolicitudDetalle)
                .Include(z => z.ProgramaPresupuestario)
                .Include(s => s.Convenio)
                .FirstOrDefault();



            var SolModel = _mapper.Map<SolicitudModel>(sol);
            SolModel.AprobacionActual = new AprobacionModel();

            var aprobacionConfig = _repoAprobacionConfig.Query().Where(ac => ac.Id == sol.AprobadorActualId).FirstOrDefault();

            SolModel.AprobacionActual.NombreAprobacionConfig = aprobacionConfig.Nombre;



            return SolModel;

        }
     
        public SolicitudModel FindSolicitudConCDP(int Id)
        {

            var sol = _repoSolicitud.Query().Where(e => e.Id == Id && e.ValidacionCDP && (e.EstadoId == 3 || e.EstadoId == 9 || e.EstadoId == 10) ) // && e.OrdenCompra != null se quita opcion  de finalizada
                .Include(s => s.Solicitante).ThenInclude(s => s.Sector)
                .Include(m => m.TipoMoneda)
                .Include(m => m.SolicitudDetalle)
                //.Include(s => s.Estado)
                .FirstOrDefault();

            var SolModel = _mapper.Map<SolicitudModel>(sol);
            SolModel.AprobacionActual = new AprobacionModel();

            var aprobacionConfig = _repoAprobacionConfig.Query().Where(ac => ac.Id == sol.AprobadorActualId).FirstOrDefault();
            SolModel.AprobacionActual.NombreAprobacionConfig = aprobacionConfig.Nombre;
            return SolModel;

        }

        public SolicitudModel FindTieneOCById(int Id)
        {

            var sol = _repoSolicitud.Query().Where(e => e.Id == Id  && e.OrdenCompra != null) // se quita opcion  de finalizada
                .Include(s => s.Solicitante).ThenInclude(s => s.Sector)
                .Include(m => m.TipoMoneda)
                .Include(m => m.SolicitudDetalle)
                //.Include(s => s.Estado)
                .FirstOrDefault();



            var SolModel = _mapper.Map<SolicitudModel>(sol);
            SolModel.AprobacionActual = new AprobacionModel();

            var aprobacionConfig = _repoAprobacionConfig.Query().Where(ac => ac.Id == sol.AprobadorActualId).FirstOrDefault();

            SolModel.AprobacionActual.NombreAprobacionConfig = aprobacionConfig.Nombre;



            return SolModel;

        }

        public async Task<SolicitudModel> FindByNumSolicitud(string Id)
        {

            var sol = await _repoSolicitud.Query().Where(e => e.NroSolicitud == Id)
                //.Include(s => s.Estado)
                .FirstOrDefaultAsync();

            var SolModel = _mapper.Map<SolicitudModel>(sol);

            return SolModel;

        }


        public int GuardarAprobacionEnMatriz(AprobacionModel aprob)
        {
            int ret = 0;
            var aprobLis = _repoAprobacion.Query().Where(e => e.Orden == aprob.Orden && e.SolicitudId == aprob.SolicitudId).ToList();

            //var sol = FindById(1);
            try
            {

                if (aprob.Id == 0)
                {
                    if (aprobLis.Count() > 0)
                        _repoAprobacion.UpdateAprobadoresIds(aprob.Orden, aprob.SolicitudId);

                    ret = insertarApro(aprob);
                }
                else
                {
                    if (aprobLis.Count() == 1)
                        if (aprobLis[0].Id != aprob.Id)
                            _repoAprobacion.UpdateAprobadoresIds(aprob.Orden, aprob.SolicitudId);
                        else if (aprobLis.Count() > 1)
                            _repoAprobacion.UpdateAprobadoresIds(aprob.Orden, aprob.SolicitudId);
                    ret = editarAprob(aprob);
                }
            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), $"{aprob.Id} - Error al guardar Aprobacion", LogLevel.Error);
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);

            }






            return ret;
        }

        private int editarAprob(AprobacionModel aprob)
        {
            var apro = _repoAprobacion.Query().FirstOrDefault(e => e.Id == aprob.Id);

            _mapper.Map<AprobacionModel, Aprobacion>(aprob, apro);

            _repoAprobacion.Update(apro);
            _repoAprobacion.SaveChanges();

            return apro.Id;

        }

        private int insertarApro(AprobacionModel aprob)
        {
            var apro = _mapper.Map<Aprobacion>(aprob);
            _repoAprobacion.Add(apro);
            _repoAprobacion.SaveChanges();

            return apro.Id;
        }

        public async Task<int> GuardarAsync(SolicitudModel solicitud)
        {
            int ret = 0;
            //var sol = FindById(1);
            try
            {
                if (solicitud.Id == 0)
                {
                    ret = await insertarAsync(solicitud);
                }
                else
                {
                    ret = editar(solicitud);
                }
            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), $"{solicitud.Id} - Error al guardar solicitud del solicitante: {solicitud.SolicitanteId}", LogLevel.Error);
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);

            }

            return ret;
        }

        private int editar(SolicitudModel solicitud)
        {
            var sol = _repoSolicitud.All().Where(e => e.Id == solicitud.Id).Include(s => s.SolicitudDetalle).FirstOrDefault();

            List<SolicitudDetalle> detalle = new List<SolicitudDetalle>(sol.SolicitudDetalle.ToList());
            bool montoAproxEsDiferente = sol.MontoAprox != solicitud.MontoAprox._toDecimal();

            _mapper.Map<SolicitudModel, Solicitud>(solicitud, sol);

            var resultantList = detalle.ToList().Where(item1 =>
                    sol.SolicitudDetalle.Any(item2 => item1.Anio == item2.Anio
                            && item1.MontoMonedaSel == item2.MontoMonedaSel)).ToList();

            if (resultantList.Count != detalle.Count || montoAproxEsDiferente)
            {
                sol.CDPNum = null;
                sol.ValidacionCDP = false;
                sol.FechaValidacionCDP = null;
                sol.FuncionarioValidacionCDPId = null;
            }

            _repoSolicitud.Update(sol);
            _repoSolicitud.SaveChanges();

            return sol.Id;

        }



        public async Task<List<MisAprobacionesTablaModel>> GetMisAprobaciones(int userId)
        {
            // Obtener las solicitudes que tengo que aprobar
            List<Aprobacion> aprobacionesUser = await _repoAprobacion.Query().Where(a => a.UserAprobadorId == userId && !a.EstaAprobado).Distinct().ToListAsync();
            List<int> solicitudesId = aprobacionesUser.Select(s => s.SolicitudId).ToList();

            List<int> aprobacionConfigIds = aprobacionesUser.Select(s => s.AprobacionConfigId).Distinct().ToList();

            //var solicitudes = await _repoSolicitud.Query().Where(s => solicitudesId.Any(sId => sId == s.Id) &&
            //aprobacionConfigIds.Any(aIds => aIds == s.AprobadorActualId) &&
            //(s.EstadoId != Estados.RechazadaEnAprobacion)
            //)
            //    .Include(s => s.ContraparteTecnica)
            //    .Include(s => s.Solicitante)
            //    .Include(s => s.AprobadorActual)
            //    .Include(s => s.UnidadDemandante)
            //    .Include(s => s.Aprobaciones)
            //    .ToListAsync();


            var solicitudes = await _repoSolicitud.Query().Where(s => solicitudesId.Any(sId => sId == s.Id) &&
            (s.EstadoId == Estados.ProcesoAprobacion || s.EstadoId == Estados.Asignada)
            )
                .Include(s => s.ContraparteTecnica)
                .Include(s => s.Solicitante)
                .Include(s => s.AprobadorActual)
                .Include(s => s.UnidadDemandante)
                .Include(s => s.Aprobaciones)
                .ToListAsync();

            var solicTablaModel = _mapper.Map<List<MisAprobacionesTablaModel>>(solicitudes);

            


                // solicTablaModel = Feriados(solicTablaModel);
            List<MisAprobacionesTablaModel> tablaRet = new List<MisAprobacionesTablaModel>();

            // Evaluar si tiene que aprobar una ahora
            foreach (var item in solicitudes)
            {
                Aprobacion aprobacion = aprobacionesUser.OrderBy(a => a.Orden).FirstOrDefault(a => a.SolicitudId == item.Id);

                MisAprobacionesTablaModel solicitudActual = solicTablaModel.FirstOrDefault(st => st.Id == item.Id);
                
                //bool tieneQueAprobar = item.AprobadorActual.Orden == aprobacion.Orden;
                bool tieneQueAprobar = item.AprobadorActualId == aprobacion.AprobacionConfigId;
                solicitudActual.Semaforo = Feriados(solicitudActual,item);


                if (tieneQueAprobar)
                {
                    solicitudActual.TieneQueAprobar = tieneQueAprobar;
                    tablaRet.Add(solicitudActual);
                }

            }

            return tablaRet;

        }

        private int Feriados(MisAprobacionesTablaModel solicTablaModel,Solicitud sol)
        {
            DateTime startD = GetFechaUltimaAprob(sol);
            DateTime endD = DateTime.Now;
            
            double calcDiasHabiles = 0 + ((endD - startD).TotalDays * 5 - (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;
            if ((int)endD.DayOfWeek == 6) calcDiasHabiles--;
            if ((int)startD.DayOfWeek == 0) calcDiasHabiles--;

            double calcDiasHabilesferiados = 0;
            List<FeriadoChileModel> feriadosM = _servFeriado.GetAllFeriados();
            foreach (FeriadoChileModel feriados in feriadosM)
            {
                DateTime bh = feriados.Fecha.Date;
                if (startD <= bh && bh <= endD)
                {
                    if ((int)bh.DayOfWeek == 6 || (int)bh.DayOfWeek == 0) { } 
                    else { ++calcDiasHabilesferiados; }
                }
                    
    
            }

            return (int)calcDiasHabiles - (int)calcDiasHabilesferiados;
        }

        public async Task<List<SolicitudPorFinalizarModel>> GetPorFinalizar(UserRoleModel user)
        {

            // Obtener las solicitudes por finalizar 
            if (user.RoleId == 2 || user.RoleId == 3)
            {// presupuesto
                var solicitudes = await _repoSolicitud.Query()
                     .Where(s => //(s.AnalistaPresupuestoId == user.UserId) && 
                     (s.EstadoId == Estados.GenerandoOC) && s.FaseCDP == "PREVIO FASE 1"
                 )
                     .Include(s => s.ContraparteTecnica)
                     .Include(s => s.Solicitante)
                     .Include(s => s.AprobadorActual)
                     .Include(s => s.AnalistaProceso)
                     .Include(s => s.FuncionarioValidacionCDP)
                     .Include(s => s.Estado)
                     .Include(s => s.AnalistaPresupuesto)
                     .ToListAsync();
                var solicTablaModel = _mapper.Map<List<SolicitudPorFinalizarModel>>(solicitudes);
                return solicTablaModel;
            }
            else if (user.RoleId == 4 || user.RoleId == 5)
            {//Compras
                var solicitudes = await _repoSolicitud.Query()
                    .Where(s => (s.AnalistaProcesoId == user.UserId) && 
                    (s.EstadoId == Estados.Aprobada || s.EstadoId == Estados.GenerandoOC)
                )
                    .Include(s => s.ContraparteTecnica)
                    .Include(s => s.Solicitante)
                    .Include(s => s.AprobadorActual)
                    .Include(s => s.AnalistaProceso)
                    .Include(s => s.FuncionarioValidacionCDP)
                    .Include(s => s.Estado)
                    .Include(s => s.AnalistaPresupuesto)
                    .ToListAsync();
                var solicTablaModel = _mapper.Map<List<SolicitudPorFinalizarModel>>(solicitudes);
                return solicTablaModel;
            }
            else { return null; }





            //List<MisAprobacionesTablaModel> tablaRet = new List<MisAprobacionesTablaModel>();




        }


        public async Task<List<SolicitudPorFinalizarModel>> GetPorAutorizar()
        {

            // Obtener las solicitudes por finalizar 
            if (1==1)
            {// presupuesto
                var solicitudes = await _repoSolicitud.Query()
                     .Where(s => //(s.AnalistaPresupuestoId == user.UserId) && 
                     (s.FuncionarioCambioCDPId != null) && s.FuncionarioAjusteCDPId == null
                 )
                     .Include(s => s.ContraparteTecnica)
                     .Include(s => s.Solicitante)
                     .Include(s => s.AprobadorActual)
                     .Include(s => s.AnalistaProceso)
                     .Include(s => s.FuncionarioValidacionCDP)
                     .Include(s => s.Estado)
                     .Include(s => s.AnalistaPresupuesto)
                     .ToListAsync();
                var solicTablaModel = _mapper.Map<List<SolicitudPorFinalizarModel>>(solicitudes);
                return solicTablaModel;
            }
            
          


        }

        public async Task<List<MisSolicitudTablaModel>> GetBySolicitanteId(int id)
        {
            // obtener las solicitudes
            var solicitud = await _repoSolicitud.Query().Where(e => e.SolicitanteId == id)
                .Include(s => s.ContraparteTecnica)
                .Include(s => s.Solicitante)
                .Include(s => s.AprobadorActual)
                .Include(s => s.Estado)
                .OrderByDescending(a => a.SolicitanteId == id).ToListAsync();

            var solicModel = _mapper.Map<List<MisSolicitudTablaModel>>(solicitud);

            return solicModel;
        }

        private async Task<int> insertarAsync(SolicitudModel solicitud)
        {
            var valorTipoMoneda = _dicTipoMoneda[solicitud.TipoMonedaId];

            var solic = _mapper.Map<Solicitud>(solicitud);

            solic.AprobadorActualId = _servAprobacionConfig.GetPrimerId()._toInt();
            solic.EstadoId = Estados.Creada;
            solic.ValorDivisa = valorTipoMoneda;

            int correlativoAnual = await _repoSolicitud.GetCorrelativoAnual();
            var anho= _servProp.FindByCodigo("ANHO").Valor.ToString();
            solic.NroSolicitud = $"{anho} - {correlativoAnual.ToString().PadLeft(5, '0')}";

            await _repoSolicitud.AddAsync(solic);
            _repoSolicitud.SaveChanges();


            //// Agregar usuario aprobadores 
            await _servAprobacionConfig.GetUsersAprobadoresAsync(solic);
            // TODO: Cambiar esta logica, los "Services"
            // se deben llamar en los controller, por lo que hay que dejar el metodo Guardar solo para esta accion
            // JCP - 06/04/2021

            return solic.Id;
        }

        public int Aprobar(AprobacionModel aprobacion)
        {
            //Aprobacion entidad = _mapper.Map<Aprobacion>(aprobacion);
            //Aprobacion entidad = _repoAprobacion.All().Where(a => a.UserAprobadorId == aprobacion.UserAprobadorId && a.SolicitudId == aprobacion.SolicitudId).FirstOrDefault();
            List<Aprobacion> aprobaciones = _repoAprobacion.All().Where(a => a.SolicitudId == aprobacion.SolicitudId).OrderBy(a => a.Orden).ToList();



            Solicitud solicitud = _repoSolicitud.All().Where(s => s.Id == aprobacion.SolicitudId)
                .Include(s => s.AprobadorActual)
                .FirstOrDefault();

            Dato.Entities.AprobacionConfig aprobacionConfigActual = _repoAprobacionConfig.Query().Where(ac => ac.Id == solicitud.AprobadorActualId).FirstOrDefault();

            Aprobacion entidadAprobacion = aprobaciones.Where(
                a => a.UserAprobadorId == aprobacion.UserAprobadorId &&
                     a.AprobacionConfigId == solicitud.AprobadorActualId
            ).FirstOrDefault();

            //Aprobacion entidadAprobacion = aprobaciones.Where(a => 
            //a.UserAprobadorId == aprobacion.UserAprobadorId && 
            //a.Orden == solicitud.AprobadorActual.Orden)
            //    .FirstOrDefault();

            //int ordenActual = aprobacionConfigActual.Orden;
            int ordenActual = entidadAprobacion.Orden;

            List<Aprobacion> aprobacionesFaltantes = aprobaciones.Where(a => a.Orden > ordenActual).ToList();


            if (aprobacionesFaltantes.Count == 0)
            {
                solicitud.EstadoId = Estados.Aprobada;
            }
            else
            {
                

                Aprobacion sgteAprobacion = aprobacionesFaltantes.FirstOrDefault();
                bool tieneCompras = aprobaciones.Any(a => a.AprobacionConfigId == Entidad.Interfaz.AprobacionConfig.AnalistaCompra);

                solicitud.AprobadorActualId = sgteAprobacion.AprobacionConfigId;

                if (!tieneCompras) {
                    solicitud.AprobadorActualId = Entidad.Interfaz.AprobacionConfig.AnalistaCompra;
                }

                if (aprobacionConfigActual.Id == 1018)
                {
                    solicitud.AprobadorActualId = 1015;
                }

                //solicitud.AprobadorActualId = aprobacionesFaltantes.FirstOrDefault().AprobacionConfigId;
                solicitud.EstadoId = Estados.ProcesoAprobacion;

                if (aprobacion.AnalistaPresupuestoId != null)
                {
                    solicitud.AnalistaPresupuestoId = aprobacion.AnalistaPresupuestoId;
                }
            }

            //insertarAprobadores(aprobacionConfig.FirstOrDefault(), solicitud);

            entidadAprobacion.FechaAprobacion = DateTime.Now;
            entidadAprobacion.EstaAprobado = true;
            entidadAprobacion.Observacion = aprobacion.Observacion;

            _repoAprobacion.Update(entidadAprobacion);
            _repoAprobacion.SaveChanges();

            _repoAprobacion.UpdateMatrizAprobacion((int)entidadAprobacion.AprobacionConfigId, entidadAprobacion.SolicitudId, entidadAprobacion.UserAprobadorId);


            return entidadAprobacion.SolicitudId;
        }

        public async Task<List<GestionSolicitudesTablaModel>> GetTblGestionSolicitudes(GSFiltros filtros)
        {
            List<Solicitud> solicitudes = await _repoSolicitud.Query()
                .Where(s => filtros.EstadoId == 0 ? true : s.EstadoId == filtros.EstadoId)
                .Where(s => filtros.TieneComSIGFE == null ? true : !string.IsNullOrEmpty(s.FoliocompromisoSIGFE) == filtros.TieneComSIGFE)
                .Where(s => filtros.TieneFechaEnvOC == null ? true : !string.IsNullOrEmpty(s.FechaOrdenCompra.ToString()) == filtros.TieneFechaEnvOC)
                .Include(s => s.AprobadorActual)
                .Include(s => s.FuncionarioValidacionCDP)
                .Include(s => s.AnalistaProceso)
                .Include(s => s.Solicitante)
                .Include(s => s.Estado)
                .Include(s => s.AnalistaPresupuesto)
                .ToListAsync();

            List<GestionSolicitudesTablaModel> ret = _mapper.Map<List<GestionSolicitudesTablaModel>>(solicitudes);

            return ret;

        }

        public async Task<List<AprobacionModel>> GetAprobacionesBySolicitudId(int solId)
        {
            var aprobadores = await _repoAprobacion.Query().Where(e => e.SolicitudId == solId)
               .Include(u => u.UserAprobador)
               .Include(s => s.AprobacionConfig)
               .OrderBy(a => a.Orden).ToListAsync();

            var solicitud = await _repoSolicitud.Query().Where(s => s.Id == solId).FirstOrDefaultAsync();

            

            var aprobModel = _mapper.Map<List<AprobacionModel>>(aprobadores);

            foreach (var itemAprb in aprobModel.OrderBy(a => a.Orden))
            {
                itemAprb.Actual = solicitud.AprobadorActualId == itemAprb.AprobacionConfigId;

                int indexAnt = aprobModel.IndexOf(itemAprb) - 1;
                AprobacionModel ant = indexAnt < 0 ? new AprobacionModel() : aprobModel[indexAnt];

                
                itemAprb.PuedeAprobar = !itemAprb.EstaAprobado && ant.EstaAprobado;
            }

            return aprobModel;
        }

        public async Task<List<SolicitudModel>> FindByUserId(int? id)
        {
            int? UserId = (id == 0) ? null : id;
            var solicitud = await _repoSolicitud.Query().Where(e => e.AnalistaProcesoId == UserId && e.EstadoId<10 )
                .Include(s => s.Solicitante)
              .ToListAsync();
            var solModel = _mapper.Map<List<SolicitudModel>>(solicitud);

            return solModel;
        }

        public AprobacionModel FindAprobacion(int id, int configId)
        {


            var aprob = _repoAprobacion.Query().Where(e => e.SolicitudId == id && e.AprobacionConfigId == configId)
                .FirstOrDefault();


            var SolModel = _mapper.Map<AprobacionModel>(aprob);



            return SolModel;
        }


        public async Task<int> RechazarSolicitud(AprobacionModel aprobacionEntity)
        {
            Aprobacion aprobacion = await _repoAprobacion.All().Where(a => a.SolicitudId == aprobacionEntity.SolicitudId && a.UserAprobadorId == aprobacionEntity.UserAprobadorId).FirstOrDefaultAsync();
            //List<Aprobacion> aprobaciones = await _repoAprobacion.All().Where(a => a.SolicitudId == aprobacionEntity.SolicitudId).ToListAsync();

            Solicitud solicitud = await _repoSolicitud.All().Where(s => s.Id == aprobacionEntity.SolicitudId).FirstOrDefaultAsync();

            //foreach (var itemAprobacion in aprobaciones)
            //{

            //    itemAprobacion.Observacion = null;
            //    itemAprobacion.FechaAprobacion = null;
            //    itemAprobacion.EstaAprobado = false;

            //    _repoAprobacion.Update(itemAprobacion);
            //}

            aprobacion.Observacion = aprobacionEntity.Observacion;
            aprobacion.FechaAprobacion = DateTime.Now;
            solicitud.EstadoId = Estados.RechazadaEnAprobacion;
            solicitud.ValidacionCDP = false;
            solicitud.FechaValidacionCDP = null;
            solicitud.FuncionarioValidacionCDPId = null;



            _repoAprobacion.Update(aprobacion);
            _repoAprobacion.InsertMatrizAprobacion(solicitud.Id);
            int ret = _repoAprobacion.SaveChanges();

            _repoAprobacion.UpdateAprobadoresActualiza(0, solicitud.Id);

            return ret;


        }

        public int GenerarOC(OCSolicitudModel oCSolicitud)
        {
            try
            {
                Solicitud solicitud = _repoSolicitud.Query().Where(s => s.Id == oCSolicitud.SolicitudId).FirstOrDefault();


                if (oCSolicitud.EstadoStr == "FINALIZADA")
                {
                    var detalles = _repoSolicitudDetalle.Query().Where(sd => sd.SolicitudId == oCSolicitud.SolicitudId).ToList();
                    //solicitud.ValorDivisaFinaliza = _servAprobacionConfig.;
                    foreach (var item in detalles)
                    {
                        item.MontoFinal = item.MontoMonedaSel * oCSolicitud.MontoDivisa;
                        //if (oCSolicitud.MontoOC != null)
                        //{

                        //    item.MontoFinal = (decimal)oCSolicitud.MontoOC ;
                        //    item.MontoMonedaSelFinal = (decimal)oCSolicitud.MontoOC;
                        //    if (item.MontoPresupuestado != (decimal)oCSolicitud.MontoOC)
                        //    {
                        //        item.EsAjuste = true;
                        //        solicitud.FuncionarioCambioCDPId = 0;

                        //    }


                        //}

                        _repoSolicitudDetalle.Update(item);
                    }
                }


                _mapper.Map<OCSolicitudModel, Solicitud>(oCSolicitud, solicitud);

                //solicitud.EstadoId = Estados.GenerandoOC;

                _repoSolicitud.Update(solicitud);
                _repoSolicitud.SaveChanges();

                return solicitud.Id;

            }
            catch (Exception ex)
            {
                _log.Registrar(this.GetType().ToString(), ex, LogLevel.Error);

                return 0;
            }

        }

 

        public async Task<int> AprobarCDP(SolicitudModel solicitud)
        {
            int number = await _repoAprobacion.GetObtenerNumCDP();
            solicitud.CDPNum = "CDP - C" + String.Format("{0:D4}", number);
            editar(solicitud);

            return solicitud.Id;
        }

        public AprobacionModel EliminarAprobacion(int id)
        {
            var apro = _repoAprobacion.Query().Include(a => a.UserAprobador).Include(a => a.AprobacionConfig).FirstOrDefault(e => e.Id == id);


            AprobacionModel aproModel = new AprobacionModel();
            _mapper.Map<Aprobacion, AprobacionModel>(apro, aproModel);

            _repoAprobacion.Remove(apro);
            _repoAprobacion.SaveChanges();

            return aproModel;
        }


        public async Task<List<SelectAprobacionConfigModel>> GetConfigForSelect()
        {

            var config = await _repoAprobacionConfig.Query().ToListAsync();
            var configModel = _mapper.Map<List<SelectAprobacionConfigModel>>(config);

            return configModel;
        }

        public async Task<List<SolicitudDetalleModel>> FindDetalleBySolicitudId(int solicitudId)
        {
            List<SolicitudDetalle> listSolicitudDetalle = await _repoSolicitudDetalle.Query().Where(sd => sd.SolicitudId == solicitudId).OrderBy(sd => sd.Anio).ToListAsync();
            decimal valorDivisa = await _repoSolicitud.Query().Where(s => s.Id == solicitudId).Select(s => s.ValorDivisa).FirstOrDefaultAsync();

            var modelo = _mapper.Map<List<SolicitudDetalleModel>>(listSolicitudDetalle);

            //se hace la conversion directamente al guardar en base de datos
            //foreach (var item in modelo)
            //{
            //    item.MontoPresupuestado = item.MontoPresupuestado / valorDivisa;
            //}

            return modelo;

        }

        public async Task<SolicitudModel> AsingarSolicitudAsync(SolicitudAnalistaCompraModel asignacion)
        {

            Solicitud sol = await _repoSolicitud.Query().Where(s => s.Id == asignacion.Id).FirstOrDefaultAsync();

            //sol.AnalistaProcesoId = asignacion.AnalistaProcesoId;
            //sol.FechaDerivacionAnalista = DateTime.Now;
            //sol.EstadoId = Estados.Asignada;//asignada

            sol.AnalistaProcesoId = asignacion.AnalistaProcesoId;
            sol.FechaDerivacionAnalista = asignacion.FechaDerivacion;
            sol.EstadoId = asignacion.EstadoId;

            int reqAsignacionConfigId = asignacion.AprobadorActualId._toInt();

            if (asignacion.AprobadorActualId == null)
                reqAsignacionConfigId = _servAprobacionConfig.GetAsignacionConfigId()._toInt();

            sol.AprobadorActualId = reqAsignacionConfigId;

            _repoSolicitud.Update(sol);
            int ret = _repoSolicitud.SaveChanges();

            var solicitudModel = _mapper.Map<SolicitudModel>(sol);

            return solicitudModel;

        }

        public string GetNroSolicitud(int solicitudId)
        {
            string ret = _repoSolicitud.Query()
                .Where(s => s.Id == solicitudId)
                .Select(s => s.NroSolicitud)
                .FirstOrDefault();

            return ret;
        }


        private DateTime GetFechaUltimaAprob(Solicitud sol)
        {
            DateTime ultimaFecha = sol.FechaCreacion;
            try
            {

                foreach (Aprobacion aprob in sol.Aprobaciones)
                {
                    if (aprob.FechaAprobacion != null)
                        ultimaFecha = (DateTime)aprob.FechaAprobacion;
                }

                return ultimaFecha;
            }

            catch (Exception e)
            {
                return ultimaFecha;
            }

        }


        public async Task<int> GuardarFoliosAsync(SolicitudPresupuestoModel sol)
        {
            var solicitud = await _repoSolicitud.Query().Where(s => s.Id == sol.Id).FirstOrDefaultAsync();

            _mapper.Map<SolicitudPresupuestoModel, Solicitud>(sol, solicitud);

            //solicitud.EstadoId = Estados.GenerandoOC;

            _repoSolicitud.Update(solicitud);
            _repoSolicitud.SaveChanges();

            return solicitud.Id;

        }

        public bool TieneAnalistaDeCompra(int solicitudId)
        {
            int? analistaId = _repoSolicitud.Query().Where(s => s.Id == solicitudId).Select(s => s.AnalistaProcesoId).FirstOrDefault();

            return analistaId != null;

        }


        public async Task<List<MisGestionesTablaModel>> GetTblMisGestiones(int userId)
        {
           

            var solicitudes = await _repoSolicitud.Query().Where(s => s.AnalistaProcesoId == userId)
                .Include(m => m.TipoMoneda)
                .Include(c => c.TipoCompra).ToListAsync();
            var estados = await _repoEstado.Query().ToListAsync();

            var solicTablaModel = _mapper.Map<List<MisGestionesTablaModel>>(solicitudes);

            solicTablaModel.ForEach(s => s.CodEstadoLicitacion = !string.IsNullOrEmpty(s.CodEstadoLicitacion) ? estados.Where(e => e.CodigoStr == s.CodEstadoLicitacion).FirstOrDefault().Nombre : "");
           

            return solicTablaModel;
        }

        public async Task<SolicitudModel> SetEstadoSolicitud(string estado, int solicitudid)
        {
            Solicitud solic = await _repoSolicitud.Query().Where(s => s.Id == solicitudid).FirstOrDefaultAsync();


            solic.CodEstadoLicitacion = estado;

            _repoSolicitud.Update(solic);
            _repoSolicitud.SaveChanges();

            return _mapper.Map<SolicitudModel>(solic);
        }

        public SolicitudDetalleModel FindDetalleAnioActualById(int solicitudId)
        {
            var detalles = _repoSolicitudDetalle.Query().Where(sd => sd.SolicitudId == solicitudId).OrderBy(sd => sd.Anio).FirstOrDefault();


            var DetModel = _mapper.Map<SolicitudDetalleModel>(detalles);
            

            return DetModel;


        }

        public async Task<int> SetAprobadorActual(int sigAprobadorId, int solicitudId)
        {
            var solicitud = await _repoSolicitud.All().Where(s => s.Id == solicitudId).FirstOrDefaultAsync();

            var aprobador = await _repoAprobacion.Query().Where(a => a.Id == sigAprobadorId).FirstOrDefaultAsync();

            solicitud.AprobadorActualId = aprobador.AprobacionConfigId;

            _repoSolicitud.Update(solicitud);
            _repoSolicitud.SaveChanges();

            return solicitudId;

        }

        public async Task<SolicitudModel> FindByIdAsync(int Id)
        {

            var sol =  _repoSolicitud.Query().Where(e => e.Id == Id)
                .Include(s => s.Solicitante).ThenInclude(s => s.Sector)
                .Include(m => m.SolicitudDetalle)
                .Include(a => a.Archivos)
                .Include(t => t.TipoCompra)
                .Include(u => u.UnidadDemandante)
                .Include(c => c.ConceptoPresupuestario)
                .Include(p => p.ProgramaPresupuestario)
                .Include(ct => ct.ContraparteTecnica)
                .FirstOrDefault();



            var SolModel = _mapper.Map<SolicitudModel>(sol);
 

            return SolModel;

        }
    }
}