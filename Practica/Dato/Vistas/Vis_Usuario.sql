USE [reqCompra]
GO

/****** Object:  View [dbo].[vis_Usuario]    Script Date: 20/8/2021 11:45:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--DROP VIEW vis_Usuario
--ALTER VIEW vis_Usuario
CREATE VIEW [dbo].[vis_Usuario]
AS SELECT 
IdUsuario=u.Id,
NombreFuncionario=u.Nombre,
Apellido,
UserName,
--Password,
Email,
CargoId,
SectorId,
JefeDirectoId, 
IdCargo=c.Id,
NombreCargo=c.Nombre, 
IdSector=s.id,
NombreSector=s.Nombre,
Codigo,
Estado,
SectorPadre,
TienePresupuesto,
UserRole =(Select STUFF((SELECT ','+ Nombre FROM [Role] WHERE id in (select RoleId from UserRole where UserId=u.id) ORDER BY    Nombre FOR XML PATH('')),1,1,'')  )
FROM [User] u LEFT JOIN Cargo c  on  u.CargoId=c.Id 
LEFT JOIN Sector s  on  u.SectorId=s.Id 


--select * from vis_Usuario
GO

