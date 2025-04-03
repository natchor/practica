USE [reqCompra]
GO

/****** Object:  StoredProcedure [dbo].[SP_Reporte_SoliciudesUsuarioEstado]    Script Date: 20/8/2021 11:47:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--use reqCompra;
 --select * from solicitud where OrdenCompra='584105-334-CM21'
--ALTER DATABASE reqCompra SET COMPATIBILITY_LEVEL = 130
-- execute SP_Reporte_SoliciudesUsuarioEstado '<Opciones><Opcion IdE="1"  Nombre="Cristian" Estado="muy bien"/></Opciones>'
CREATE PROCEDURE [dbo].[SP_Reporte_SoliciudesUsuarioEstado]
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


declare @AuxSoli table
(
	Id smallint,
	AprobadorActualConfig smallint,
	nroSolicitud nvarchar (20),
	UserID smallint
)
insert into  @AuxSoli 
Select id, AprobadorActualId,NroSolicitud, UserId =  (select UserAprobadorId from aprobacion where AprobacionConfigId= s.AprobadorActualId and SolicitudId=s.id ) from solicitud s where EstadoId in (2,8) and  AprobadorActualId not in (1)

select  UsuarioEstado=(select UserName from [User] where Id= UserID), Cantidad=count(*),Solicitudes=(Select STUFF((SELECT '; '+ b.NroSolicitud FROM @AuxSoli b WHERE b.UserID=a.UserID  ORDER BY    NroSolicitud FOR XML PATH('')),1,1,'')  ) from @AuxSoli a  group by UserID
union all


Select 'Presupuesto', count(id),(Select STUFF((SELECT '; '+ b.NroSolicitud FROM Solicitud b WHERE b.EstadoId=a.EstadoId and AprobadorActualId in (1)   ORDER BY    NroSolicitud FOR XML PATH('')),1,1,'')  ) from Solicitud  a where AprobadorActualId in (1) and EstadoId=2 group by EstadoId

union all

Select (select nombre from Estado where Id=EstadoId), count(*), (Select STUFF((SELECT '; '+ b.NroSolicitud FROM Solicitud b WHERE b.EstadoId=a.EstadoId and a.EstadoId in (11,10,4,1,9,3)   ORDER BY    NroSolicitud FOR XML PATH('')),1,1,'')  ) from Solicitud a where a.EstadoId in (11,10,4,1,9,3) group by EstadoId




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

