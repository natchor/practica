using AutoMapper;
using Biblioteca.Librerias;
using Dato.Entities;
using Dato.Interfaces.Repositories;
using Dato.Respositories;
using Entidad.Interfaz.Models.AprobacionConfigModels;
using Entidad.Interfaz.Models.UserModels;
using Negocio.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class AprobacionConfigService : IAprobacionConfigService
    {
        private readonly IAprobacionConfigRepository _repoAprobacionConfig;
        private readonly IAprobacionRepository _repoAprobacion;
        private readonly IUserRepository _repoUser;
        private readonly IProgramaPresupuestarioRepository _repoProgPres;
        private readonly IMapper _mapper;

        public AprobacionConfigService(IAprobacionConfigRepository aprobacionConfigRepository, IAprobacionRepository aprobacionRepository, IMapper mapper, IUserRepository userRepository, IProgramaPresupuestarioRepository progPresRepository)
        {
            _repoAprobacionConfig = aprobacionConfigRepository;
            _repoAprobacion = aprobacionRepository;
            _repoUser = userRepository;
            _repoProgPres = progPresRepository;
            _mapper = mapper;
        }

        public AprobacionConfigModel FindById(int aprobacionConfigId)
        {
            AprobacionConfig acEntity = _repoAprobacionConfig.Query().Where(ac => ac.Id == aprobacionConfigId).FirstOrDefault();

            var ret = _mapper.Map<AprobacionConfigModel>(acEntity);


            return ret;
        }

        public List<AprobacionConfigModel> GetActivos()
        {
            List<AprobacionConfig> acList = _repoAprobacionConfig.Query().Where(cp => cp.EstaActivo)
                .OrderBy(cp => cp.Orden).ToList();

            var ret = _mapper.Map<List<AprobacionConfigModel>>(acList);


            return ret;
        }

        public int? GetAsignacionConfigId()
        {
            int asignaConfigId = _repoAprobacionConfig.Query().Where(ac => ac.RequiereAsignacion && ac.EstaActivo)
                .Select(ac => ac.Id).FirstOrDefault();

            return asignaConfigId;
        }

        public int? GetPrimerId()
        {
            List<AprobacionConfig> acList = _repoAprobacionConfig.Query().Where(ac => ac.EstaActivo && !ac.RequiereAsignacion)
               .OrderBy(cp => cp.Orden).ToList();

            return acList.FirstOrDefault().Id;
        }

        public List<UserModel> GetSiguienteAprobador(int solicitudId, int ConfigId)
        {
            List<Aprobacion> siguienteAprobador = _repoAprobacion.Query().Where(e => e.SolicitudId == solicitudId && e.AprobacionConfigId == ConfigId)
                .OrderBy(cp => cp.Orden).ToList();

            List<int> usuariosId = siguienteAprobador.Select(s => s.UserAprobadorId).ToList();
            List<UserModel> siguienteUserAprobador = new List<UserModel>();

            

            foreach (var item in usuariosId)
            {
                var userAprobacion = _repoUser.Query().Where(e => e.Id == item).FirstOrDefault();
                var userModel = _mapper.Map<UserModel>(userAprobacion);

                siguienteUserAprobador.Add(userModel);
            }




            return siguienteUserAprobador;
        }

        public async Task GetUsersAprobadoresAsync(Solicitud solic)
        {

            var progPres = _repoProgPres.Query().Where(pp => pp.SinCDP && pp.Id == solic.ProgramaPresupuestarioId).FirstOrDefault();

            var progPres2 = _repoProgPres.Query().Where(pp => pp.ConCS && pp.Id == solic.ProgramaPresupuestarioId).FirstOrDefault();


            List<AprobacionConfig> acList = _repoAprobacionConfig.Query().Where(ac => ac.EstaActivo && !ac.RequiereAsignacion && ac.EsParaTodoConceptoPre)
               .OrderBy(cp => cp.Orden ).ToList();

            if (progPres2 != null)
                acList = _repoAprobacionConfig.Query().Where(ac => ac.EstaActivo && !ac.RequiereAsignacion && ac.Id != 1).OrderBy(cp => cp.Orden).ToList();

            if (progPres != null && progPres2 ==null)
                acList = _repoAprobacionConfig.Query().Where(ac => ac.EstaActivo && !ac.RequiereAsignacion && ac.Id != 1 && ac.Id !=1015  && ac.EsParaTodoConceptoPre).OrderBy(cp => cp.Orden).ToList();



            foreach (var ac in acList)
            {

                


                List<int> usersIds = new List<int>();

                if (ac.MontoUTMDesde > solic.MontoUTM)
                    continue;

                if (ac.MontoUTMDesde._toDecimal() > 0 && ac.MontoUTMHasta._toDecimal() < 0)
                    ac.MontoUTMHasta = long.MaxValue;


                if (solic.MontoUTM.IsBetweenII(ac.MontoUTMDesde._toDecimal(), ac.MontoUTMHasta._toDecimal()))
                    usersIds = await _repoAprobacion.GetAprobadoresIds(ac, solic.SolicitanteId._toInt());


                if (ac.MontoUTMDesde._toDecimal() < 0 && ac.MontoUTMHasta._toDecimal() < 0)
                    usersIds = await _repoAprobacion.GetAprobadoresIds(ac, solic.SolicitanteId._toInt());



                if (ac.AConfigRequeridaId != null)
                {
                    var acReq = acList.FirstOrDefault(acL => acL.Id == ac.AConfigRequeridaId);

                    var usersReqIds = await _repoAprobacion.GetAprobadoresIds(acReq, solic.SolicitanteId._toInt());

                    insertarAprobadores(acReq, solic, usersReqIds);
                }
                
                    insertarAprobadores(ac, solic, usersIds);
                // Insertar Aprobadores

                

            }

            /* Se comenta hasta tener aprobacion de Martin 
            // solicitud es trato directo ?
            if (solic.TipoCompraId != Entidad.Interfaz.TipoCompra.TratoDirecto)
                return;


            // ya tiene gabinete dentro de las aprobaciones ?
            if (TieneGabinete(solic.Id))
                return;


            // si no tiene a gabinete debe ingresarse dentro de las aprobaciones
            var gabinete = acList.FirstOrDefault(a => a.Id == Entidad.Interfaz.AprobacionConfig.AprobacionGabinete);
            gabinete.Orden = 0;
            solic.AprobadorActualId = Entidad.Interfaz.AprobacionConfig.AprobacionGabinete;
            var usersGabinete = await _repoAprobacion.GetAprobadoresIds(gabinete, solic.SolicitanteId._toInt());
            insertarAprobadores(gabinete, solic, usersGabinete);
            */


        }

        private bool TieneGabinete(int solicitudId) 
        {
            bool result = _repoAprobacion.Query().Any(a => a.SolicitudId == solicitudId && a.AprobacionConfigId == Entidad.Interfaz.AprobacionConfig.AprobacionGabinete);

            return result;
        }

        private int insertarAprobadores(AprobacionConfig ac, Solicitud solicitud, List<int> usersIds)
        {

            foreach (var item in usersIds)
            {
                Aprobacion apro = new Aprobacion
                {
                    AprobacionConfigId = ac.Id,
                    SolicitudId = solicitud.Id,
                    UserAprobadorId = item,
                    FechaAprobacion = null,
                    Orden = ac.Orden
                };

                _repoAprobacion.Add(apro);
            }

            return _repoAprobacion.SaveChanges();
        }
    }
}
