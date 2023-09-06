using Deixar.API.Commons;
using Deixar.Domain.Interfaces;
using Deixar.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deixar.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[EndpointGroupName("v1")]
[LogMethod]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ControllerName("Email Service")]
public class EmailServiceController : ControllerBase
{
    private readonly IEmailUtility _emailService;
    public EmailServiceController(IEmailUtility emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Email sender
    /// </summary>
    [HttpPost]
    public void SendMail(string[] to, string subject, string content)
    {
        var msg = new Message(to, subject, content);
        _emailService.SendMail(msg);
    }
}
