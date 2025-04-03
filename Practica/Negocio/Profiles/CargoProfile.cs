using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.CargoModels;

namespace Negocio.Profiles
{


    public class CargoProfile : Profile
    {
        public CargoProfile()
        {
            CreateMap<CargoModel, Cargo>();
            CreateMap<Cargo, CargoModel>();
            CreateMap<Cargo, SelectCargoModel>();

        }
    }


}




