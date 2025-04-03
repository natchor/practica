using AutoMapper;
using Dato.Entities;
using Entidad.Interfaz;
using Entidad.Interfaz.Models.FeriadoChileModels;
using Entidad.Interfaz.Models.SolicitudDetalleModels;
using Entidad.Interfaz.Models.SolicitudModels;
using System;
using System.Collections.Generic;

namespace Negocio.Profiles
{


    public class SolicitudProfile : Profile
    {
        public SolicitudProfile()
        {
            CreateMap<SolicitudModel, Solicitud>()
                .ForMember(dest => dest.FaseCDP, opt => opt.MapFrom(x => (x.TipoMonedaId == Entidad.Interfaz.TipoMoneda.PesoChileno && x.ModalidadCompraId == Entidad.Interfaz.ModalidadCompra.Anual) ? "Fase 2".ToUpper() : "Previo Fase 1".ToUpper()))
                .ForMember(dest => dest.SolicitudDetalle, opt => opt.MapFrom(x => x.Detalle));

            CreateMap<OCSolicitudModel, Solicitud>()
                .ForMember(dest => dest.OrdenCompra, opt => opt.MapFrom(x => x.NumOrdenCompra))
                .ForMember(dest => dest.ProveedorRut, opt => opt.MapFrom(x => x.RutProveedor))
                .ForMember(dest => dest.ProveedorNombre, opt => opt.MapFrom(x => x.NombreProveedor))
                .ForMember(dest => dest.EstadoId, opt => opt.MapFrom(x => x.EstadoStr == "FINALIZADA" ? Estados.Finalizada : Estados.GenerandoOC));


            CreateMap<Solicitud, SolicitudModel>()
                .ForMember(dest => dest.Detalle, opt => opt.MapFrom(x => x.SolicitudDetalle));

            CreateMap<Solicitud, MisAprobacionesTablaModel>()
                .ForMember(dest => dest.NombreContraparteTecnica, opt => opt.MapFrom(x => $"{x.ContraparteTecnica.Nombre} {x.ContraparteTecnica.Apellido}"))
                .ForMember(dest => dest.NombreSolicitante, opt => opt.MapFrom(x => $"{x.Solicitante.Nombre} {x.Solicitante.Apellido}"))
                .ForMember(dest => dest.Semaforo, opt => opt.MapFrom(x => GetIntSemaforo(x)))
                .ForMember(dest => dest.FechaUltimaAproba, opt => opt.MapFrom(x => GetFechaUltimaAprob(x)))

                .ForMember(dest => dest.AprobacionPendiente, opt => opt.MapFrom(x => x.AprobadorActual.Nombre))
                .ForMember(dest => dest.UnidadDemandanteStr, opt => opt.MapFrom(x => x.UnidadDemandante.Nombre));


            CreateMap<Solicitud, MisSolicitudTablaModel>()
                .ForMember(dest => dest.ContraparteTecnica, opt => opt.MapFrom(x => x.ContraparteTecnica.Nombre))
                .ForMember(dest => dest.AprobadorActualStr, opt => opt.MapFrom(x => GetStringEstado(x)))
                .ForMember(dest => dest.ContraparteTecnica, opt => opt.MapFrom(x => $"{x.ContraparteTecnica.Nombre} {x.ContraparteTecnica.Apellido}"))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(x => x.Estado.Nombre));

            CreateMap<Solicitud, MisGestionesTablaModel>()
              .ForMember(dest => dest.TieneContrato, opt => opt.MapFrom(x => x.TipoCompra.Contrato))
              .ForMember(dest => dest.TipoMoneda, opt => opt.MapFrom(x => x.TipoMoneda.Nombre))
              .ForMember(dest => dest.TipoCompra, opt => opt.MapFrom(x => x.TipoCompra.Nombre));
              //.ForMember(dest => dest.ContraparteTecnica, opt => opt.MapFrom(x => $"{x.ContraparteTecnica.Nombre} {x.ContraparteTecnica.Apellido}"))
              //.ForMember(dest => dest.Estado, opt => opt.MapFrom(x => x.Estado.Nombre)); 


            CreateMap<Solicitud, GestionSolicitudesTablaModel>()
                .ForMember(dest => dest.AprobadorActualStr, opt => opt.MapFrom(x => GetStringEstado(x)))
                .ForMember(dest => dest.NombreSolicitante, opt => opt.MapFrom(x => $"{x.Solicitante.Nombre} {x.Solicitante.Apellido}"))
                .ForMember(dest => dest.EstadoStr, opt => opt.MapFrom(x => x.Estado.Nombre))
                .ForMember(dest => dest.AnalistaProcesoStr, opt => opt.MapFrom(x => x.AnalistaProceso == null ? "" : $"{x.AnalistaProceso.Nombre} {x.AnalistaProceso.Apellido}"))
                .ForMember(dest => dest.FuncionarioValidacionCDPStr, opt => opt.MapFrom(x => x.FuncionarioValidacionCDP == null ? "" : $"{x.FuncionarioValidacionCDP.Nombre} {x.FuncionarioValidacionCDP.Apellido}"))
                .ForMember(dest => dest.FuncionarioPresupuestoStr, opt => opt.MapFrom(x => x.AnalistaPresupuestoId == null ? "" : $"{x.AnalistaPresupuesto.Nombre} {x.AnalistaPresupuesto.Apellido}"));


            CreateMap<Solicitud, SolicitudPorFinalizarModel>()
                .ForMember(dest => dest.AprobadorActualStr, opt => opt.MapFrom(x => GetStringEstado(x)))
                .ForMember(dest => dest.NombreSolicitante, opt => opt.MapFrom(x => $"{x.Solicitante.Nombre} {x.Solicitante.Apellido}"))
                .ForMember(dest => dest.EstadoStr, opt => opt.MapFrom(x => x.Estado.Nombre))
                .ForMember(dest => dest.AprobadorCDPStr, opt => opt.MapFrom(x => x.FuncionarioValidacionCDP == null ? "" : $"{x.FuncionarioValidacionCDP.Nombre} {x.FuncionarioValidacionCDP.Apellido}"));


            CreateMap<SolicitudDetalleModel, SolicitudDetalle>().ReverseMap();

            CreateMap<SolicitudPresupuestoModel, Solicitud>().ReverseMap();

        }

        public static double GetDiasHabiles(DateTime startD, DateTime endD) 
        { 
            double calcDiasHabiles = 1 + ((endD - startD).TotalDays * 5 - (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7; 
            if ((int)endD.DayOfWeek == 6) calcDiasHabiles--; 
            if ((int)startD.DayOfWeek == 0) calcDiasHabiles--; 


            return calcDiasHabiles-1; 
        }

       


        private int GetIntSemaforo(Solicitud sol)
        {
            int ret = 0;
            int ret2 = 0;
            DateTime fechaCreacion =GetFechaUltimaAprob(sol);
            DateTime fecha = DateTime.Now;

            ret2 = (int)(fecha - fechaCreacion).TotalDays;
            //ret =(int)GetDiasHabiles(fechaCreacion, fecha);

            return ret;
        }

        private DateTime GetFechaUltimaAprob(Solicitud sol)
        {
            DateTime ultimaFecha = sol.FechaCreacion;
            try
            {

                foreach (Aprobacion aprob in sol.Aprobaciones)
                {
                    if (aprob.FechaAprobacion != null)
                        ultimaFecha = (DateTime)aprob.FechaAprobacion;
                }

                return ultimaFecha;
            }

            catch (Exception e)
            {
                return ultimaFecha;
            }

        }


            //int ret = 0;
            


        private string GetStringEstado(Solicitud sol)
        {
            string ret = string.Empty;

            switch (sol.EstadoId)
            {
                case Estados.Aprobada:
                case Estados.GenerandoOC:
                    ret = "Analista de Compra y Presupuesto";
                    break;
                case Estados.Finalizada:
                    ret = "";
                    break;
                case Estados.Creada:
                    ret = "Por asignar";
                    break;
                case Estados.Asignada:
                default:
                    ret = sol.AprobadorActual == null ? "Aprobador actual no encontrado." : sol.AprobadorActual.Nombre;

                    break;
            }


            return ret;
        }
   
    
    }


}
