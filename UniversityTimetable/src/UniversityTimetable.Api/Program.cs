using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UniversityTimetable.Api.MiddlewareFilters;
using UniversityTimetable.Api.Extensions;
using UniversityTimetable.Domain.Validation.CreateValidators;
using UniversityTimetable.Domain.Validation.UniversalValidators;
using UniversityTimetable.Domain.Validation.UpdateValidators;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
                         ?? throw new InvalidOperationException(
                             "Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddScoped<DbContext, ApplicationDbContext>();

builder.Services.AddControllers(options => { options.Filters.Add<MiddlewareExceptionFilter>(); })
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddValidatorsFromAssembly(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddDefaultFacades(Assembly.Load("UniversityTimetable.Shared"));
builder.Services.AddDefaultDataServices(Assembly.Load("UniversityTimetable.Shared"));
builder.Services.AddSingleItemSelectors(Assembly.Load("UniversityTimetable.Shared"),
    Assembly.Load("UniversityTimetable.Infrastructure"));
builder.Services.AddMultipleDataSelectors(Assembly.Load("UniversityTimetable.Infrastructure"));
builder.Services.DecorateDataServicesWithRelationshipsServices(Assembly.Load("UniversityTimetable.Shared"));
builder.Services.AddDataValidator(Assembly.Load("UniversityTimetable.Infrastructure"));
builder.Services.AddUniqueServices(Assembly.Load("UniversityTimetable.Infrastructure"),
    Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddFluentValidationWrappers<AuditoryDto>();
builder.Services.AddFluentValidationWrappers<ClassDto>();
builder.Services.AddFluentValidationWrappers<GroupDto>();
builder.Services.AddFluentValidationWrappers<FacultyDto>();
builder.Services.AddFluentValidationWrappers<DepartmentDto>();
builder.Services.AddFluentValidationWrappers<SubjectDto>();
builder.Services.AddFluentValidationWrappers<StudentDto>();
builder.Services.AddFluentValidationWrappers<TeacherDto>();
builder.Services.AddFluentValidationWrappers<UserDto>();

builder.Services.Decorate<IUpdateValidator<UserDto>, UserUpdateValidator>();
builder.Services.Decorate<ICreateValidator<UserDto>, UserCreateValidator>();

builder.Services.Decorate<IUpdateValidator<ClassDto>, ClassDtoUniversalValidator>();
builder.Services.Decorate<ICreateValidator<ClassDto>, ClassDtoUniversalValidator>();

builder.Services.AddScoped<IUpdateValidator<PasswordChangeDto>, FluentValidatorWrapper<PasswordChangeDto>>();
builder.Services.Decorate<IUpdateValidator<PasswordChangeDto>, PasswordChangeDtoUpdateValidator>();

builder.Services.AddSingleton(typeof(IRelationshipsHandler<,,>),typeof(RelationshipsHandler<,,>));

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