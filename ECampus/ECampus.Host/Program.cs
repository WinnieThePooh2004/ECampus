using ECampus.Core.Extensions;
using ECampus.DataAccess;
using ECampus.Infrastructure;
using ECampus.Services;
using ECampus.WebApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(ServicesAssemblyMarker));
builder.Services.UserInstallersFromAssemblyContaining(builder.Configuration, typeof(ServicesAssemblyMarker),
    typeof(ApiAssemblyMarker), typeof(DataAccessAssemblyMarker),
    typeof(InfrastructureAssemblyMarker), typeof(ServicesAssemblyMarker));

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

namespace ECampus.Host
{
    public abstract class Program
    {
    }
}