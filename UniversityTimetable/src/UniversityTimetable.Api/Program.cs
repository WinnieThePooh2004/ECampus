using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using UniversityTimetable.Api.MiddlewareFilters;
using UniversityTimetable.Api.Extensions;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Domain.Validation;
using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.Domain.Validation.CreateValidators;
using UniversityTimetable.Domain.Validation.UniversalValidators;
using UniversityTimetable.Domain.Validation.UpdateValidators;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Auth;
using UniversityTimetable.Infrastructure.DataCreateServices;
using UniversityTimetable.Infrastructure.DataUpdateServices;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Infrastructure.ValidationDataAccess;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models.RelationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
                         ?? throw new InvalidOperationException(
                             "Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddScoped<DbContext, ApplicationDbContext>();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<MiddlewareExceptionFilter>();
    })
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<IValidator<AuditoryDto>, AuditoryDtoValidator>();
builder.Services.AddScoped<IValidator<ClassDto>, ClassDtoValidator>();
builder.Services.AddScoped<IValidator<DepartmentDto>, DepartmentDtoValidator>();
builder.Services.AddScoped<IValidator<FacultyDto>, FacultyDtoValidator>();
builder.Services.AddScoped<IValidator<GroupDto>, GroupDtoValidator>();
builder.Services.AddScoped<IValidator<SubjectDto>, SubjectDtoValidator>();
builder.Services.AddScoped<IValidator<TeacherDto>, TeacherDtoValidator>();
builder.Services.AddScoped<IValidator<PasswordChangeDto>, PasswordChangeDtoValidator>();
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();

builder.Services.AddScoped(typeof(IValidationFacade<>), typeof(ValidationFacade<>));

builder.Services.AddDefaultDomainServices<AuditoryDto>();
builder.Services.AddDefaultDomainServices<DepartmentDto>();
builder.Services.AddDefaultDomainServices<FacultyDto>();
builder.Services.AddDefaultDomainServices<GroupDto>();
builder.Services.AddDefaultDomainServices<SubjectDto>();
builder.Services.AddDefaultDomainServices<TeacherDto>();
builder.Services.AddDefaultDomainServices<UserDto>();

builder.Services.AddScoped<IDataValidator<User>, UserDataValidator>();
builder.Services.Decorate<IUpdateValidator<UserDto>, UserUpdateValidator>();
builder.Services.Decorate<ICreateValidator<UserDto>, UserCreateValidator>();

builder.Services.AddScoped<IUpdateValidator<PasswordChangeDto>, UpdateValidator<PasswordChangeDto>>();
builder.Services.AddScoped<IValidationDataAccess<User>, UserDataValidator>();
builder.Services.Decorate<IUpdateValidator<PasswordChangeDto>, PasswordChangeDtoUpdateValidator>();

builder.Services.AddScoped<IValidationDataAccess<Class>, ClassValidationDataAccess>();
builder.Services.AddScoped<IUpdateValidator<ClassDto>, ClassDtoUniversalValidator>();
builder.Services.AddScoped<ICreateValidator<ClassDto>, ClassDtoUniversalValidator>();

builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddSingleton(typeof(IRelationshipsDataAccess<,,>), typeof(RelationshipsDataAccess<,,>));
builder.Services.AddSingleton<IRelationsDataAccess, RelationsDataAccess>();

builder.Services.AddMultipleDataSelector<Auditory, AuditoryParameters, MultipleAuditorySelector>();
builder.Services.AddMultipleDataSelector<Department, DepartmentParameters, MultipleDepartmentSelector>();
builder.Services.AddMultipleDataSelector<Faculty, FacultyParameters, MultipleFacultySelector>();
builder.Services.AddMultipleDataSelector<Group, GroupParameters, MultipleGroupSelector>();
builder.Services.AddMultipleDataSelector<Subject, SubjectParameters, MultipleSubjectSelector>();
builder.Services.AddMultipleDataSelector<Teacher, TeacherParameters, MultipleTeacherSelector>();

builder.Services.AddDefaultDataServices<Auditory>();
builder.Services.AddDefaultDataServices<Department>();
builder.Services.AddDefaultDataServices<Faculty>();
builder.Services.AddDefaultDataServices<Group>();
builder.Services.AddDefaultDataServices<Class>();

builder.Services.AddDefaultDataServices<Subject>();
builder.Services.Decorate<IDataUpdateService<Subject>, DataUpdateServiceWithRelationships<Subject, Teacher, SubjectTeacher>>();
builder.Services.Decorate<IDataCreateService<Subject>, DataCreateServiceWithRelationships<Subject, Teacher, SubjectTeacher>>();

builder.Services.AddDefaultDataServices<Teacher>();
builder.Services.Decorate<IDataUpdateService<Teacher>, DataUpdateServiceWithRelationships<Teacher, Subject, SubjectTeacher>>();
builder.Services.Decorate<IDataCreateService<Teacher>, DataCreateServiceWithRelationships<Teacher, Subject, SubjectTeacher>>();

builder.Services.AddDefaultDataServices<User>();
builder.Services.Decorate<IDataUpdateService<User>, DataUpdateServiceWithRelationships<User, Auditory, UserAuditory>>();
builder.Services.Decorate<IDataUpdateService<User>, DataUpdateServiceWithRelationships<User, Group, UserGroup>>();
builder.Services.Decorate<IDataUpdateService<User>, DataUpdateServiceWithRelationships<User, Teacher, UserTeacher>>();

builder.Services.Decorate<IDataCreateService<User>, DataCreateServiceWithRelationships<User, Auditory, UserAuditory>>();
builder.Services.Decorate<IDataCreateService<User>, DataCreateServiceWithRelationships<User, Group, UserGroup>>();
builder.Services.Decorate<IDataCreateService<User>, DataCreateServiceWithRelationships<User, Teacher, UserTeacher>>();

builder.Services.AddSingleton<ISingleItemSelector<User>, SingleUserSelector>();
builder.Services.AddSingleton<ISingleItemSelector<Subject>, SingleSubjectSelector>();
builder.Services.AddSingleton<ISingleItemSelector<Teacher>, SingleTeacherSelector>();

builder.Services.AddDefaultFacadeServices<Auditory, AuditoryDto, AuditoryParameters>();
builder.Services.AddDefaultFacadeServices<Department, DepartmentDto, DepartmentParameters>();
builder.Services.AddDefaultFacadeServices<Faculty, FacultyDto, FacultyParameters>();
builder.Services.AddDefaultFacadeServices<Group, GroupDto, GroupParameters>();
builder.Services.AddDefaultFacadeServices<Subject, SubjectDto, SubjectParameters>();
builder.Services.AddDefaultFacadeServices<Teacher, TeacherDto, TeacherParameters>();

builder.Services.AddScoped<IBaseService<ClassDto>, BaseService<ClassDto, Class>>();
builder.Services.AddScoped<IBaseDataAccessFacade<Class>, BaseDataAccessFacade<Class>>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ITimetableDataAccessFacade, TimetableDataAccessFacade>();

builder.Services.AddScoped<IBaseService<UserDto>, BaseService<UserDto, User>>();
builder.Services.AddScoped<IBaseDataAccessFacade<User>, BaseDataAccessFacade<User>>();
builder.Services.AddScoped<IUserDataAccessFacade, UserDataAccessFacade>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordChange, PasswordChange>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IAuthorizationDataAccess, AuthorizationDataAccess>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.Headers["Locations"] = context.RedirectUri;
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Headers["Locations"] = context.RedirectUri;
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
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