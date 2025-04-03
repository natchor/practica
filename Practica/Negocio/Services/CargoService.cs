using AutoMapper;
using Dato.Entities;
using Dato.Respositories;
using Entidad.Interfaz.Models.CargoModels;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Services
{
    public class CargoService : ICargoService
    {

        private readonly ICargoRepository _repoCargo;
        private readonly IMapper _mapper;


        public CargoService(ICargoRepository cargoRepository, IMapper mapper)
        {
            _repoCargo = cargoRepository;
            _mapper = mapper;

        }


        public CargoModel FindById(int Id)
        {

            var cargo = _repoCargo.Query().Where(e => e.Id == Id).FirstOrDefault();
            var cargoModel = _mapper.Map<CargoModel>(cargo);

            return cargoModel;
        }



        public async Task<List<CargoModel>> GetAllCargo()
        {
            //Task.FromResult(result.ToList());
            var cargo = await _repoCargo.Query().ToListAsync();
            var cargoModel = _mapper.Map<List<CargoModel>>(cargo);

            return cargoModel;
        }

        public async Task<List<SelectCargoModel>> GetForSelect()
        {
            //Task.FromResult(result.ToList());
            var cargo = await _repoCargo.Query().ToListAsync();
            var cargoModel = _mapper.Map<List<SelectCargoModel>>(cargo);

            return cargoModel;
        }

        public int Guardar(CargoModel Cargo)
        {
            int ret = 0;
            try
            {
                if (Cargo.Id == 0)
                {
                    ret = insertar(Cargo);
                }
                else
                {
                    ret = editar(Cargo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        private int editar(CargoModel cargo)
        {
            var mon = _repoCargo.Query().FirstOrDefault(e => e.Id == cargo.Id);

            _mapper.Map<CargoModel, Cargo>(cargo, mon);

            _repoCargo.Update(mon);
            _repoCargo.SaveChanges();

            return cargo.Id;
        }

        private int insertar(CargoModel Cargo)
        {
            var solic = _mapper.Map<Cargo>(Cargo);
            _repoCargo.Add(solic);
            _repoCargo.SaveChanges();

            return solic.Id;
        }
    }
}