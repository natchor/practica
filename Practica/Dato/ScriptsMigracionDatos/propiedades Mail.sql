select * from reqCompra..PropertiesEmail


update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre "$NombreCompra" fue aprobada por $Usuario  <br/>' where Id = 1
update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, se ha creado la Solicitud de compra N° $Id, con nombre "$NombreCompra" por $Usuario <br/>' where Id = 2
update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, se le ha asignado la Solicitud de compra N° $Id, con nombre "$NombreCompra" <br/>' where Id = 3
update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, se ha creado CDP correspondiente a la Solicitud de compra N° $Id, con nombre "$NombreCompra" por $Usuario <br/>' where Id = 4
update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre "$NombreCompra" requiere de su aprobación<br/>' where Id = 5
update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre "$NombreCompra" fue rechazada por $Usuario <br/>' where Id = 6
update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, la Solicitud de compra N° $Id, con nombre "$NombreCompra" fue anulada por $Usuario <br/>' where Id = 7
update reqCompra..PropertiesEmail set Mensaje = 'Por este medio se comunica que, con fecha $fecha, ha finalizado la Solicitud de compra N° $Id, con nombre "$NombreCompra" <br/>' where Id = 8

update reqCompra..PropertiesEmail set Asunto = 'QA - Notifica Aprobación en Solicitud de Compra N° $Id' where Id = 1
update reqCompra..PropertiesEmail set Asunto = 'QA - Notifica Creación en Solicitud de Compra N° $Id' where Id = 2
update reqCompra..PropertiesEmail set Asunto = 'QA - Solicitud de Compra N° $Id asignada' where Id = 3
update reqCompra..PropertiesEmail set Asunto = 'QA - Notifica Creación de CDP en Solicitud de Compra N° $Id' where Id = 4
update reqCompra..PropertiesEmail set Asunto = 'QA - Solicitud de Compra N° $Id requiere de su aprobación' where Id = 5
update reqCompra..PropertiesEmail set Asunto = 'QA - Solicitud de Compra N° $Id rechazada' where Id = 6
update reqCompra..PropertiesEmail set Asunto = 'QA - Solicitud de Compra N° $Id anulada' where Id = 7
update reqCompra..PropertiesEmail set Asunto = 'QA - Notifica proceso de Solicitud de Compra N° $Id Finalizado' where Id = 8