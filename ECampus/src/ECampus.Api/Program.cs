using ECampus.Api;
using ECampus.Api.MiddlewareFilters;
using ECampus.Core.Extensions;
using ECampus.Domain;
using ECampus.Infrastructure;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Infrastructure.DataCreateServices;
using ECampus.Infrastructure.Interfaces;
using ECampus.Services;
using ECampus.Shared.Auth;
using ECampus.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSingleton<ILogger>(logger);

builder.Services.AddScoped<PrimitiveDataAccessManager>();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")!));

builder.Services.AddScoped<DbContext, ApplicationDbContext>();

builder.Services.AddControllers(options => { options.Filters.Add<MiddlewareExceptionFilter>(); })
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(typeof(DomainAssemblyMarker));
builder.Services.AddUniqueServices(typeof(DomainAssemblyMarker), typeof(DomainAssemblyMarker),
    typeof(ApiAssemblyMarker),
    typeof(InfrastructureAssemblyMarker), typeof(ServicesAssemblyMarker));

builder.Services.UserInstallersFromAssemblyContaining(builder.Configuration, typeof(DomainAssemblyMarker),
    typeof(ApiAssemblyMarker),
    typeof(InfrastructureAssemblyMarker), typeof(ServicesAssemblyMarker));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authOptions = builder.Configuration.GetSection("jwtAuthOptions")
    .Get<JwtAuthOptions>()!;

builder.Services.AddSingleton(authOptions);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace ECampus.Api
{
    public abstract class Program
    {
    }
}