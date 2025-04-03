using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.PropertiesEmailModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class PropertiesEmailService : IPropertiesEmailService
    {

        private readonly IPropertiesEmailRepository _repoPropMail;
        private readonly IMapper _mapper;

        public PropertiesEmailService(IPropertiesEmailRepository repoPropMail, IMapper mapper)
        {
            _repoPropMail = repoPropMail;
            _mapper = mapper;
        }
        public PropertiesEmailModel FindById(int Id)
        {
            //var encuesta = _repoDocumento.Query().OrderByDescending(a => a.Id).ToList();

            var prop = _repoPropMail.Query().Where(e => e.Id == Id).FirstOrDefault();
            var propModel = _mapper.Map<PropertiesEmailModel>(prop);

            return propModel;

        }

        public async Task<List<PropertiesEmailModel>> GetAll()
        {
            //Task.FromResult(result.ToList());
            var mail = await _repoPropMail.Query().ToListAsync();
            var mailModel = _mapper.Map<List<PropertiesEmailModel>>(mail);

            return mailModel;
        }

        public PropertiesEmailModel FindByCodigo(string nombre)
        {
            var prop = _repoPropMail.Query().Where(e => e.Nombre == nombre).FirstOrDefault();
            var propModel = _mapper.Map<PropertiesEmailModel>(prop);

            return propModel;
        }

        public int Guardar(PropertiesEmailModel propertiesEmail)
        {
            int ret = 0;

            try
            {
                if (propertiesEmail.Id == 0)
                {
                    ret = insertar(propertiesEmail);
                }
                else
                {
                    ret = editar(propertiesEmail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(PropertiesEmailModel propertiesEmail)
        {

            var prop = _repoPropMail.Query().FirstOrDefault(e => e.Id == propertiesEmail.Id);

            _mapper.Map<PropertiesEmailModel, PropertiesEmail>(propertiesEmail, prop);

            _repoPropMail.Update(prop);
            _repoPropMail.SaveChanges();

            return 1;

        }

        private int insertar(PropertiesEmailModel propertiesEmail)
        {
            var prop = _mapper.Map<PropertiesEmail>(propertiesEmail);
            _repoPropMail.Add(prop);
            _repoPropMail.SaveChanges();

            return prop.Id;
        }

    }
}