using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_voyage_log.Utilities;


public class TokenGenerator : ITokenGenerator
{
    protected readonly IConfigs _config;

    public TokenGenerator(IConfigs config) {
        _config = config;
    }

    /// <summary>
    /// Token generation function. Requires SECRET_KEY, JWT_AUDIENCE, and JWT_ISSUER environment variables. 
    /// Generates token, which has 1 day expiration, Subject contains User.Id and User.AppRole as claims.
    /// </summary>
    /// <returns>
    /// string: Generated token
    /// </returns>
    public string GenerateToken(User user)
    {
        /*
        *   Generate token, which contains userId and appRole for correct permissions  
        */
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config.GetSecretKey());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [ 
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.AppRole)
                ]),
            Issuer = _config.GetIssuer(),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _config.GetAudience()
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}