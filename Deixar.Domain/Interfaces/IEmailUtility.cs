using Deixar.DTOs;

namespace Deixar.Domain.Interfaces;

public interface IEmailUtility
{
    /// <summary>
    /// Send mail using SMTP protocol
    /// </summary>
    void SendMail(Message message);
}
