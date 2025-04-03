using System;
using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz.Models.ArchivoModels;

namespace Negocio.Profiles
{


    public class ArchivoProfile : Profile
    {
        public ArchivoProfile()
        {
            CreateMap<ArchivoModel, Archivo>();
            CreateMap<Archivo, ArchivoModel>();

            CreateMap<Archivo, ArchivoTablaModel>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(x => $"{x.Usuario.Nombre} {x.Usuario.Apellido}"))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(x => x.FechaCreacion.Value.ToString("dd/MM/yyyy")));


        }
    }


}




