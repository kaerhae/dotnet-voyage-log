using System.Security.Claims;
using System.Text;
using Amazon.S3;
using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Repository;
using dotnet_voyage_log.Service;
using dotnet_voyage_log.Utilities;
using Microsoft.IdentityModel.Tokens;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IVoyageService, VoyageService>();
builder.Services.AddScoped<IVoyageRepository, VoyageRepository>();
builder.Services.AddSingleton<DataContext>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddSingleton<IConfigs, Configs>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
    builder.Services.AddSingleton<IS3Service, S3Service>(sp =>
        {
            string bucketName = sp.GetRequiredService<IConfigs>().GetImageBucket();
            
            /* If development, use localstack service */
            if(builder.Environment.IsDevelopment()) {

                var client =  new AmazonS3Client(new AmazonS3Config{
                    ForcePathStyle = true,
                    ServiceURL = "http://localstack:4566",
                });
                return new S3Service(client, bucketName, sp.GetRequiredService<ILogger<IS3Service>>());
            }

            /* Production, use default aws options */
            return new S3Service(new AmazonS3Client(), bucketName, sp.GetRequiredService<ILogger<IS3Service>>());

        });

builder.Services.AddAuthorization();
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options => 
    {    
        string? key = builder.Configuration["JwtSettings:SecretKey"] ?? Environment.GetEnvironmentVariable("SECRET_KEY");

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key ?? throw new Exception("Key is missing"))),
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
        policy.RequireClaim(ClaimTypes.Role, "user");
    });
    opts.AddPolicy("AllUsers",
        policy => policy.RequireClaim(ClaimTypes.Role));
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
if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseStatusCodePagesWithReExecute("/error/{0}");
    }app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
