using AutoMapper;
using Dato.Entities;
using Dato.Interfaces.Repositories;
using Dato.Respositories;
using Entidad.Interfaz.Models.ConvenioModels;
using Entidad.Interfaz.Models.PropertiesSystemModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class ConvenioService : IConvenioService
    {

        private readonly IConvenioRepository _repoConvenio;
        private readonly IAprobacionRepository _repoAprobacion;
        private readonly IMapper _mapper;


        public ConvenioService(IConvenioRepository convenioRepository, IAprobacionRepository aprobacionRepository, IMapper mapper)
        {
            _repoConvenio = convenioRepository;
            _repoAprobacion = aprobacionRepository;
            _mapper = mapper;


        }


        public ConvenioModel FindBySolicitudId(int Id)
        {

            var con = _repoConvenio.Query().Where(e => e.SolicitudId == Id).FirstOrDefault();
            var convModel = _mapper.Map<ConvenioModel>(con);

            return convModel;

        }

        public async Task<List<ConvenioModel>> GetAllConvenios()
        {
            //Task.FromResult(result.ToList());
            var con = await _repoConvenio.Query().ToListAsync();
            var convModel = _mapper.Map<List<ConvenioModel>>(con);

            return convModel;
        }


        public async Task<List<PropertiesSystemModel>> GetAllProperties()
        {
            //Task.FromResult(result.ToList());
            var prop = await _repoConvenio.Query().ToListAsync();
            var propModel = _mapper.Map<List<PropertiesSystemModel>>(prop);

            return propModel;
        }

        public async Task<int> AprobarCS(ConvenioModel conv)
        {
            int number = await _repoAprobacion.GetObtenerNumCS();
            conv.CertificadoSaldo = "CS - C" + String.Format("{0:D4}", number);
            editar(conv);

            return conv.Id;
        }

        public int Guardar(ConvenioModel convenio)
        {
            int ret = 0;

            try
            {
                if (convenio.Id == 0)
                {
                    ret = insertar(convenio);
                }
                else
                {
                    ret = editar(convenio);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar( ConvenioModel convenio)
        {

            try
            {
                var conv = _repoConvenio.Query().FirstOrDefault(e => e.Id == convenio.Id);

                _mapper.Map<ConvenioModel, Convenio>(convenio, conv);

                _repoConvenio.Update(conv);
                _repoConvenio.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
          
            return 1;

        }

        private int insertar(ConvenioModel convenio)
        {
            var conv = _mapper.Map<Convenio>(convenio);
            _repoConvenio.Add(conv);
            _repoConvenio.SaveChanges();

            return conv.Id;
        }

     
    }
}