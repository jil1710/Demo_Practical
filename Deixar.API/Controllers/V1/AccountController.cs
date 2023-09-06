using Deixar.API.Commons;
using Deixar.Domain.DTOs;
using Deixar.Domain.Interfaces;
using Deixar.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Deixar.API.Controllers.V1;

[LogMethod]
[ApiController]
[ApiVersion("1.0")]
[EndpointGroupName("v1")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly TokenUtility _tokenUtility;

    public AccountController(IAccountRepository accountRepository, TokenUtility tokenUtility)
    {
        _accountRepository = accountRepository;
        _tokenUtility = tokenUtility;
    }

    /// <summary>
    /// Check given credentials
    /// </summary>
    /// <returns>Access token on successful login</returns>
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] Credentials creds)
    {
        if (!await _accountRepository.IsUserExist(creds.EmailAddress)) return BadRequest(new { Error = "User not found!" });
        UserDetails? user = await _accountRepository.GetUserByEmailPasswordAsync(creds.EmailAddress, creds.Password);
        string token = _tokenUtility.GenerateJWT(user);
        return Ok(new { Success = "Login successful.", Token = token });
    }

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserModel user)
    {
        if (!ModelState.IsValid) return BadRequest(new { Error = "Invalid User details." });
        int id = await _accountRepository.RegisterUserAsync(user);
        if (id == -1) return BadRequest(new { Error = "Something went wrong registering new user." });
        return Ok(new { Success = "User registered successful." });
    }
}
