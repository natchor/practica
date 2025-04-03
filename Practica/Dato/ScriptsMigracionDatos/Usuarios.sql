select 'SET IDENTITY_INSERT reqCompra..[User] ON' as query
union all
select 
distinct 'insert into reqCompra..[User] (Id, Nombre, Apellido, UserName, [Password], Email, CargoId, SectorId, JefeDirectoId) values (
' + cast(f.Id as varchar) + '
, ''' + pl.Nombres + '''
, ''' + pl.ApellidoPaterno + ' ' + pl.ApellidoMaterno +  '''
, ''' + SUBSTRING(f.Email, 1, CHARINDEX('@', f.Email) - 1) + '''
, 0
, ''' + f.Email + '''
, ' + CAST(
  case 
	when CargoId in (129, 143, 141) then 2 --profesional
	when CargoId in (130, 139) then 3 --tecnico
	when CargoId = 125 then 10 --seremi
	when CargoId in (127, 131, 136 ) then 6 -- asistente administrativo
	when CargoId = 123 then 9 --subsecretario
 	when CargoId in (138, 133, 140) then 5
 	when CargoId in (126, 142) then 14 -- JEFES
 	when CargoId = 124 then 15 -- ASESOR/A
 	when CargoId = 122 then 8 -- MINISTRO
 	when CargoId in (137, 132) then 16 -- AUXILIAR
	when CargoId in (128) then 17 -- CONDUCTOR
	when CargoId in (144) then 18 -- EXPERTA/O
	else CargoId
	end  as varchar)
	+
'
, ' + CAST( d.id as varchar) + '
, ' + cast(f.Id as varchar) + '
)' as query

--,  (select id from Anexos..Funcionario where CodigoPersona = f.FullRutJefe) as jefeDirecto

from Anexos..Funcionario f
inner join Anexos..Division d on f.DivisionId = d.pyr_CodigoCentroCosto 
inner join pyr.dbo.TPersonasLocal pl on f.CodigoPersona = pl.CodigoPersona
where Activo = 1
--and f.CargoId not in ( 144 )
--and f.Id = 1359
union all
select 'SET IDENTITY_INSERT reqCompra..[User] OFF' as query

--actualizar jefe directo
select 
'update reqCompra..[User] set jefeDirectoId = ' + cast((select id from Anexos..Funcionario where CodigoPersona = f.FullRutJefe) as varchar) + ' where Id = ' + cast(f.Id as varchar)
--,(select id from Anexos..Funcionario where CodigoPersona = f.FullRutJefe) 
--, *
from Anexos..Funcionario f
inner join Anexos..Division d on f.DivisionId = d.pyr_CodigoCentroCosto 
inner join pyr.dbo.TPersonasLocal pl on f.CodigoPersona = pl.CodigoPersona
where Activo = 1
and f.CargoId not in ( 144 )


insert into reqCompra..[User] (Nombre, Apellido, UserName, [Password], Email, CargoId, SectorId, JefeDirectoId) 
values ('CRISTIAN', 'MAC-NAMARA', 'cmac-namara', '0', 'cmac-namara@minenergia.cl', 2, 232, 856)


select distinct 'insert into reqCompra..UserRole (UserId, RoleId) values (' + cast(UserId as varchar)   + ', ' + cast(RoleId as varchar) + ')' as query
from reqCompra..UserRole


--Concepto presupuestario
select distinct 'insert into reqCompra..ConceptoPresupuestario (Nombre, Codigo, Estado) values (''' + Nombre   + ''', ''' + ISNULL( Codigo, 'NULL')+ ''', ' + CAST( Estado as varchar) + ')' as query
from reqCompra..ConceptoPresupuestario

