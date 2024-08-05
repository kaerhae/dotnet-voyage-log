using System.Security.Claims;
using System.Text;
using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Repository;
using dotnet_voyage_log.Service;
using dotnet_voyage_log.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddSingleton<DataContext>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddSingleton<IConfigs, Configs>();
builder.Services.AddScoped<IAuthentication, Authentication>();

builder.Services.AddAuthorization();
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options => 
    {    
        var key = builder.Configuration["JwtSettings:SecretKey"] ?? Environment.GetEnvironmentVariable("SECRET_KEY");

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JwtSettings:Audience"] ??  Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    ClockSkew = TimeSpan.Zero
                };
    });
builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("Admins", policy => {
        policy.RequireClaim(ClaimTypes.Role, "admin");
    });
    opts.AddPolicy("Users", policy => {
        policy.RequireClaim(ClaimTypes.Role, "User");
    });
});
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
