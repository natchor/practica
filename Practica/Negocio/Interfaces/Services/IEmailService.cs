using Biblioteca;
using Entidad.Interfaz.Models.SolicitudModels;
using System.Collections.Generic;

namespace Negocio.Interfaces.Services
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
        EmailMessage ArmaMensaje(string correos, string usuario, string tipo, SolicitudModel model, string userNameDest = "");
    }
}

