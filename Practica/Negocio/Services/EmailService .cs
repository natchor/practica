using Biblioteca;
using Dato.Interfaces.Repositories;
using DemoIntro.Models;
using Entidad.Interfaz.Models.PropertiesEmailModels;
using Entidad.Interfaz.Models.SolicitudModels;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Negocio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfigurationService _emailConfiguration;
        private readonly IPropertiesEmailService _servPropEmail;
        private readonly ISolicitudDetalleRepository _repoSolicitudDetalle;

        public EmailService(IEmailConfigurationService emailConfiguration, IPropertiesEmailService propEmailService, ISolicitudDetalleRepository solicitudDetalleRepository)
        {
            _emailConfiguration = emailConfiguration;
            _servPropEmail = propEmailService;
            _repoSolicitudDetalle = solicitudDetalleRepository;
        }

        //public List<EmailMessage> ReceiveEmail()
        //{
        //	throw new NotImplementedException();
        //}

        //public void Send()
        //{
        //	throw new NotImplementedException();
        //}

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emails.Add(emailMessage);
                }

                return emails;
            }
        }

        private string GenerarGlosaDifMontos(int idSolicitud)
        {
            var detalle = _repoSolicitudDetalle.Query().Where(sd => sd.SolicitudId == idSolicitud).ToList();

            string glosaDifMontos = string.Empty;
            foreach (var item in detalle)
            {
                glosaDifMontos += $"Año {item.Anio}: <br>Monto Presupuestado: {item.MontoPresupuestado} <br>Monto final: {item.MontoFinal} <br>Diferencia: {Math.Abs(item.MontoPresupuestado - item.MontoFinal)} <br><br>";
            }

            return glosaDifMontos;

        }

        public EmailMessage ArmaMensaje(string correos, string usuarioAccion, string tipo, SolicitudModel model, string userNameDest = "")
        {
            try
            {


                PropertiesEmailModel propMail = _servPropEmail.FindByCodigo(tipo);
                if (propMail.Cc != null)
                {
                    correos = correos +","+ propMail.Cc;
                }
                string asunto = propMail.Asunto.Trim();

                //asunto = asunto.Replace("$Id", model.Id.ToString());
                asunto = asunto.Replace("$Id", model.NroSolicitud);
                string idDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                String msg = propMail.Mensaje.Trim();
                //msg = msg.Replace("$Id", model.Id.ToString());
                
                msg = msg.Replace("$Id", $"<a href='{SiteKeys.WebSiteDomain}/Login?ReturnUrl=/Solicitud/Ver/{model.Id}'>{model.NroSolicitud}</a>" );
                msg = msg.Replace("$fecha", idDate);
                msg = msg.Replace("$Usuario", usuarioAccion);
                msg = msg.Replace("$NombreCompra", model.NombreCompra);
                msg = msg.Replace("$foreachMontos", GenerarGlosaDifMontos(model.Id));

                if (model.ObservacionGeneral != null)
                    if (model.ObservacionGeneral.StartsWith("0-"))
                        msg = msg.Replace("$Observacion", model.ObservacionGeneral.Substring(2));

                string msgHtml = CuerpoMail(msg, userNameDest);

                EmailMessage mail = new EmailMessage();
                EmailAddress direccion = new EmailAddress();
                List<EmailAddress> libreta = new List<EmailAddress>();
                direccion.Address = propMail.From;
                direccion.Name = propMail.FromNombre;
                libreta.Add(direccion);

                mail.Subject = asunto;
                mail.Content = msgHtml;
                mail.FromAddresses = libreta;
                mail.ToAddresses = CorreoDestino(correos);
                //mail.
                return mail;
            }
            catch
            {
                throw;
            }
        }

        private List<EmailAddress> CorreoDestino(string correos)
        {

            List<string> myListCorreos = new List<string>(correos.Split(','));
            List<EmailAddress> libreta = new List<EmailAddress>();
            foreach (string element in myListCorreos)
            {
                if (!string.IsNullOrEmpty(element))
                {
                    EmailAddress direccion = new EmailAddress();
                    direccion.Address = element;
                    //direccion.Name = element.Substring;
                    libreta.Add(direccion);
                }
            }

            return libreta;
        }

        private string CuerpoMail(string msg, string userNameDestinatario)
        {
            string estilo = @"<!-- 
                                        .Estilo1 {
                                            font-family: Arial, Helvetica, sans-serif; 
                                            font-size: 12px;
                                            color: #000000;}
                                        .Estilo2 {
                                            color: #006699;
                                            font-family: Arial, Helvetica, sans-serif;
                                            font-size: 12px;}
                                        .Estilo4 {
                                font-family: Arial, Helvetica, sans-serif;
                                font-size: 11px;
                                color: #666666;}
                                .Estilo12 {
                                font-family: Arial, Helvetica, sans-serif;
                                color: #FF6600
                                font-size: 12px;}
                                .Estilo13 {
                                font-family: Arial, Helvetica, sans-serif;
                                font-size: 12px;
                                color: #000000;
                                font-weight: bold;
                                font-style: italic;}
                                -->";

            String msgHtml = $@"<html>
                                    <head>
                                        <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
                                        <title>Documento sin t&iacute;tulo</title>
                                        <style type='text/css'>
                                        {estilo}
                                </style>
                                </head>
                                <body>
                                <table width='100%' border='0' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td height='156' valign='top' class='Estilo2'>
                                <table border='0' cellpadding='0' cellspacing='0'>
                                <tr> <td colspan='2' class='Estilo2'>
                                <p class='Estilo13'>Estimado(s) Usuario(s): {userNameDestinatario}</p>
                                <p class='Estilo1'>{msg}</p>
                                <br>
                                </td>
                                </tr>
                                <tr>
                                <td colspan = '2'>
                                <p class='Estilo13'> Atentamente, </p>
								<p class='MsoNormal'><span style='font-size:18.0pt;font-family:&quot;MS Gothic&quot;;color:#0168b3' >━━━</span><span style='font-size:18.0pt;font-family:&quot;MS Gothic&quot;;color:#ee3a43'>━━━━━<u></u><u></u></span></p>
								</td>
							    </tr>
                                <tr>
                                <td width='299' class='Estilo12'>
                                <strong class='Estilo2'>MINISTERIO DE ENERGIA</strong><br />
                                Alameda 1449, Edificio Santiago Downtown II, Santiago de Chile <br />
                                www.energia.gob.cl/
                                </td>
                                <td width='433'></td></tr>
                                <tr></tr>
                                <tr><td colspan='2'>&nbsp;</td></tr></table></td></tr></table> 
                                <table width='100%' border='0' cellpadding='0' cellspacing='2'>
                                <tr><td class='Estilo2'><div align='justify'>
                                <hr align='left' width='100%' noshade='noshade' color='#cccccc' />
                                <span class='Estilo4'>Este e-mail ha sido generado autom&aacute;ticamente desde nuestro portal web, por favor no responda a este message</span>
                                </div></td></tr>
                                <tr><td height='3'>&nbsp;</td></tr>
                                </table></body></html>";
            return msgHtml;

        }

        public void Send(EmailMessage emailMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.Subject = emailMessage.Subject;
                //estamos enviando HTML. Pero hay opciones para texto plano, etc. 
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = emailMessage.Content
                };

                //¡Tenga cuidado de que la clase SmtpClient sea la de Mailkit, no el marco!
                using (var emailClient = new SmtpClient())
                {
                    //// El último parámetro aquí es usar SSL
                    //SecureSocketOptions options = SecureSocketOptions.Auto;
                    SecureSocketOptions options = SecureSocketOptions.None;
                    //SecureSocketOptions options = SecureSocketOptions.SslOnConnect;
                    //SecureSocketOptions options = SecureSocketOptions.StartTls;
                    //SecureSocketOptions options = SecureSocketOptions.StartTlsWhenAvailable;
                    //emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                    //emailClient.Connect("10.0.0.245", 25, options); //25 or 587   | 465
                    emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, options);

                    //// Elimina cualquier funcionalidad de OAuth ya que no la usamos. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    //emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                    emailClient.Send(message);

                    emailClient.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                
            }

        }

    }
}