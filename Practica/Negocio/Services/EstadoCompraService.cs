using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.EstadoCompraModels;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;




namespace Negocio.Services
{
    public class EstadoCompraService : IEstadoCompraService
    {

        private readonly IEstadoCompraRepository _repoEstadoCompra;
        private readonly IMapper _mapper;

        public EstadoCompraService(IEstadoCompraRepository estadoCompraRepository,
                                  IMapper mapper)
        {
            _repoEstadoCompra = estadoCompraRepository;
            _mapper = mapper;
        }

        public EstadoCompraModel FindById(int Id)
        {
            var compra = _repoEstadoCompra.Query().Where(e => e.Id == Id).FirstOrDefault();
            var compraModel = _mapper.Map<EstadoCompraModel>(compra);

            return compraModel;

        }

   


        public int Guardar(EstadoCompraModel EstadoCompra)
        {
            int ret = 0;
            try
            {
                if (EstadoCompra.Id == 0)
                {
                    ret = insertar(EstadoCompra);
                }
                else
                {
                    ret = editar(EstadoCompra);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(EstadoCompraModel estadoCompra)
        {
            var com = _repoEstadoCompra.Query().FirstOrDefault(e => e.Id == estadoCompra.Id);

            _mapper.Map<EstadoCompraModel, EstadoCompra>(estadoCompra, com);

            _repoEstadoCompra.Update(com);
            _repoEstadoCompra.SaveChanges();

            return estadoCompra.Id;
        }

        Task<List<EstadoCompraModel>> IEstadoCompraService.GetAllEstadoCompra()
        {
            throw new NotImplementedException();
        }

        Task<List<EstadoCompraModel>> IEstadoCompraService.GetForSelect()
        {
            throw new NotImplementedException();
        }

        public async Task<List<EstadoCompraModel>> GetForSelectbyTpComp(int tipoCompra)
        {
            var estado = await _repoEstadoCompra.Query()
                .Where(u => u.TipoCompraId == tipoCompra)
                .Include(u => u.Estado)
                .ToListAsync();
            var estadoModel = _mapper.Map<List<EstadoCompraModel>>(estado);

            return estadoModel;
        }

        private int insertar(EstadoCompraModel EstadoCompra)
        {
            var solic = _mapper.Map<EstadoCompra>(EstadoCompra);
            _repoEstadoCompra.Add(solic);
            _repoEstadoCompra.SaveChanges();

            return solic.Id;
        }
    }
}
