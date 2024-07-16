using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using First_Backend.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        // Entry point of the application
        var builder = WebApplication.CreateBuilder(args);

        // Adding services to the controller
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        // Database Services
        builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configuring JWT Authentication
        var jwtKey = builder.Configuration["Jwt:Key"]; // JWT Key
        var jwtIssuer = builder.Configuration["Jwt:Issuer"]; // JWT Issuer
        var jwtAudience = builder.Configuration["Jwt:Audience"]; // JWT Audience

        // Check if the JWT config values are missing
        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
        {
            throw new InvalidOperationException("JWT configuration values are missing");
        }

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        builder.Services.AddAuthorization();

        // Build the web application
        var app = builder.Build();

        // Configure the HTTP Pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // app.UseHttpsRedirection(); // Not necessary for now

        // Initialize Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Start the application
        app.Run();
    }
}