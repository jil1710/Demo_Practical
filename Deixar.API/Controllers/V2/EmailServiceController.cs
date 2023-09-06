using Deixar.API.Commons;
using Deixar.Domain.DTOs;
using Deixar.Domain.Interfaces;
using Deixar.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deixar.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[EndpointGroupName("v2")]
[LogMethod]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmailServiceController : ControllerBase
{
    private readonly IEmailUtility _emailService;

    public EmailServiceController(IEmailUtility emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Send mail (HR access only)
    /// </summary>
    [Authorize(Roles = "HR")]
    [HttpPost]
    public void SendMail(MailRequest mailRequest)
    {
        var msg = new Message(mailRequest.To, mailRequest.Subject, mailRequest.Body);
        _emailService.SendMail(msg);
    }
}
