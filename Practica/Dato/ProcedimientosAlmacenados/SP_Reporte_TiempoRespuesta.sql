USE [reqCompra]
GO

/****** Object:  StoredProcedure [dbo].[SP_Reporte_TiempoRespuesta]    Script Date: 20/8/2021 11:48:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--use reqCompra;
--ALTER DATABASE reqCompra SET COMPATIBILITY_LEVEL = 130
-- execute SP_Reporte_TiempoRespuesta '<Opciones><Opcion IdE="1"  Nombre="Cristian" Estado="muy bien"/></Opciones>'
CREATE PROCEDURE [dbo].[SP_Reporte_TiempoRespuesta]
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



--INSERT INTO @Opciones (
--  id
--, nombre
--, estado)
--SELECT
--	*
--    FROM OPENJSON(@FiltrosID)
--    WITH (
--      id      smallint '$.id'
--    , nombre varchar(50) '$.nombre'
--    , estado varchar(50) '$.estado'
--    ) AS jsonValues;
 
 select * from @TablaXML 

Select 
ROW_NUMBER() OVER(PARTITION BY SolicitudID ORDER BY Fecha, Id DESC) AS rowNum,
Id,
Fecha,
FechaAnterior=Fecha,
Observacion,
UserId,
UserAnterior=UserId,
SolicitudId,
dias = cast(0 as int)
INTO #TempTable  FROM Bitacora bita 

update #TempTable set FechaAnterior=null,UserAnterior=0

update #TempTable  set 
FechaAnterior=(Select Fecha from #TempTable where rowNum=a.rowNum-1 and solicitudId=a.SolicitudId ),
UserAnterior=(Select COALESCE((UserId),0) from #TempTable where rowNum=a.rowNum-1 and solicitudId=a.SolicitudId ) 
from  #TempTable a where rowNum>1
update #TempTable set dias = DATEDIFF(DAY,FechaAnterior,Fecha)

Select 
--rowNum,
--a.Id,
Fecha,
FechaAnterior,
Observacion,
--UserId,
--UserAnterior,
--SolicitudId,
dias,
--OrigenIdUsuario=b.IdUsuario,
--OrigenNombreFuncionario=b.NombreFuncionario,
--OrigenApellido=b.Apellido,
OrigenUserName=b.UserName,
--OrigenEmail=b.Email,
--OrigenCargoId=b.CargoId,
--OrigenSectorId=b.SectorId,
--OrigenJefeDirectoId=b.JefeDirectoId,
--OrigenIdCargo=b.IdCargo,
OrigenNombreCargo=b.NombreCargo,
--OrigenIdSector=b.IdSector,
OrigenNombreSector=b.NombreSector,
--OrigenCodigo=b.Codigo,
--OrigenEstado=b.Estado,
--OrigenSectorPadre=b.SectorPadre,
--OrigenTienePresupuesto=b.TienePresupuesto,
OrigenUserRole=b.UserRole,

--DestinoIdUsuario=c.IdUsuario,
--DestinoNombreFuncionario=c.NombreFuncionario,
--DestinoApellido=c.Apellido,
DestinoUserName=c.UserName,
--DestinoEmail=c.Email,
--DestinoCargoId=c.CargoId,
--DestinoSectorId=c.SectorId,
--DestinoJefeDirectoId=c.JefeDirectoId,
--DestinoIdCargo=c.IdCargo,
DestinoNombreCargo=c.NombreCargo,
--DestinoIdSector=c.IdSector,
DestinoNombreSector=c.NombreSector,
--DestinoCodigo=c.Codigo,
--DestinoEstado=c.Estado,
--DestinoSectorPadre=c.SectorPadre,
--DestinoTienePresupuesto=c.TienePresupuesto,
DestinoUserRole=c.UserRole,
NroSolicitud,
--SolicitanteId,
--UnidadDemandanteId,
--ProgramaPresupuestarioId,
IniciativaVigenteId,
IniciativaVigente,
--ConceptoPresupuestarioId,
--FolioRequerimientoSIGFE,
--FoliocompromisoSIGFE,
--TipoCompraId,
--TipoMonedaId,
NombreCompra,
--ObjetivoJustificacion,
--ObservacionGeneral,
MontoUTM,
--ModalidadCompraId,
--AnalistaProcesoId,
--FuncionarioValidacionCDPId,
--AnalistaPresupuestoId,
CDPNum,
ContraparteTecnicaId,
FechaCreacion,
OrdenCompra,
--ProveedorNombre,
--ProveedorRut,
--FechaOrdenCompra,
--EstadoId,
FaseCDP,
MontoCLP
--ValorDivisa
from #TempTable a 
left join [vis_usuario] b on  b.IdUsuario = a.UserAnterior 
left join [vis_usuario] c on  c.IdUsuario = a.UserId 
left join Solicitud s on s.Id=a.SolicitudId

drop table #TempTable



--Select *,fechainicio=(select Fecha from bitacora where ) from bitacora a

--Select * from Bitacora

--SELECT *, FROM Solicitud sol INNER JOIN Bitacora bita on sol.id=bita.SolicitudId

--Select top 1 * from Bitacora

END TRY

BEGIN CATCH

	RAISERROR('Error al intentar cargar Reporte',16,1)

END CATCH
GO

