using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.OrdenCompraModels;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio.Services
{
    public class OrdenCompraService : IOrdenCompraService
    {

        private readonly IOrdenCompraRepository _repoOrdenCompra;
        private readonly IMapper _mapper;

        public OrdenCompraService(IOrdenCompraRepository ordenCompraRepository,
                                  IMapper mapper)
        {
            _repoOrdenCompra = ordenCompraRepository;
            _mapper = mapper;
        }

        public OrdenCompraModel FindById(int Id)
        {
            var compra = _repoOrdenCompra.Query().Where(e => e.Id == Id).FirstOrDefault();
            var compraModel = _mapper.Map<OrdenCompraModel>(compra);

            return compraModel;

        }

        public OrdenCompraModel FindByOC(string oc)
        {
            var compra = _repoOrdenCompra.Query().Where(e => e.CodigoOC == oc).FirstOrDefault();
            var compraModel = _mapper.Map<OrdenCompraModel>(compra);

            return compraModel;

        }


        public int Guardar(OrdenCompraModel OrdenCompra)
        {
            int ret = 0;
            try
            {
                if (OrdenCompra.Id == 0)
                {
                    ret = insertar(OrdenCompra);
                }
                else
                {
                    ret = editar(OrdenCompra);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(OrdenCompraModel ordenCompra)
        {
            var com = _repoOrdenCompra.Query().FirstOrDefault(e => e.Id == ordenCompra.Id);

            _mapper.Map<OrdenCompraModel, OrdenCompra>(ordenCompra, com);

            _repoOrdenCompra.Update(com);
            _repoOrdenCompra.SaveChanges();

            return ordenCompra.Id;
        }

        private int insertar(OrdenCompraModel OrdenCompra)
        {
            var solic = _mapper.Map<OrdenCompra>(OrdenCompra);
            _repoOrdenCompra.Add(solic);
            _repoOrdenCompra.SaveChanges();

            return solic.Id;
        }
    }
}
