USE [reqCompra]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--use reqCompra;
--ALTER DATABASE reqCompra SET COMPATIBILITY_LEVEL = 130
-- execute SP_Reiniciar_Sequence '<Opciones><Opcion IdE="1"  Nombre="Cristian" Estado="muy bien"/></Opciones>'
ALTER PROCEDURE [dbo].[SP_Reiniciar_Sequence]
  @XML ntext = null
as
BEGIN TRY
 
 ALTER SEQUENCE Solicitud_CDP RESTART WITH 1 ; 
 ALTER SEQUENCE Solicitud_CorrelativoAnual RESTART WITH 1 ; 


END TRY

BEGIN CATCH

	RAISERROR('Error al intentar Reiniciar Sequence',16,1)
	--SELECT
	--ERROR_NUMBER() AS ErrorNumber,
 --   ERROR_STATE() AS ErrorState,
 --   ERROR_SEVERITY() AS ErrorSeverity,
 --   ERROR_PROCEDURE() AS ErrorProcedure,
 --   ERROR_LINE() AS ErrorLine,
 --   ERROR_MESSAGE() AS ErrorMessage;

END CATCH
