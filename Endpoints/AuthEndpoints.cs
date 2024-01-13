using Microsoft.AspNetCore.Identity;

namespace foodies_api;

public static class AuthEndpoints
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthEndpoints(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
    ) 
    {

    }
    public async static Task<IResult> Login(LoginModel model) 
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if(userExists != null) 
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                new Response { Status = "Error",
                MessageProcessingHandler = "User already exists!"} );
        }
    }
}
