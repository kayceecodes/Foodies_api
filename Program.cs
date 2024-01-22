
using foodies_api;
using foodies_api.Data;
using foodies_api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();
// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure the HTTP client and register the typed client
builder.Services.AddHttpClient<IExternalApiClient, ExternalApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.yelp.com/v3");

});

// Register the API endpoint
builder.Services.AddEndpointsApiExplorer(endpoints =>
{
    endpoints.Map("/", async context =>
    {
        // Use the typed HTTP client to make a request
        var externalApiClient = context.RequestServices.GetRequiredService<IExternalApiClient>();
        string data = await externalApiClient.GetData();

        await context.Response.WriteAsync(data);
    });
});

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})


// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

var app = builder.Build();

app.UseHttpsRedirection();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                       policy  =>
//                       {
//                           policy.WithOrigins("http://example.com",
//                                               "http://www.contoso.com");
//                       });
// });

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("restaurant").AddEndpointFilter<ApiKeyEndpointFilter>();

app.ConfigurationAuthEndpoints();
app.ConfigurationRestaurantEndpoints();

app.Run();
