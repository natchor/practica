  --script que entrega diferencias entre RH y reqcompra sobre jefaturas directas 
  
  DECLARE @VT_JefeDirecto TABLE(

  UserName_RH VARCHAR(100) , 
  Nombre_RH VARCHAR(100) ,
  Dependencia_RH VARCHAR(100) ,
  Dependencia_RC VARCHAR(100),
  UserName_JefeD VARCHAR(100) ,
  Nombre_RH_JD VARCHAR(100) ,
  UserName_RC_JD VARCHAR(100))
 
  insert into @VT_JefeDirecto
  Select 
  TU.WinUser,
  TU.Nombres+' '+ TU.ApellidoPaterno +' '+TU.ApellidoMaterno,
  TJ.Nombre,
  (Select Nombre from reqCompra..Sector where Id = US.SectorId),
  TUJ.WinUser,
  TUJ.Nombres+' '+ TUJ.ApellidoPaterno +' '+TUJ.ApellidoMaterno,
  (Select UserName from reqCompra..[User] where Id=US.JefeDirectoId)


  from intranet..t_Usuarios TU 
  inner join intranet..t_FuncionariosXJefatura TF on TU.IdUsuario=TF.IDUsuario 
  inner join intranet..t_Jefaturas TJ on TJ.IdJefatura=TF.IdJefatura 
  inner join intranet..t_Usuarios TUJ on TUJ.IDUsuario=TJ.IDUsuario
  inner join reqCompra..[User] US on  US.UserName = TU.WinUser 
  where US.Estado=1

  Select *,'update reqCompra..[User] set JefeDirectoId='+ CAST((Select id from reqCompra..[User] where UserName= A.UserName_JefeD) AS varchar)+ ' where id='+ CAST((Select id from reqCompra..[User] where UserName= A.UserName_RH) AS varchar)  from @VT_JefeDirecto A where  ( UserName_JefeD <>
  UserName_RC_JD )   order by UserName_RC_JD




