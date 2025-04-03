using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.EstadoModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class EstadoService : IEstadoService
    {

        private readonly IEstadoRepository _repoEstado;
        private readonly IMapper _mapper;

        public EstadoService(IEstadoRepository estadoRepository, IMapper mapper)
        {
            _repoEstado = estadoRepository;
            _mapper = mapper;

        }

        public async Task<EstadoModel> FindByCodStr(string estado)
        {
            var est = await _repoEstado.Query().Where(s => s.CodigoStr == estado).FirstOrDefaultAsync();

            return _mapper.Map<EstadoModel>(est);

        }

        public EstadoModel FindById(int Id)
        {
            var estado = _repoEstado.Query().Where(e => e.Id == Id).FirstOrDefault();
            var estadoModel = _mapper.Map<EstadoModel>(estado);

            return estadoModel;

        }

        public async Task<List<EstadoModel>> GetAllEstado()
        {
            //Task.FromResult(result.ToList());
            var estado = await _repoEstado.Query().ToListAsync();
            var estadoModel = _mapper.Map<List<EstadoModel>>(estado);

            return estadoModel;
        }

        public async Task<List<EstadoSelectModel>> GetForSelect()
        {
            List<Estado> estados = await _repoEstado.Query().ToListAsync();

            var estadosModel = _mapper.Map<List<EstadoSelectModel>>(estados);

            return estadosModel;
        }

        public int Guardar(EstadoModel Estado)
        {
            int ret = 0;
            try
            {
                if (Estado.Id == 0)
                {
                    ret = insertar(Estado);
                }
                else
                {
                    ret = editar(Estado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(EstadoModel estado)
        {
            var mon = _repoEstado.Query().FirstOrDefault(e => e.Id == estado.Id);

            estado.PermiteGenerarOC = estado.Estado;
            _mapper.Map<EstadoModel, Estado>(estado, mon);

            _repoEstado.Update(mon);
            _repoEstado.SaveChanges();

            return estado.Id;
        }

        private int insertar(EstadoModel Estado)
        {
            var solic = _mapper.Map<Estado>(Estado);
            _repoEstado.Add(solic);
            _repoEstado.SaveChanges();

            return solic.Id;
        }
    }
}