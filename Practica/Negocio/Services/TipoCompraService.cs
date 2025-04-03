using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.TipoCompraModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class TipoCompraService : ITipoCompraService
    {

        private readonly ITipoCompraRepository _repoTipoCompra;
        private readonly IMapper _mapper;

        public TipoCompraService(ITipoCompraRepository tipoCompraRepository, IMapper mapper)
        {
            _repoTipoCompra = tipoCompraRepository;
            _mapper = mapper;

        }


        public TipoCompraModel FindById(int Id)
        {
            var compra = _repoTipoCompra.Query().Where(e => e.Id == Id).FirstOrDefault();
            var compraModel = _mapper.Map<TipoCompraModel>(compra);

            return compraModel;

        }


        public async Task<List<TipoCompraModel>> GetAllCompra()
        {
            //Task.FromResult(result.ToList());
            var comp = await _repoTipoCompra.Query().ToListAsync();
            var compraModel = _mapper.Map<List<TipoCompraModel>>(comp);

            return compraModel;
        }
        public List<TipoMonedaSelectModel> GetSelect()
        {
            var lista = _repoTipoCompra.Query().ToList();

            var listaModel = _mapper.Map<List<TipoMonedaSelectModel>>(lista);

            return listaModel;
        }

        public int Guardar(TipoCompraModel TipoCompra)
        {
            int ret = 0;
            try
            {
                if (TipoCompra.Id == 0)
                {
                    ret = insertar(TipoCompra);
                }
                else
                {
                    ret = editar(TipoCompra);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(TipoCompraModel tipoCompra)
        {
            var com = _repoTipoCompra.Query().FirstOrDefault(e => e.Id == tipoCompra.Id);

            _mapper.Map<TipoCompraModel, TipoCompra>(tipoCompra, com);

            _repoTipoCompra.Update(com);
            _repoTipoCompra.SaveChanges();

            return tipoCompra.Id;
        }

        private int insertar(TipoCompraModel TipoCompra)
        {
            var solic = _mapper.Map<TipoCompra>(TipoCompra);
            _repoTipoCompra.Add(solic);
            _repoTipoCompra.SaveChanges();

            return solic.Id;
        }
    }
}