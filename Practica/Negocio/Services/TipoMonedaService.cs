using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class TipoMonedaService : ITipoMonedaService
    {

        private readonly ITipoMonedaRepository _repoTipoMoneda;
        private readonly IMapper _mapper;

        public TipoMonedaService(ITipoMonedaRepository repoTipoMoneda, IMapper mapper)
        {
            _repoTipoMoneda = repoTipoMoneda;
            _mapper = mapper;
        }


        public TipoMonedaModel FindById(int Id)
        {
            //var encuesta = _repoDocumento.Query().OrderByDescending(a => a.Id).ToList();

            var mon = _repoTipoMoneda.Query().Where(e => e.Id == Id).FirstOrDefault();
            var MonModel = _mapper.Map<TipoMonedaModel>(mon);

            return MonModel;

        }

        public async Task<List<SelectTipoMonedaModel>> GetForSelect()
        {
            //Task.FromResult(result.ToList());
            var mon = await _repoTipoMoneda.Query().ToListAsync();
            var monModel = _mapper.Map<List<SelectTipoMonedaModel>>(mon);

            return monModel;
        }

        public async Task<List<TipoMonedaModel>> GetAll()
        {
            //Task.FromResult(result.ToList());
            var mon = await _repoTipoMoneda.Query().ToListAsync();
            var monModel = _mapper.Map<List<TipoMonedaModel>>(mon);

            return monModel;
        }




        public TipoMonedaModel FindByCodigo(string codigo)
        {
            var mon = _repoTipoMoneda.Query().Where(e => e.Codigo == codigo).FirstOrDefault();
            var MonModel = _mapper.Map<TipoMonedaModel>(mon);

            return MonModel;
        }

        public int Guardar(TipoMonedaModel TipoMoneda)
        {
            int ret = 0;

            try
            {
                if (TipoMoneda.Id == 0)
                {
                    ret = insertar(TipoMoneda);
                }
                else
                {
                    ret = editar(TipoMoneda);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(TipoMonedaModel tipoMoneda)
        {

            var mon = _repoTipoMoneda.Query().FirstOrDefault(e => e.Id == tipoMoneda.Id);

            _mapper.Map<TipoMonedaModel, TipoMoneda>(tipoMoneda, mon);

            _repoTipoMoneda.Update(mon);
            _repoTipoMoneda.SaveChanges();

            return 1;

        }

        private int insertar(TipoMonedaModel TipoMoneda)
        {
            var solic = _mapper.Map<TipoMoneda>(TipoMoneda);
            _repoTipoMoneda.Add(solic);
            _repoTipoMoneda.SaveChanges();

            return solic.Id;
        }

        object ITipoMonedaService.GetReporte()
        {
            throw new NotImplementedException();
        }
    }
}