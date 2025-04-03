
using Entidad.Interfaz.Models.ArchivoModels;
using Entidad.Interfaz.Models.ConceptoPresupuestarioModels;
using Entidad.Interfaz.Models.ConvenioModels;
using Entidad.Interfaz.Models.EstadoModels;
using Entidad.Interfaz.Models.ModalidadCompraModels;
using Entidad.Interfaz.Models.ProgramaPresupuestarioModels;
using Entidad.Interfaz.Models.SectorModels;
using Entidad.Interfaz.Models.SolicitudDetalleModels;
using Entidad.Interfaz.Models.TipoCompraModels;
using Entidad.Interfaz.Models.TipoMonedaModels;
using Entidad.Interfaz.Models.UnidadDemandanteModels;
using Entidad.Interfaz.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidad.Interfaz.Models.SolicitudModels
{
    public class SolicitudModel
    {
        

        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]

        public string NroSolicitud { get; set; }

        public int? AnalistaPresupuestoId { get; set; }
        public int SolicitanteId { get; set; }

        public int? AprobadorActualId { get; set; }
        public string AprobadorActualStr { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        public int UnidadDemandanteId { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public int ProgramaPresupuestarioId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        public string IniciativaVigente { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        public string IniciativaVigenteId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        public int ConceptoPresupuestarioId { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public String FolioRequerimientoSIGFE { get; set; }
        public String FoliocompromisoSIGFE { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public int TipoCompraId { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public int TipoMonedaId { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public String NombreCompra { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public String ObjetivoJustificacion { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public Decimal MontoAprox { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public decimal MontoUTM { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public Decimal? MontoMultiAnual { get; set; }

        public decimal? MontoAnhoActual { get; set; }
        public decimal? ValorDivisa { get; set; }
        public int ModalidadCompraId { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        //public int  Archivos { get; set; }
        public DateTime? FechaDerivacionAnalista { get; set; }
        public int? AnalistaProcesoId { get; set; }
        public int? FuncionarioValidacionCDPId { get; set; }
        public DateTime? FechaValidacionCDP { get; set; }
        public string CDPNum { get; set; }
        public Boolean ValidacionCDP { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public int ContraparteTecnicaId { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        public DateTime FechaCreacion { get; set; }
        public String OrdenCompra { get; set; }
        public String ProveedorNombre { get; set; }
        public String ProveedorRut { get; set; }
        //public string RutaArchivos { get; set; }
        public int EstadoId { get; set; }
        public DateTime? FechaOrdenCompra { get; set; }

        public string FaseCDP { get; set; }

        public String ObservacionGeneral { get; set; }

        public DateTime? FechaInicioContrato { get; set; }
        public DateTime? FechaFinContrato { get; set; }

        public decimal ValorDivisaFinaliza { get; set; }
        public DateTime? FechaAjusteCDP { get; set; }
        public int? FuncionarioAjusteCDPId { get; set; }
        public int? FuncionarioCambioCDPId { get; set; }

        public Boolean Arrastre { get; set; }
        public ICollection<ArchivoModel> Archivos { get; set; }
        public ICollection<SolicitudDetalleModel> Detalle { get; set; }
        public ICollection<ConvenioModel> Convenio { get; set; }

        public ConceptoPresupuestarioModel ConceptoPresupuestario { get; set; }
        public ModalidadCompraModel ModalidadCompra { get; set; }
        public EstadoModel Estado { get; set; }
        public TipoCompraModel TipoCompra { get; set; }
        public TipoMonedaModel TipoMoneda { get; set; }
        public UserModel Solicitante { get; set; }
        public UserModel AnalistaProceso { get; set; }
        public UserModel ContraparteTecnica { get; set; }
        public SectorModel UnidadDemandante { get; set; }
        public ProgramaPresupuestarioModel ProgramaPresupuestario { get; set; }

        public AprobacionModels.AprobacionModel AprobacionActual { get; set; }

    }
}
