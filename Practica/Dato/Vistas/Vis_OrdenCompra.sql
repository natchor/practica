USE [reqCompra]
GO

/****** Object:  View [dbo].[vis_OrdenCompra]    Script Date: 20/8/2021 11:44:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--DROP VIEW vis_Usuario
CREATE VIEW [dbo].[vis_OrdenCompra]
AS SELECT
OC.Id,
ActividadProveedor,
Cantidad,
CantidadEvaluacion,
CargoContacto,
CargoContactoProveedor,
Cargos,
Categoria,
CodigoCategoria,
CodigoEstadoProveedor,
CodigoOC,
CodigoProducto,
CodigoSucursal,
CodigoTipo,
ComunaProveedor,
Descripcion,
Descuentos,
DireccionProveedor,
EspecificacionComprador,
EspecificacionProveedor,
EstadoProveedor,
FechaAceptacion,
FechaCancelacion,
FechaCreacionOC=OC.FechaCreacion,
FechaEnvio,
FechaUltimaModificacion,
Financiamiento,
FonoContacto,
FonoContactoProveedor,
FormaPago,
Impuestos,
ListadoCorrelativo,
MailContacto,
MailContactoProveedor,
Moneda,
Nombre,
NombreContacto,
NombreContactoProveedor,
NombreProveedor,
NombreSucursal,
PaisProveedor,
PorcentajeIva,
PrecioNeto,
Producto,
PromedioCalificacion,
ProveedorCodigo,
RegionProveedor,
RutSucursal,
TieneItems,
Tipo,
TipoMoneda,
Total,
Total2,
TotalCargos,
TotalDescuentos,
TotalImpuestos,
TotalNeto,
Unidad,
SolicitudId=S.Id,
NroSolicitud,
SolicitanteId,
AprobadorActualId,
UnidadDemandanteId,
ProgramaPresupuestarioId,
IniciativaVigenteId,
IniciativaVigente,
ConceptoPresupuestarioId,
FolioRequerimientoSIGFE,
FoliocompromisoSIGFE,
TipoCompraId,
TipoMonedaId,
NombreCompra,
ObjetivoJustificacion,
MontoAprox,
MontoUTM,
MontoMultiAnual,
MontoAnhoActual,
ModalidadCompraId,
FechaDerivacionAnalista,
AnalistaProcesoId,
FuncionarioValidacionCDPId,
FechaValidacionCDP,
CDPNum,
ValidacionCDP,
ContraparteTecnicaId,
FechaCreacionSolicitud=S.FechaCreacion,
OrdenCompra,
ProveedorNombre,
ProveedorRut,
FechaOrdenCompra,
EstadoId,
FaseCDP,
MontoCLP,
ValorDivisa,
AnalistaPresupuestoId,
ObservacionGeneral
--UserRole =(Select STUFF((SELECT ','+ Nombre FROM [Role] WHERE id in (select RoleId from UserRole where UserId=u.id) ORDER BY    Nombre FOR XML PATH('')),1,1,'')  )
FROM [OrdenCompra] OC LEFT JOIN Solicitud S  on  OC.CodigoOC=S.OrdenCompra



--select * from vis_Usuario
GO

