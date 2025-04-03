


select * from Anexos..Cargo  
where pyr_CodigoCargo in (128, 144, 132)

select * from Anexos..Division where descripcion like '%DIVI%'
select * from pyr..TFuncionario where CodigoCentroCosto = 1085             
select * from pyr..TCentroCosto where CodigoCentroCosto = 1085             


select 'SET IDENTITY_INSERT reqCompra..Sector ON' as query
union all
select distinct 'insert into reqCompra..Sector (Id, Nombre, Estado, TienePresupuesto) values (' + cast(d.id AS varchar) + ', ''' + d.descripcion + ''', 1, 0)' as query
from Anexos..Division d
inner join Anexos..Funcionario b on d.pyr_CodigoCentroCosto = b.DivisionId
and b.Activo = 1
union all
select 'SET IDENTITY_INSERT reqCompra..Sector OFF' as query

-- Los sectores padre y si tiene o no presupuesto, el script esta en el archivo
-- https://minenergia.sharepoint.com/:x:/s/DesarrolloTI/EWOnmGe2kvJJs6yD0xD-VcEBkFW-gA48lpwgaySPXq7nbA?e=psGi1d