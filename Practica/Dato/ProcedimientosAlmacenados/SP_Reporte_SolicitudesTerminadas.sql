USE [reqCompra]
GO

/****** Object:  StoredProcedure [dbo].[SP_Reporte_SolicitudesTerminadas]    Script Date: 20/8/2021 11:47:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--use reqCompra;
--ALTER DATABASE reqCompra SET COMPATIBILITY_LEVEL = 130
-- execute SP_Reporte_SolicitudesTerminadas '<Opciones><Opcion IdE="1"  Nombre="Cristian" Estado="muy bien"/></Opciones>'
CREATE PROCEDURE [dbo].[SP_Reporte_SolicitudesTerminadas]
  @XML ntext
as
BEGIN TRY
 
 --DECLARE @Opciones TABLE(
 --id INT,
 --nombre VARCHAR(20),
 --estado VARCHAR(12));

declare @fecha datetime
set @fecha = getdate()

declare @TablaXML table
(
	Id smallint,
	Nombre varchar(50),
	Estado varchar(11)
)

declare @idoc int

exec sp_xml_preparedocument @idoc output, @XML


INSERT INTO @TablaXML
SELECT
	*
FROM
	OPENXML(@idoc, '/Opciones/Opcion', 2)
WITH
(
	Id smallint '@Id',
	Nombre varchar(50) '@Nombre',
	Estado varchar(11) '@Estado'
)

EXEC sp_xml_removedocument @idoc OUTPUT

 
 --select * from @TablaXML 

 Select  


 NroSolicitud
,s.FechaCreacion
,NombreCompra
,ObjetivoJustificacion
,Unidad_demandante = unide.Nombre
,UserSolicitante= solic.UserName
,MailSolicitante=solic.Email
,OrdenCompra
,ProveedorNombre
,ProveedorRut
,NombreSucursal
,RutSucursal
,Modalidad_Compra=tcomp.Nombre  
,AnualMultianual=modal.Nombre 
,Tipo_Moneda = mon.Nombre  
,MontoAprox
,MontoUTM
,Monto_OC=Total

,Estado=est.Nombre 
,FaseCDP
,ActividadProveedor
,CodigoProducto
,ComunaProveedor
,Descripcion
,EspecificacionComprador
,FechaAceptacion
,FechaCancelacion
,FechaEnvio
,FechaUltimaModificacion
,FonoContactoProveedor
,MailContactoProveedor
,NombreContactoProveedor
,CargoContactoProveedor
,UserCompras= solic.UserName
,MailCompras=solic.Email
,UserPresupuesto= presu.UserName
,MailPresupuesto=presu.Email
,Responzable_RC= ctecn.UserName
,MailResponzable_RC=ctecn.Email



from 

Solicitud s
left join OrdenCompra oc on s.OrdenCompra = oc.CodigoOC
left join [vis_usuario] solic on  s.SolicitanteId = solic.IdUsuario
left join [vis_usuario] compra on  s.AnalistaProcesoId = compra.IdUsuario
left join [vis_usuario] presu on  s.AnalistaPresupuestoId = presu.IdUsuario
left join [vis_usuario] ctecn on  s.ContraparteTecnicaId = ctecn.IdUsuario
left join TipoMoneda mon on  s.TipoMonedaId = mon.Id
left join TipoCompra tcomp on  s.TipoCompraId = tcomp.Id
left join ModalidadCompra modal on  s.ModalidadCompraId = modal.Id
left join Estado est on  s.EstadoId = est.Id
left join Sector unide on  s.UnidadDemandanteId = unide.Id



where EstadoId =10



END TRY

BEGIN CATCH

	RAISERROR('Error al intentar cargar Reporte',16,1)

END CATCH
GO

