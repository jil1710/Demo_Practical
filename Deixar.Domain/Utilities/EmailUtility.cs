using Deixar.Domain.Interfaces;
using Deixar.DTOs;
using MailKit.Net.Smtp;
using MimeKit;

namespace Deixar.Domain.Utilities;

public class EmailUtility : IEmailUtility
{
    private readonly EmailConfiguration _emailConfig;

    public EmailUtility(EmailConfiguration emailConfiguration)
    {
        _emailConfig = emailConfiguration;
    }

    public void SendMail(Message message)
    {
        MimeMessage emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    /// <summary>
    /// Helper method which send mail using SmtpClient service
    /// </summary>
    /// <param name="emailMessage">Mime message</param>
    private void Send(MimeMessage emailMessage)
    {
        using SmtpClient smtpClient = new SmtpClient();
        try
        {
            smtpClient.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
            smtpClient.Authenticate(_emailConfig.From, _emailConfig.Password);
            smtpClient.Send(emailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception ====> {ex.Message}");
        }
        finally
        {
            smtpClient.Disconnect(true);
            smtpClient.Dispose();
        }
    }

    /// <summary>
    /// Helper method which create mail body
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
        return emailMessage;
    }
}
