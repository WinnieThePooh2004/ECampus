using System.Reflection;
using ECampus.Api.Extensions;
using ECampus.Api.MiddlewareFilters;
using ECampus.Domain.Validation.CreateValidators;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Infrastructure;
using ECampus.Infrastructure.Relationships;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Domain.Validation;
using FluentValidation;
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

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
                         ?? throw new InvalidOperationException(
                             "Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddScoped<DbContext, ApplicationDbContext>();

builder.Services.AddControllers(options => { options.Filters.Add<MiddlewareExceptionFilter>(); })
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddValidatorsFromAssembly(Assembly.Load("ECampus.Domain"));

builder.Services.AddAutoMapper(Assembly.Load("ECampus.Domain"));

builder.Services.AddDefaultFacades(Assembly.Load("ECampus.Shared"));
builder.Services.AddDefaultDataServices(Assembly.Load("ECampus.Shared"));
builder.Services.AddSingleItemSelectors(Assembly.Load("ECampus.Shared"),
    Assembly.Load("ECampus.Infrastructure"));
builder.Services.AddMultipleDataSelectors(Assembly.Load("ECampus.Infrastructure"));
builder.Services.DecorateDataServicesWithRelationshipsServices(Assembly.Load("ECampus.Shared"));
builder.Services.AddDataValidator(Assembly.Load("ECampus.Infrastructure"));
builder.Services.AddUniqueServices(Assembly.Load("ECampus.Infrastructure"),
    Assembly.Load("ECampus.Domain"));

builder.Services.AddFluentValidationWrappers<AuditoryDto>();
builder.Services.AddFluentValidationWrappers<ClassDto>();
builder.Services.AddFluentValidationWrappers<GroupDto>();
builder.Services.AddFluentValidationWrappers<FacultyDto>();
builder.Services.AddFluentValidationWrappers<DepartmentDto>();
builder.Services.AddFluentValidationWrappers<SubjectDto>();
builder.Services.AddFluentValidationWrappers<StudentDto>();
builder.Services.AddFluentValidationWrappers<TeacherDto>();
builder.Services.AddFluentValidationWrappers<UserDto>();
builder.Services.AddFluentValidationWrappers<CourseDto>();
builder.Services.AddFluentValidationWrappers<CourseTaskDto>();

builder.Services.Decorate<IUpdateValidator<UserDto>, UserUpdateValidator>();
builder.Services.Decorate<ICreateValidator<UserDto>, UserCreateValidator>();

builder.Services.Decorate<IUpdateValidator<ClassDto>, ClassDtoUniversalValidator>();
builder.Services.Decorate<ICreateValidator<ClassDto>, ClassDtoUniversalValidator>();

builder.Services.AddScoped<IUpdateValidator<PasswordChangeDto>, FluentValidatorWrapper<PasswordChangeDto>>();
builder.Services.Decorate<IUpdateValidator<PasswordChangeDto>, PasswordChangeDtoUpdateValidator>();

builder.Services.AddSingleton(typeof(IRelationshipsHandler<,,>), typeof(RelationshipsHandler<,,>));

builder.Services.AddSingleton(typeof(IRelationsDataAccess<,,>), typeof(RelationsDataAccess<,,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authOptions = builder.Configuration.GetSection("jwtAuthOptions")
    .Get<JwtAuthOptions>() ?? throw new Exception("Cannot find section 'jwtAuthOptions'");

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

public partial class Program
{
}