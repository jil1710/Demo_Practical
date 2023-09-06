using Deixar.API.Commons;
using Deixar.Domain.DTOs;
using Deixar.Domain.Interfaces;
using Deixar.Domain.Utilities;
using Deixar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deixar.API.Controllers.V2;

[LogMethod]
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmailUtility _emailUtility;
    private readonly TokenUtility _tokenUtility;

    public AccountController(IAccountRepository accountRepository, TokenUtility tokenUtility, IEmailUtility emailUtility)
    {
        _accountRepository = accountRepository;
        _tokenUtility = tokenUtility;
        _emailUtility = emailUtility;
    }

    /// <summary>
    /// Check given credentials and if it is valid than return token
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] Credentials creds)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid credentials.");
        if (!await _accountRepository.IsUserExist(creds.EmailAddress)) return BadRequest(new { Error = "User not found!" });
        UserDetails? userData = await _accountRepository.GetUserByEmailPasswordAsync(creds.EmailAddress, creds.Password);
        string token = _tokenUtility.GenerateJWT(userData);

        //Send mail on each successful login 
        Message message = new Message(new[] { "jil1710.jp@gmail.com" }, "New login found", $"<h1>New device login at {DateTime.UtcNow}</h1>");
        _emailUtility.SendMail(message);

        return Ok(new { Success = "Login successful.", Token = token });
    }

    /// <summary>
    /// Create new user with role (HR access only)
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Authorize(Roles = "HR")]
    [HttpPost]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserModel user)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid user details.");
        if (!ModelState.IsValid) return BadRequest(new { Error = "Invalid User details." });
        int id = await _accountRepository.RegisterUserAsync(user);
        if (id == -1) return BadRequest(new { Error = "Something went wrong registering new user." });
        return Ok(new { Success = "User registered successful." });
    }
}
