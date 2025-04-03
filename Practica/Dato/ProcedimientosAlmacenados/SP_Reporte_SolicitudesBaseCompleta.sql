USE [reqCompra]
GO

/****** Object:  StoredProcedure [dbo].[SP_Reporte_SolicitudesBaseCompleta]    Script Date: 20/8/2021 13:02:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--use reqCompra;
--ALTER DATABASE reqCompra SET COMPATIBILITY_LEVEL = 130
-- execute SP_Reporte_SolicitudesBaseCompleta '<Opciones><Opcion IdE="1"  Nombre="Cristian" Estado="muy bien"/></Opciones>'
CREATE PROCEDURE [dbo].[SP_Reporte_SolicitudesBaseCompleta]
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
ROW_NUMBER() OVER(PARTITION BY SolicitudID ORDER BY Anio ASC) AS rowNum
,Id	
,SolicitudId	
,MontoPresupuestado	
,Anio	
,MontoFinal	
,MontoMonedaSel

INTO #TempTable  FROM SolicitudDetalle solde 

 Select  

 *,
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

,Anio_T=(Select Anio from #TempTable where SolicitudId=s.Id and rowNum=1)
,Cantidad_anios=(Select count(id) from SolicitudDetalle where SolicitudId=s.Id)

,Total_Presupuestado_T=(Select MontoPresupuestado from #TempTable where SolicitudId=s.Id and rowNum=1)
,Total_Final_Pesos_T=(Select MontoFinal from #TempTable where SolicitudId=s.Id and rowNum=1)
,Total_Moneda_informada_T=(Select MontoMonedaSel from #TempTable where SolicitudId=s.Id and rowNum=1)


,Total_Presupuestado_PesosTN=(Select sum(MontoPresupuestado) from  #TempTable where SolicitudId=s.Id and rowNum>1)
,Total_Final_PesosTN=(Select sum(MontoFinal) from  #TempTable where SolicitudId=s.Id and rowNum>1)
,Total_Moneda_informadaTN=(Select sum(MontoMonedaSel) from  #TempTable where SolicitudId=s.Id and rowNum>1)

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
left join ConceptoPresupuestario con on  s.ConceptoPresupuestarioId = con.Id
left join ProgramaPresupuestario prog on  s.ProgramaPresupuestarioId = prog.Id
left join AprobacionConfig aprob on  s.ProgramaPresupuestarioId = aprob.Id







END TRY

BEGIN CATCH

	RAISERROR('Error al intentar cargar Reporte',16,1)

END CATCH
GO

