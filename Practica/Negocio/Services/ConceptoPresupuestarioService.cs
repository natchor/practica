using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.ConceptoPresupuestarioModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class ConceptoPresupuestarioService : IConceptoPresupuestarioService
    {

        private readonly IConceptoPresupuestarioRepository _repoConceptoPresupuestario;
        private readonly IMapper _mapper;

        public ConceptoPresupuestarioService(IConceptoPresupuestarioRepository conceptoPresupuestarioRepository, IMapper mapper)
        {
            _repoConceptoPresupuestario = conceptoPresupuestarioRepository;
            _mapper = mapper;

        }

        public async Task<List<ConceptoPresupuestarioModel>> GetAllConcepto()
        {
            //Task.FromResult(result.ToList());
            var conc = await _repoConceptoPresupuestario.Query().ToListAsync();
            var concModel = _mapper.Map<List<ConceptoPresupuestarioModel>>(conc);

            return concModel;
        }

        public async Task<List<SelectConceptoPresupuestarioModel>> GetForSelect()
        {
            //Task.FromResult(result.ToList());
            var concepto = await _repoConceptoPresupuestario.Query().ToListAsync();
            var conceptoModel = _mapper.Map<List<SelectConceptoPresupuestarioModel>>(concepto);

            return conceptoModel;
        }




        public ConceptoPresupuestarioModel FindById(int Id)
        {
            var concepto = _repoConceptoPresupuestario.Query().Where(e => e.Id == Id).FirstOrDefault();
            var conceptoModel = _mapper.Map<ConceptoPresupuestarioModel>(concepto);

            return conceptoModel;

        }

        public int Guardar(ConceptoPresupuestarioModel ConceptoPresupuestario)
        {
            int ret = 0;
            try
            {
                if (ConceptoPresupuestario.Id == 0)
                {
                    ret = insertar(ConceptoPresupuestario);
                }
                else
                {
                    ret = editar(ConceptoPresupuestario);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(ConceptoPresupuestarioModel conceptoPresupuestario)
        {
            var mon = _repoConceptoPresupuestario.Query().FirstOrDefault(e => e.Id == conceptoPresupuestario.Id);

            _mapper.Map<ConceptoPresupuestarioModel, ConceptoPresupuestario>(conceptoPresupuestario, mon);

            _repoConceptoPresupuestario.Update(mon);
            _repoConceptoPresupuestario.SaveChanges();

            return conceptoPresupuestario.Id;
        }

        private int insertar(ConceptoPresupuestarioModel ConceptoPresupuestario)
        {
            var solic = _mapper.Map<ConceptoPresupuestario>(ConceptoPresupuestario);
            _repoConceptoPresupuestario.Add(solic);
            _repoConceptoPresupuestario.SaveChanges();

            return solic.Id;
        }
    }
}