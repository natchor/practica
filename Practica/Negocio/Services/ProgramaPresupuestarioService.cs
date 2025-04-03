using AutoMapper;
using Dato.Entities;
using Dato.Interfaces.Repositories;
using Dato.Respositories;
using Entidad.Interfaz.Models.ProgramaPresupuestarioModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class ProgramaPresupuestarioService : IProgramaPresupuestarioService
    {

        private readonly IProgramaPresupuestarioRepository _repoProgramaPresupuestario;
        private readonly ISectProgPreRepository _repoSectProgPre;
        private readonly IMapper _mapper;

        public ProgramaPresupuestarioService(IProgramaPresupuestarioRepository programaPresupuestarioRepository
            , ISectProgPreRepository sectProgPreRepository
            , IMapper mapper)
        {
            _repoProgramaPresupuestario = programaPresupuestarioRepository;
            _repoSectProgPre = sectProgPreRepository;
            _mapper = mapper;
        }

        public ProgramaPresupuestarioModel FindById(int Id)
        {
            var programa = _repoProgramaPresupuestario.Query().Where(e => e.Id == Id).FirstOrDefault();
            var programaModel = _mapper.Map<ProgramaPresupuestarioModel>(programa);
            return programaModel;
        }

        public async Task<ProgramaPresupuestarioModel> FindByIdAsync(int Id)
        {
            var programa = await _repoProgramaPresupuestario.Query().Where(e => e.Id == Id).FirstOrDefaultAsync();
            var programaModel = _mapper.Map<ProgramaPresupuestarioModel>(programa);
            return programaModel;
        }


        public async Task<List<ProgramaPresupuestarioModel>> GetAllPrograma()
        {
            //Task.FromResult(result.ToList());
            var prog = await _repoProgramaPresupuestario.Query().ToListAsync();
            var progModel = _mapper.Map<List<ProgramaPresupuestarioModel>>(prog);

            return progModel;
        }

        public List<ProgPresSelectModel> GetBySectorIdForSelect(int sectorId)
        {
            var lista = _repoSectProgPre.Query().Where(sp => sp.SectorId == sectorId && sp.ProgramaPresupuestario.Estado == true)
                .Include(spp => spp.ProgramaPresupuestario).Select(spp => spp.ProgramaPresupuestario).ToList();

            var listaModel = _mapper.Map<List<ProgPresSelectModel>>(lista);

            return listaModel;
        }

        public List<ProgPresSelectModel> GetForSelect()
        {
            var lista = _repoProgramaPresupuestario.Query().Where(e => e.Estado).ToList();

            var listaModel = _mapper.Map<List<ProgPresSelectModel>>(lista);

            return listaModel;
        }

        public async Task<List<ProgPresSelectModel>> GetForSelectAsync()
        {
            var lista = await _repoProgramaPresupuestario.Query().Where(e => e.Estado).ToListAsync();

            var listaModel = _mapper.Map<List<ProgPresSelectModel>>(lista);

            return listaModel;
        }

        public async Task<List<ProgPresSelectModel>> GetForSelectBySectorAsync(int sectId)
        {
            var lista = await _repoSectProgPre.Query().Where(spp => spp.SectorId == sectId && spp.ProgramaPresupuestario.Estado).Include(s => s.ProgramaPresupuestario).Select(s => s.ProgramaPresupuestario).ToListAsync();

            var listaModel = _mapper.Map<List<ProgPresSelectModel>>(lista);

            return listaModel;
        }

        public int Guardar(ProgramaPresupuestarioModel ProgramaPresupuestario)
        {
            int ret = 0;
            try
            {
                if (ProgramaPresupuestario.Id == 0)
                {
                    ret = insertar(ProgramaPresupuestario);
                }
                else
                {
                    ret = editar(ProgramaPresupuestario);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;

        }

        private int editar(ProgramaPresupuestarioModel programaPresupuestario)
        {
            var prog = _repoProgramaPresupuestario.Query().FirstOrDefault(e => e.Id == programaPresupuestario.Id);

            _mapper.Map<ProgramaPresupuestarioModel, ProgramaPresupuestario>(programaPresupuestario, prog);

            _repoProgramaPresupuestario.Update(prog);
            _repoProgramaPresupuestario.SaveChanges();

            return programaPresupuestario.Id;
        }

        private int insertar(ProgramaPresupuestarioModel ProgramaPresupuestario)
        {
            var solic = _mapper.Map<ProgramaPresupuestario>(ProgramaPresupuestario);
            _repoProgramaPresupuestario.Add(solic);
            _repoProgramaPresupuestario.SaveChanges();

            return solic.Id;
        }
    }
}