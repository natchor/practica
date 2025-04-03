using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.ArchivoModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio.Services
{
    public class ArchivoService : IArchivoService
    {

        private readonly IArchivoRepository _repoArchivo;
        private readonly IMapper _mapper;

        public ArchivoService(IArchivoRepository archivoRepository, IMapper mapper)
        {
            _repoArchivo = archivoRepository;
            _mapper = mapper;
        }

        public ArchivoModel FindById(int Id)
        {

            var archivo = _repoArchivo.Query().Where(e => e.Id == Id).FirstOrDefault();
            var archivoModel = _mapper.Map<ArchivoModel>(archivo);

            return archivoModel;
        }

        public List<ArchivoModel> FindBySolicitudId(int solicitudId)
        {
            List<Archivo> archivo = _repoArchivo.Query().Where(a => a.SolicitudId == solicitudId).ToList();

            List<ArchivoModel> archivoModel = _mapper.Map<List<ArchivoModel>>(archivo);

            return archivoModel;

        }

        
        public List<ArchivoModel> FindCSBySolicitudId(int solicitudId)
        {
            List<Archivo> archivos = _repoArchivo.Query()
                .Where(a => a.SolicitudId == solicitudId && a.Nombre.Contains("CS"))
                .OrderByDescending(a => a.FechaCreacion)
                .ToList();

            List<ArchivoModel> archivoModel = _mapper.Map<List<ArchivoModel>>(archivos);

            return archivoModel;
        }

        public List<ArchivoModel> FindCDPBySolicitudId(int solicitudId)
        {
            List<Archivo> archivos = _repoArchivo.Query()
                .Where(a => a.SolicitudId == solicitudId && a.Nombre.Contains("CDP"))
                .OrderByDescending(a => a.FechaCreacion)
                .ToList();

            List<ArchivoModel> archivoModel = _mapper.Map<List<ArchivoModel>>(archivos);

            return archivoModel;
        }
        public List<ArchivoTablaModel> GetForBitacora(int solicitudId)
        {
            List<Archivo> archivo = _repoArchivo.Query().Where(a => a.SolicitudId == solicitudId).Include(a => a.Usuario).ToList();



            List<ArchivoTablaModel> archivoModel = _mapper.Map<List<ArchivoTablaModel>>(archivo);

            return archivoModel;

        }


        public void Delete(int archivoId)
        {

            Archivo archivo = _repoArchivo.Query().Where(e => e.Id == archivoId).FirstOrDefault();

            _repoArchivo.Remove(archivo);
            _repoArchivo.SaveChanges();
        }

        public int Guardar(ArchivoModel archivo)
        {
            int ret = 0;
            try
            {
                if (archivo.Id == 0)
                {
                    ret = insertar(archivo);
                }
                else
                {
                    ret = editar(archivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        private int editar(ArchivoModel archivo)
        {
            var mon = _repoArchivo.Query().FirstOrDefault(e => e.Id == archivo.Id);

            _mapper.Map<ArchivoModel, Archivo>(archivo, mon);

            _repoArchivo.Update(mon);
            _repoArchivo.SaveChanges();

            return archivo.Id;
        }

        private int insertar(ArchivoModel archivo)
        {
            var solic = _mapper.Map<Archivo>(archivo);
            _repoArchivo.Add(solic);
            _repoArchivo.SaveChanges();

            return solic.Id;
        }
    }
}