USE [reqCompra]
GO
/****** Object:  StoredProcedure [dbo].[SP_Reporte_InformacionCDP]    Script Date: 25/8/2021 14:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--use reqCompra;
 --select * from solicitud where OrdenCompra='584105-334-CM21'
--ALTER DATABASE reqCompra SET COMPATIBILITY_LEVEL = 130
-- execute SP_Reporte_InformacionCDP '<Opciones><Opcion IdE="1"  Nombre="Cristian" Estado="muy bien"/></Opciones>'
ALTER PROCEDURE [dbo].[SP_Reporte_InformacionCDP]
  @XML ntext = null
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

 Select 
ROW_NUMBER() OVER(PARTITION BY SolicitudID ORDER BY Anio ASC) AS rowNum
,Id	
,SolicitudId	
,MontoPresupuestado	
,Anio	
,MontoFinal	
,MontoMonedaSel

INTO #TempTable  FROM SolicitudDetalle solde 
--select * from #TempTable




 Select  
 CDPNum
, NroSolicitud
,s.FechaCreacion
,NombreCompra
,Unidad_demandante=unide.Nombre
,Programa_presupuestario= pp.Nombre
,Concepto_presupuestario= cp.Nombre
,IniciativaVigenteId
,IniciativaVigente 
,FolioRequerimientoSIGFE 
,FoliocompromisoSIGFE


,Modalidad_Compra=tcomp.Nombre  
,AnualMultianual=modal.Nombre

,Tipo_Moneda = mon.Nombre  
,Estado =est.Nombre
--,Monto_presupuestado=MontoAprox
--,MontoUTM
,CodigoOC=CodigoOC
,FaseCDP
,Monto_Presupuestado=s.MontoAprox
,Monto_OC=Total
,Anio_T=(Select Anio from #TempTable where SolicitudId=s.Id and rowNum=1)
,Cantidad_anios=(Select count(id) from SolicitudDetalle where SolicitudId=s.Id)

,Total_Presupuestado_T=(Select MontoPresupuestado from #TempTable where SolicitudId=s.Id and rowNum=1)
,Total_Final_Pesos_T=(Select MontoFinal from #TempTable where SolicitudId=s.Id and rowNum=1)
,Total_Moneda_informada_T=(Select MontoMonedaSel from #TempTable where SolicitudId=s.Id and rowNum=1)


,Sumatoria_Pesos_Presup_años_dif_T=(Select sum(MontoPresupuestado) from  #TempTable where SolicitudId=s.Id and rowNum>1)
,Sumatoria_Pesos_Final_años_dif_T=(Select sum(MontoFinal) from  #TempTable where SolicitudId=s.Id and rowNum>1)
,Sumatoria_Moneda_Informada_años_dif_T=(Select sum(MontoMonedaSel) from  #TempTable where SolicitudId=s.Id and rowNum>1)

--Select * from SolicitudDetalle
--,MontosAnuales= (Select STRING_AGG(CONVERT(NVARCHAR(max), ISNULL(MontoPresupuestado,'0')), ',')  from SolicitudDetalle where SolicitudId=s.Id)


from 

Solicitud s
left join OrdenCompra oc on s.OrdenCompra = oc.CodigoOC
left join [vis_usuario] solic on  s.SolicitanteId = solic.IdUsuario
left join [vis_usuario] compra on  s.AnalistaProcesoId = compra.IdUsuario
left join [vis_usuario] presu on  s.AnalistaPresupuestoId = presu.IdUsuario
left join TipoMoneda mon on  s.TipoMonedaId = mon.Id
left join TipoCompra tcomp on  s.TipoCompraId = tcomp.Id
left join ModalidadCompra modal on  s.ModalidadCompraId = modal.Id
left join Estado est on  s.EstadoId = est.Id
left join Sector unide on  s.UnidadDemandanteId = unide.Id
left join ConceptoPresupuestario cp on s.ConceptoPresupuestarioId= cp.Id
left join ProgramaPresupuestario pp on s.ProgramaPresupuestarioId= pp.Id



where s.CDPNum is not null order by CDPNum;


drop table #TempTable;
END TRY

BEGIN CATCH

	RAISERROR('Error al intentar cargar Reporte',16,1)
	--SELECT
	--ERROR_NUMBER() AS ErrorNumber,
 --   ERROR_STATE() AS ErrorState,
 --   ERROR_SEVERITY() AS ErrorSeverity,
 --   ERROR_PROCEDURE() AS ErrorProcedure,
 --   ERROR_LINE() AS ErrorLine,
 --   ERROR_MESSAGE() AS ErrorMessage;

END CATCH
