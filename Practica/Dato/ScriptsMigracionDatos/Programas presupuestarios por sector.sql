-- De actuerdo a lo indicado en el excel Unidades Demandantes y Campos_pdg.xlsx
-- https://bit.ly/2Qo5XLq


select * from reqCompra..ProgramaPresupuestario
select * from reqCompra..SectProgPre
select * from reqCompra..Sector where Nombre like '%relacio%'
/*
faltan:
Dvisión de Mercados Eléctricos
División de Combustibles y Nuevos Energéticos- P01
División de Políticas y Estudios Energeticos y Ambientales
Nivel Central - Administración

*/


--PROGRAMA 01
select * from reqCompra..[User] where Nombre like '%brec%'

--insert into reqCompra..SectProgPre (SectorId, ProgramaPresupuestarioId, UserEncargadoId) values (204, 1, 1306)
--insert into reqCompra..SectProgPre (SectorId, ProgramaPresupuestarioId, UserEncargadoId) values (233, 1, 1306)
--insert into reqCompra..SectProgPre (SectorId, ProgramaPresupuestarioId, UserEncargadoId) values (232, 1, 1378) --DEPARTAMENTO DE TECNOLOGÍAS DE INFORMACIÓN



select * from reqCompra..Sector where Id in (204, 243, 213, 225, 234,239,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194) --programa 01


insert into reqCompra..SectProgPre (SectorId, ProgramaPresupuestarioId, UserEncargadoId)
select id, 1, 1306 from reqCompra..Sector where Id in (204, 233, 232, 204, 243, 213, 225, 234,239,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194) --programa 01

--PROGRAMA 03
select * from reqCompra..ProgramaPresupuestario
select * from reqCompra..Sector where Nombre like '%sos%'
select * from reqCompra..[User] where Nombre like '%paz%'

insert into reqCompra..SectProgPre (SectorId, ProgramaPresupuestarioId, UserEncargadoId) values (204, 3, 1306)

--PROGRAMA 04
select * from reqCompra..Sector where Nombre like '%desa%'
select * from reqCompra..[User] where Nombre like '%marcolin%' 
insert into reqCompra..SectProgPre (SectorId, ProgramaPresupuestarioId, UserEncargadoId) values (220, 4, 143)


--PROGRAMA 05

insert into reqCompra..SectProgPre (SectorId, ProgramaPresupuestarioId, UserEncargadoId) values (204, 5, 1306)

--SIN APLICACION 
select * from reqCompra..Sector where Nombre like '%gore%'
