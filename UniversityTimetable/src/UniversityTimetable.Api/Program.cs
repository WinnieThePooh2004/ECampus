using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UniversityTimetable.Api.MiddlewareFilters;
using UniversityTimetable.Api.Extensions;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Shared.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
                         ?? throw new InvalidOperationException(
                             "Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddScoped<DbContext, ApplicationDbContext>();

builder.Services.AddControllers(options => { options.Filters.Add<MiddlewareExceptionFilter>(); })
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddDefaultFacades(Assembly.Load("UniversityTimetable.Shared"));
builder.Services.AddDefaultDataServices(Assembly.Load("UniversityTimetable.Shared"));
builder.Services.AddDefaultDomainServices(Assembly.Load("UniversityTimetable.Shared"),
    Assembly.Load("UniversityTimetable.Domain"));
builder.Services.AddSingleItemSelectors(Assembly.Load("UniversityTimetable.Shared"),
    Assembly.Load("UniversityTimetable.Infrastructure"));
builder.Services.AddMultipleDataSelectors(Assembly.Load("UniversityTimetable.Infrastructure"));
builder.Services.DecorateDataServicesWithRelationshipsServices(Assembly.Load("UniversityTimetable.Shared"));
builder.Services.AddDataValidator(Assembly.Load("UniversityTimetable.Infrastructure"));
builder.Services.AddUniqueServices(Assembly.Load("UniversityTimetable.Infrastructure"), Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddValidatorsFromAssembly(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtAuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtAuthOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = JwtAuthOptions.GetSymmetricSecurityKey(),
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

public partial class Program
{
}