﻿using Eshop.Persistence;
using Eshop.WebApi.Filters;
using Eshop.WebApi.Infrastructure;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(SetupControllers);
SetupSwagger(builder);
SetupEntityFramework(builder);
SetupMediatrAndFluentValidation(builder);
var devCorsPolicy = SetupDevCors(builder);
SetupAuthenticationAndAuthorization(builder);

var app = builder.Build();
CreateDbIfNotExists(app);
await SetupDevEnvironment(devCorsPolicy, app);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void SetupControllers(MvcOptions options)
{
    options.Filters.Add<GlobalExceptionFilter>();
}

static void SetupSwagger(WebApplicationBuilder builder)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter your Bearer token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}

static void SetupEntityFramework(WebApplicationBuilder builder)
{
    // EF Configuration
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<EshopDbContext>(
        options => options.UseSqlServer(connectionString)
                          .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                          .EnableSensitiveDataLogging(true)
    );
}

static void SetupMediatrAndFluentValidation(WebApplicationBuilder builder)
{
    // MediatR - the only nuget package needed is MediatR.Extensions.FluentValidation.AspNetCore - rest are included automatically
    var assembly = typeof(Program).Assembly;
    builder.Services.AddFluentValidation([assembly]);
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}

static string SetupDevCors(WebApplicationBuilder builder)
{
    var devCorsPolicy = "devCorsPolicy";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(devCorsPolicy, builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
    });
    return devCorsPolicy;
}

static void SetupAuthenticationAndAuthorization(WebApplicationBuilder builder)
{
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
    {
        cfg.User.RequireUniqueEmail = true;

        cfg.Password.RequireDigit = true;
        cfg.Password.RequiredLength = 6;
        cfg.Password.RequireNonAlphanumeric = true;
        cfg.Password.RequireUppercase = true;
        cfg.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<EshopDbContext>()
    .AddDefaultTokenProviders();

    var authConfiguration = new AuthConfiguration(builder.Configuration);
    builder.Services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = authConfiguration.SigningKey,
            ValidateIssuer = true,
            ValidIssuer = authConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = authConfiguration.Audience,
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role
        };
    });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Administrator"));
    });

    builder.Services.AddScoped<IUserContext, UserContext>();
    builder.Services.AddHttpContextAccessor();
}

static void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<EshopDbContext>();
            context.Database.EnsureCreated(); // TODO: later we will use Migrate()
        }
        catch (Exception)
        {
            throw;
        }
    }
}

static async Task SetupDevEnvironment(string devCorsPolicy, WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        await UserSeeder.SeedData(app);

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors(devCorsPolicy);
    }
}

[ExcludeFromCodeCoverage]
public partial class Program { }