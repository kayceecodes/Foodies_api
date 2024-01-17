﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace foodies_api.Endpoints;

public static class AuthEndpoints
{
    // private readonly RoleManager<IdentityRole> _roleManager;
    // private readonly IConfiguration _configuration;

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
                    toke = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
        }
        return Results.Unauthorized();
    }
}
 