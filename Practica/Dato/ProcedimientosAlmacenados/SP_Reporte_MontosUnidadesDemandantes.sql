USE [reqCompra]
GO

/****** Object:  StoredProcedure [dbo].[SP_Reporte_MontosUnidadDemandante]    Script Date: 20/8/2021 11:46:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--use reqCompra;
 --select * from solicitud where OrdenCompra='584105-334-CM21'
--ALTER DATABASE reqCompra SET COMPATIBILITY_LEVEL = 130
-- execute SP_Reporte_InformacionCDP '<Opciones><Opcion IdE="1"  Nombre="Cristian" Estado="muy bien"/></Opciones>'
CREATE PROCEDURE [dbo].[SP_Reporte_MontosUnidadDemandante]
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


Select UnidadDemandanteId, 
ANIO=year(FechaCreacion), 
Unidad_Demandante=(Select nombre from sector where id =UnidadDemandanteId and year(FechaCreacion) = year(b.FechaCreacion) ), 
N_compras= (select count(id) from solicitud where UnidadDemandanteId= b.UnidadDemandanteId and year(FechaCreacion) = year(b.FechaCreacion)),
N_comprasFinalizadas= (select count(id) from solicitud where UnidadDemandanteId= b.UnidadDemandanteId and EstadoId = 10 and year(FechaCreacion) = year(b.FechaCreacion)),
IDs_Compras =(Select STUFF((SELECT '; '+ NroSolicitud FROM Solicitud WHERE UnidadDemandanteId = b.UnidadDemandanteId  and year(FechaCreacion) = year(b.FechaCreacion)  ORDER BY    NroSolicitud FOR XML PATH('')),1,1,'')  ),
ComprasDiferentePesoFinalizada = (Select count(id) from solicitud where TipoMonedaId <> 5 and EstadoId = 10 and  UnidadDemandanteId = b.UnidadDemandanteId  and year(FechaCreacion) = year(b.FechaCreacion)),
Suma_monto_presupuestado=sum(MontoPresupuestado),
Suma_monto_Ejecutado=sum(MontoFinal),
Suma_Monto_OC = (Select sum(Total) from vis_OrdenCompra where UnidadDemandanteId = b.UnidadDemandanteId and year(FechaCreacion) = year(b.FechaCreacion) and TipoMonedaId = 5 )


from SolicitudDetalle a, Solicitud b 
where a.SolicitudId=b.Id 
group by UnidadDemandanteId, year(FechaCreacion) 


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
GO

