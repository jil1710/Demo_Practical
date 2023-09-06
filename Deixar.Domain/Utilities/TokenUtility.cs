using Deixar.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Deixar.Domain.Utilities;

public class TokenUtility
{
    private readonly IConfiguration _configuration;

    public TokenUtility(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Create JWT token
    /// </summary>
    /// <param name="data">Users information including roles</param>
    /// <returns></returns>
    public string GenerateJWT(UserDetails data)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
        {
            Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
            Audience = _configuration.GetValue<string>("Jwt:Audience"),
            Subject = new ClaimsIdentity(new List<Claim>() {
                new Claim("Id", data.User.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, data.User.EmailAddress!),
                new Claim(JwtRegisteredClaimNames.Name, $"{data.User} {data.User.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT Token Id
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()), //time at which JWT issued
                new Claim(ClaimTypes.Role , data.Role)
            }),
            Expires = DateTime.Now.AddDays(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwt = jwtTokenHandler.WriteToken(token);
        return jwt;
    }
}
