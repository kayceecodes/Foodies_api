using foodies_api;
using foodies_api.Data;
using foodies_api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var httpContextAccessor = new HttpContextAccessor();
var context = httpContextAccessor.HttpContext;

ConfigurationManager configuration = builder.Configuration;

// Configure services
builder.Services.AddHttpClient("YelpApiClient", client => 
{
    client.BaseAddress = new Uri(configuration.GetValue<string>(YelpConstants.BaseAddress));    
});

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Add YelpApiClient as a singleton with configuration
builder.Services.AddSingleton<YelpApiClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();

// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


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

// app.MapGroup("Some Name - Auth Endpoints").AddEndpointFilter<ApiKeyEndpointFilter>();

app.ConfigurationAuthEndpoints();
app.ConfigurationRestaurantEndpoints();
app.ConfigurationReviewEndpoints();

app.Run();
    