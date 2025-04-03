
using AutoMapper;
using Dato.Entities;
using Dato.Repositories;
using Dato.Respositories;
using Entidad.Interfaz.Models.FeriadoChileModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class FeriadoChileService : IFeriadoChileService
    {

        private readonly IFeriadoChileRepository _repoFeriadoChile;
        private readonly IMapper _mapper;

        public FeriadoChileService(IFeriadoChileRepository FeriadoChileRepository, IMapper mapper)
        {
            _repoFeriadoChile = FeriadoChileRepository;
            _mapper = mapper;

        }


        public FeriadoChileModel FindById(int Id)
        {
            var FeriadoChile = _repoFeriadoChile.Query().Where(e => e.Id == Id).FirstOrDefault();
            var FeriadoChileModel = _mapper.Map<FeriadoChileModel>(FeriadoChile);

            return FeriadoChileModel;

        }

        public List<FeriadoChileModel> GetAllFeriados()
        {
            //Task.FromResult(result.ToList());
            var FeriadoChile = _repoFeriadoChile.Query().Where(e => e.Region == 0 && e.Estado==1).ToList();
            var FeriadoChileModel = _mapper.Map<List<FeriadoChileModel>>(FeriadoChile);

            return FeriadoChileModel;
        }

        public int Guardar(FeriadoChileModel FeriadoChile)
        {
            int ret = 0;
            try
            {
                if (FeriadoChile.Id == 0)
                {
                    ret = insertar(FeriadoChile);
                }
                else
                {
                    ret = editar(FeriadoChile);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(FeriadoChileModel FeriadoChile)
        {
            var mon = _repoFeriadoChile.Query().FirstOrDefault(e => e.Id == FeriadoChile.Id);

            _mapper.Map<FeriadoChileModel, FeriadoChile>(FeriadoChile, mon);

            _repoFeriadoChile.Update(mon);
            _repoFeriadoChile.SaveChanges();

            return FeriadoChile.Id;
        }

        private int insertar(FeriadoChileModel FeriadoChile)
        {
            var solic = _mapper.Map<FeriadoChile>(FeriadoChile);
            _repoFeriadoChile.Add(solic);
            _repoFeriadoChile.SaveChanges();

            return solic.Id;
        }
    }
}
