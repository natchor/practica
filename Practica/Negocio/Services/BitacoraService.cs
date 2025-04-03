using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.BitacoraModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Negocio.Services
{
    public class BitacoraService : IBitacoraService
    {

        private readonly IBitacoraRepository _repoBitacora;
        private readonly IMapper _mapper;

        public BitacoraService(IBitacoraRepository bitacoraRepository, IMapper mapper)
        {
            _repoBitacora = bitacoraRepository;
            _mapper = mapper;
        }

        public BitacoraModel FindById(int Id)
        {
            var bita = _repoBitacora.Query().Where(e => e.Id == Id).FirstOrDefault();
            var bitaModel = _mapper.Map<BitacoraModel>(bita);
            return bitaModel;
        }



        public async Task<List<BitacoraModel>> FindBySolicitudId(int solId)
        {

            var solicitud = await _repoBitacora.Query().Where(e => e.SolicitudId == solId && e.TipoBitacora==0)
                .Include(s => s.User)
                .ToListAsync();


            var aprobModel = _mapper.Map<List<BitacoraModel>>(solicitud);

            return aprobModel;
        }

        public async Task<List<BitacoraModel>> FindBitaEstadosBySolicitudId(int solId)
        {

            var solicitud = await _repoBitacora.Query().Where(e => e.SolicitudId == solId && e.TipoBitacora == 1)
                .Include(s => s.User)
                .ToListAsync();


            var aprobModel = _mapper.Map<List<BitacoraModel>>(solicitud);

            return aprobModel;
        }

        public void Guardar(BitacoraModel bitacora)
        {
            int ret = 0;
            try
            {
                if (bitacora.Id == 0)
                {
                    ret = insertar(bitacora);
                }
                else
                {
                    ret = editar(bitacora);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return ret;
        }

        private int editar(BitacoraModel bitacora)
        {
            var mon = _repoBitacora.Query().FirstOrDefault(e => e.Id == bitacora.Id);

            _mapper.Map<BitacoraModel, Bitacora>(bitacora, mon);

            _repoBitacora.Update(mon);
            _repoBitacora.SaveChanges();

            return bitacora.Id;
        }

        List<BitacoraModel> IBitacoraService.FindById(int Id)
        {
            throw new NotImplementedException();
        }

        //void IBitacoraService.Guardar(BitacoraModel bitacora)
        //{
        //    throw new NotImplementedException();
        //}

        private int insertar(BitacoraModel bitacora)
        {
            var solic = _mapper.Map<Bitacora>(bitacora);
            _repoBitacora.Add(solic);
            _repoBitacora.SaveChanges();

            return solic.Id;
        }
    }
}