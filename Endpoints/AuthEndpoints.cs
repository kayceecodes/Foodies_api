using foodies_api.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace foodies_api.Endpoints;

public static class AuthEndpoints
{
    public static void ConfigurationAuthEndpoints(this WebApplication app) 
    {
        app.MapPost("/api/login", Login).WithName("Login").Accepts<LoginDto>("application/json")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/api/register", Register).WithName("Register").Accepts<RegistrationDto>("application/json")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public async static Task<IResult> Login(UserManager<IdentityUser> _userManager, [FromBody] LoginDto loginDto) 
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username);

        if(user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password)) 
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles) 
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);
            return Results.Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
        }
        return Results.Unauthorized();
    }

    public async static Task<IResult> Register([FromBody] RegistrationDto registerDto, UserManager<IdentityUser> _userManager)
    {
        var userExists = await _userManager.FindByNameAsync(registerDto.Username);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

        IdentityUser user = new()
        {
            Email = registerDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerDto.Username
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    private static JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
 