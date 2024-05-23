using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EelegantIot.Api.Domain.Entities;
using EelegantIot.Api.Infrastructure;
using EelegantIot.Api.Models;
using EelegantIot.Shared.Requests.Login;
using EelegantIot.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EelegantIot.Api.Api;

[ApiController]
[Route("users")]
public class UserControllers(AppDbContext dbContext, IOptions<JwtConfigDto> jwtOptions) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<ResponseData<LoginResponse>>> Login(LoginRequest loginRequest)
    {
        if (dbContext.Users.FirstOrDefault(x => x.Username == loginRequest.Username) is { } user)
        {
            if (user.Password == loginRequest.Password)
            {
                return new ResponseData<LoginResponse>(GenerateToken(user));
            }

            return new ResponseData<LoginResponse>(new ErrorModel("کاربری با این مشخصات یافت نشد"));
        }

        user = new(Guid.NewGuid(), loginRequest.Username, loginRequest.Password, DateTime.Now);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return new ResponseData<LoginResponse>(GenerateToken(user));
    }

    private LoginResponse GenerateToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        byte[] secretBytes = Encoding.ASCII.GetBytes(jwtOptions.Value.Secret);
        SigningCredentials signingCredentials =
            new(new SymmetricSecurityKey(secretBytes), SecurityAlgorithms.HmacSha512Signature);

        byte[] encryptionKey = Encoding.ASCII.GetBytes(jwtOptions.Value.EncryptionKey);
        EncryptingCredentials encryptingCredentials = new(new SymmetricSecurityKey(encryptionKey),
            SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
            }),

            Expires = DateTime.Now.AddDays(jwtOptions.Value.ValidDays),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        var jwtToken = jwtTokenHandler.WriteToken(token);

        return new LoginResponse
        {
            ExpireDate = tokenDescriptor.Expires.Value,
            Token = jwtToken
        };
    }
}