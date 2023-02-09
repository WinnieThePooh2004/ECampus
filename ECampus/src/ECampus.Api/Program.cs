using ECampus.Api;
using ECampus.Api.MiddlewareFilters;
using ECampus.Core.Extensions;
using ECampus.Domain;
using ECampus.Infrastructure;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<PrimitiveDataAccessManager>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(ApplicationDbContext.ConnectionKey)!, 
    optionsBuilder => optionsBuilder.MigrationsAssembly("ECampus.Infrastructure")));

builder.Services.AddScoped<DbContext, ApplicationDbContext>();

builder.Services.AddControllers(options => { options.Filters.Add<MiddlewareExceptionFilter>(); })
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(typeof(DomainAssemblyMarker));
builder.Services.UserInstallersFromAssemblyContaining(builder.Configuration, typeof(DomainAssemblyMarker),
    typeof(ApiAssemblyMarker),
    typeof(InfrastructureAssemblyMarker), typeof(ServicesAssemblyMarker));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.UseCors(builder.Configuration["Cors:Name"] ?? throw new Exception("cannot find cors name"));

app.MapControllers();

app.Run();

namespace ECampus.Api
{
    public abstract class Program
    {
    }
}