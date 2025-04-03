using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.SectorModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class SectorService : ISectorService
    {

        private readonly ISectorRepository _repoSector;
        private readonly IMapper _mapper;

        public SectorService(ISectorRepository sectorRepository, IMapper mapper)
        {
            _repoSector = sectorRepository;
            _mapper = mapper;

        }

        public async Task<List<SelectSectorModel>> GetForSelect()
        {
            //Task.FromResult(result.ToList());
            var sect = await _repoSector.Query().ToListAsync();
            var sectModel = _mapper.Map<List<SelectSectorModel>>(sect);

            return sectModel;
        }

        public async Task<List<SelectSectorModel>> GetForSelectConPresupuesto()
        {
            //Task.FromResult(result.ToList());
            var sect = await _repoSector.Query().Where(e => e.TienePresupuesto == true).ToListAsync();
            var sectModel = _mapper.Map<List<SelectSectorModel>>(sect);

            return sectModel;
        }

        public async Task<List<SectorModel>> GetAllSector()
        {
            //Task.FromResult(result.ToList());
            var sect = await _repoSector.Query().ToListAsync();
            var sectModel = _mapper.Map<List<SectorModel>>(sect);

            return sectModel;
        }

        public SectorModel FindById(int Id)
        {

            var sector = _repoSector.Query().Where(e => e.Id == Id).FirstOrDefault();
            var sectorModel = _mapper.Map<SectorModel>(sector);

            return sectorModel;
        }



        public int Guardar(SectorModel Sector)
        {
            int ret = 0;
            try
            {
                if (Sector.Id == 0)
                {
                    ret = insertar(Sector);
                }
                else
                {
                    ret = editar(Sector);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(SectorModel Sector)
        {
            var sec = _repoSector.Query().FirstOrDefault(e => e.Id == Sector.Id);

            _mapper.Map<SectorModel, Sector>(Sector, sec);

            _repoSector.Update(sec);
            _repoSector.SaveChanges();

            return Sector.Id;
        }

        private int insertar(SectorModel Sector)
        {
            var solic = _mapper.Map<Sector>(Sector);
            _repoSector.Add(solic);
            _repoSector.SaveChanges();

            return solic.Id;
        }
    }
}