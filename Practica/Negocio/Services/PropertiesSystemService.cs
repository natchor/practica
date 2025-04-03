using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
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
    public class PropertiesSystemService : IPropertiesSystemService
    {

        private readonly IPropertiesSystemRepository _repoPropSystem;
        private readonly IMapper _mapper;
        private readonly IStoredProcedureRepository _repProcedure;

        public PropertiesSystemService(IPropertiesSystemRepository repoPropSystem, IStoredProcedureRepository procedureRepo, IMapper mapper)
        {
            _repoPropSystem = repoPropSystem;
            _repProcedure = procedureRepo;
            _mapper = mapper;
        }
        public PropertiesSystemModel FindById(int Id)
        {
            //var encuesta = _repoDocumento.Query().OrderByDescending(a => a.Id).ToList();

            var prop = _repoPropSystem.Query().Where(e => e.Id == Id).FirstOrDefault();
            var propModel = _mapper.Map<PropertiesSystemModel>(prop);

            return propModel;

        }

        public async Task<List<PropertiesSystemModel>> GetAll()
        {
            //Task.FromResult(result.ToList());
            var mail = await _repoPropSystem.Query().ToListAsync();
            var mailModel = _mapper.Map<List<PropertiesSystemModel>>(mail);

            return mailModel;
        }

        public PropertiesSystemModel FindByCodigo(string codigo)
        {
            var prop = _repoPropSystem.Query().Where(e => e.Codigo == codigo).FirstOrDefault();
            var propModel = _mapper.Map<PropertiesSystemModel>(prop);

            return propModel;
        }

        public async Task<List<PropertiesSystemModel>> GetAllProperties()
        {
            //Task.FromResult(result.ToList());
            var prop = await _repoPropSystem.Query().ToListAsync();
            var propModel = _mapper.Map<List<PropertiesSystemModel>>(prop);

            return propModel;
        }

        public int Guardar(PropertiesSystemModel propertiesSystem)
        {
            int ret = 0;

            try
            {
                if (propertiesSystem.Id == 0)
                {
                    ret = insertar(propertiesSystem);
                }
                else
                {
                    ret = editar(propertiesSystem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(PropertiesSystemModel propertiesSystem)
        {
            

            var prop = _repoPropSystem.Query().FirstOrDefault(e => e.Id == propertiesSystem.Id);

            _mapper.Map<PropertiesSystemModel, PropertiesSystem>(propertiesSystem, prop);

            _repoPropSystem.Update(prop);
            _repoPropSystem.SaveChanges();
            if (propertiesSystem.Codigo=="ANHO") {
                DataTable dtable = new DataTable();

                dtable = _repProcedure.EjecutarProcedimientoAlmacenadoFull("SP_Reiniciar_Sequence", "");
            }
            return 1;

        }

        private int insertar(PropertiesSystemModel propertiesSystem)
        {
            var prop = _mapper.Map<PropertiesSystem>(propertiesSystem);
            _repoPropSystem.Add(prop);
            _repoPropSystem.SaveChanges();

            return prop.Id;
        }

    }
}