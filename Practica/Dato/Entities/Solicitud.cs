using System;
using System.Collections.Generic;

namespace Dato.Entities
{
    public class Solicitud
    {


        public int Id { get; set; }
        public string NroSolicitud { get; set; }
        //public int CorrelativoAnual { get; set; }

        public int? SolicitanteId { get; set; }

        public int AprobadorActualId { get; set; }

        public int UnidadDemandanteId { get; set; }
        public int ProgramaPresupuestarioId { get; set; }
        public string IniciativaVigenteId { get; set; }
        public String IniciativaVigente { get; set; }
        public int ConceptoPresupuestarioId { get; set; }
        public String FolioRequerimientoSIGFE { get; set; }
        public String FoliocompromisoSIGFE { get; set; }
        public int TipoCompraId { get; set; }
        public int TipoMonedaId { get; set; }
        public String NombreCompra { get; set; }
        public String ObjetivoJustificacion { get; set; }
        public decimal MontoAprox { get; set; }
        public decimal MontoUTM { get; set; }
        public decimal? MontoMultiAnual { get; set; }
        public decimal? MontoAnhoActual { get; set; }

        public int ModalidadCompraId { get; set; }
        //public int  Archivos { get; set; }
        public DateTime? FechaDerivacionAnalista { get; set; }
        public int? AnalistaProcesoId { get; set; }
        public int? FuncionarioValidacionCDPId { get; set; }
        public DateTime? FechaValidacionCDP { get; set; }
        public string CDPNum { get; set; }
        public Boolean ValidacionCDP { get; set; }
        public int? ContraparteTecnicaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String OrdenCompra { get; set; }
        public String ProveedorNombre { get; set; }
        public String ProveedorRut { get; set; }
        public DateTime? FechaOrdenCompra { get; set; }
        public int EstadoId { get; set; }
        public string FaseCDP { get; set; }
        public decimal MontoCLP { get; set; }
        public decimal ValorDivisa { get; set; }
        
        public int? AnalistaPresupuestoId { get; set; }
        public decimal ValorDivisaFinaliza { get; set; }
        public DateTime? FechaAjusteCDP { get; set; }
        public int? FuncionarioAjusteCDPId { get; set; }
        public int? FuncionarioCambioCDPId { get; set; }
        public string CodEstadoLicitacion { get; set; }


        public ConceptoPresupuestario ConceptoPresupuestario { get; set; }
        public ModalidadCompra ModalidadCompra { get; set; }
        public Estado Estado { get; set; }
        public TipoCompra TipoCompra { get; set; }
        public TipoMoneda TipoMoneda { get; set; }
        public String ObservacionGeneral { get; set; }
        public DateTime? FechaInicioContrato { get; set; }
        public DateTime? FechaFinContrato { get; set; }
        public Boolean Arrastre { get; set; }

        public ICollection<Archivo> Archivos { get; set; }
        public ICollection<Aprobacion> Aprobaciones { get; set; }
        public ICollection<SolicitudDetalle> SolicitudDetalle { get; set; }
        public ICollection<Convenio> Convenio { get; set; }


        public AprobacionConfig AprobadorActual { get; set; }
        public User Solicitante { get; set; }
        public User AnalistaProceso { get; set; }
        public User FuncionarioValidacionCDP { get; set; }
        public User AnalistaPresupuesto { get; set; }
        public User ContraparteTecnica { get; set; }
        public Sector UnidadDemandante { get; set; }
        public ProgramaPresupuestario ProgramaPresupuestario { get; set; }

    }
}
