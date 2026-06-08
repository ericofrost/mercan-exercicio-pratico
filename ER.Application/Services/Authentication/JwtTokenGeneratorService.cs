using System.Security.Claims;
using System.Text;
using ER.Domain.Shared;
using ER.Application.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace ER.Application.Services.Authentication;

public class JwtTokenGeneratorService : ITokenGeneratorService
{
    private readonly JsonWebTokenHandler _handler = new();
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Default parameter constructor
    /// </summary>
    /// <param name="configuration">Configuration provided for retrieving values</param>
    public JwtTokenGeneratorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async ValueTask<string?> GenerateTokenAsync(GenerateTokenRequest request)
    {   
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "30");

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = request.EmployeeId.ToString(),
                ["tenant_id"] = request.TenantId.ToString(),
                [JwtRegisteredClaimNames.Email] = request.Email,
                [ClaimTypes.Role] = request.Role.ToString(),
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString()
            }
        };
        
        var token = _handler.CreateToken(descriptor);
        
        return await ValueTask.FromResult(token);
    }
}