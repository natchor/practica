using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.ModalidadCompraModels;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio.Services
{
    public class ModalidadCompraService : IModalidadCompraService
    {

        private readonly IModalidadCompraRepository _repoModalidadCompra;
        private readonly IMapper _mapper;

        public ModalidadCompraService(IModalidadCompraRepository modalidadCompraRepository, IMapper mapper)
        {
            _repoModalidadCompra = modalidadCompraRepository;
            _mapper = mapper;
        }

        public ModalidadCompraModel FindById(int Id)
        {
            var compra = _repoModalidadCompra.Query().Where(e => e.Id == Id).FirstOrDefault();
            var compraModel = _mapper.Map<ModalidadCompraModel>(compra);

            return compraModel;

        }

        public List<ModalidadCompraSelectModel> GetSelect()
        {
            var lista = _repoModalidadCompra.Query().ToList();

            var listaModel = _mapper.Map<List<ModalidadCompraSelectModel>>(lista);

            return listaModel;
        }

        public int Guardar(ModalidadCompraModel ModalidadCompra)
        {
            int ret = 0;
            try
            {
                if (ModalidadCompra.Id == 0)
                {
                    ret = insertar(ModalidadCompra);
                }
                else
                {
                    ret = editar(ModalidadCompra);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(ModalidadCompraModel modalidadCompra)
        {
            var com = _repoModalidadCompra.Query().FirstOrDefault(e => e.Id == modalidadCompra.Id);

            _mapper.Map<ModalidadCompraModel, ModalidadCompra>(modalidadCompra, com);

            _repoModalidadCompra.Update(com);
            _repoModalidadCompra.SaveChanges();

            return modalidadCompra.Id;
        }

        private int insertar(ModalidadCompraModel ModalidadCompra)
        {
            var solic = _mapper.Map<ModalidadCompra>(ModalidadCompra);
            _repoModalidadCompra.Add(solic);
            _repoModalidadCompra.SaveChanges();

            return solic.Id;
        }
    }
}