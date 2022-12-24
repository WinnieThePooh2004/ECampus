using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using UniversityTimetable.Api.MiddlewareFilters;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Infrastructure.Auth;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models.RelationModels;
using Newtonsoft.Json;
using UniversityTimetable.Api.Extensions;
using UniversityTimetable.Domain.CreateUpdateValidators;
using UniversityTimetable.Domain.CreateValidators;
using UniversityTimetable.Domain.UpdateValidators;
using UniversityTimetable.Domain.Validation;
using UniversityTimetable.Infrastructure.DataCreate;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Infrastructure.DataUpdate;
using UniversityTimetable.Infrastructure.ValidationRepositories;
using UniversityTimetable.Shared.Interfaces.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
                         ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<MiddlewareExceptionFilter>();
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddValidatorsFromAssembly(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddScoped<IValidator<AuditoryDto>, AuditoryDtoValidator>();
builder.Services.AddScoped<IValidator<ClassDto>, ClassDtoValidator>();
builder.Services.AddScoped<IValidator<DepartmentDto>, DepartmentDtoValidator>();
builder.Services.AddScoped<IValidator<FacultyDto>, FacultyDtoValidator>();
builder.Services.AddScoped<IValidator<GroupDto>, GroupDtoValidator>();
builder.Services.AddScoped<IValidator<SubjectDto>, SubjectDtoValidator>();
builder.Services.AddScoped<IValidator<TeacherDto>, TeacherDtoValidator>();

builder.Services.AddDefaultDomainServices<AuditoryDto>();
builder.Services.AddDefaultDomainServices<DepartmentDto>();
builder.Services.AddDefaultDomainServices<FacultyDto>();
builder.Services.AddDefaultDomainServices<GroupDto>();
builder.Services.AddDefaultDomainServices<SubjectDto>();
builder.Services.AddDefaultDomainServices<TeacherDto>();
builder.Services.AddDefaultDomainServices<UserDto>();

builder.Services.AddScoped<IValidationRepository<User>, UserValidationRepository>();
builder.Services.Decorate<IUpdateValidator<UserDto>, UserUpdateValidator>();
builder.Services.Decorate<ICreateValidator<UserDto>, UserCreateValidator>();

builder.Services.AddScoped<IValidationRepository<Class>, ClassValidationRepository>();
builder.Services.AddScoped<IUpdateValidator<ClassDto>, ClassDtoCreateUpdateValidator>();
builder.Services.AddScoped<ICreateValidator<ClassDto>, ClassDtoCreateUpdateValidator>();

builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddMultipleDataSelector<Auditory, AuditoryParameters, AuditorySelector>();
builder.Services.AddMultipleDataSelector<Department, DepartmentParameters, DepartmentSelector>();
builder.Services.AddMultipleDataSelector<Faculty, FacultyParameters, FacultySelector>();
builder.Services.AddMultipleDataSelector<Group, GroupParameters, GroupSelector>();
builder.Services.AddMultipleDataSelector<Subject, SubjectParameters, SubjectSelector>();
builder.Services.AddMultipleDataSelector<Teacher, TeacherParameters, TeacherSelector>();

builder.Services.AddDefaultDataServices<Auditory>();
builder.Services.AddDefaultDataServices<Department>();
builder.Services.AddDefaultDataServices<Faculty>();
builder.Services.AddDefaultDataServices<Group>();
builder.Services.AddDefaultDataServices<Class>();

builder.Services.AddDefaultDataServices<Subject>();
builder.Services.Decorate<IDataUpdate<Subject>, SubjectUpdate>();
builder.Services.Decorate<IDataCreate<Subject>, SubjectCreate>();

builder.Services.AddDefaultDataServices<Teacher>();
builder.Services.Decorate<IDataUpdate<Teacher>, TeacherUpdate>();
builder.Services.Decorate<IDataCreate<Teacher>, TeacherCreate>();

builder.Services.AddDefaultDataServices<User>();
builder.Services.Decorate<IDataUpdate<User>, UserUpdate>();
builder.Services.Decorate<IDataCreate<User>, UserCreate>();

builder.Services.AddScoped<ISingleItemSelector<User>, SingleUserSelector>();
builder.Services.AddScoped<ISingleItemSelector<Subject>, SingleSubjectSelector>();
builder.Services.AddScoped<ISingleItemSelector<Teacher>, SingleTeacherSelector>();

builder.Services.AddDefaultFacadeServices<Auditory, AuditoryDto, AuditoryParameters>();
builder.Services.AddDefaultFacadeServices<Department, DepartmentDto, DepartmentParameters>();
builder.Services.AddDefaultFacadeServices<Faculty, FacultyDto, FacultyParameters>();
builder.Services.AddDefaultFacadeServices<Group, GroupDto, GroupParameters>();
builder.Services.AddDefaultFacadeServices<Subject, SubjectDto, SubjectParameters>();
builder.Services.AddDefaultFacadeServices<Teacher, TeacherDto, TeacherParameters>();

builder.Services.AddScoped<IRelationshipsRepository<Subject, Teacher, SubjectTeacher>, RelationshipsRepository<Subject, Teacher, SubjectTeacher>>();

builder.Services.AddScoped<IRelationshipsRepository<Teacher, Subject, SubjectTeacher>, RelationshipsRepository<Teacher, Subject, SubjectTeacher>>();

builder.Services.AddScoped<IBaseService<ClassDto>, BaseService<ClassDto, Class>>();
builder.Services.AddScoped<IBaseRepository<Class>, BaseRepository<Class>>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();

builder.Services.AddScoped<IRelationshipsRepository<User, Auditory, UserAuditory>, RelationshipsRepository<User, Auditory, UserAuditory>>();
builder.Services.AddScoped<IRelationshipsRepository<User, Group, UserGroup>, RelationshipsRepository<User, Group, UserGroup>>();
builder.Services.AddScoped<IRelationshipsRepository<User, Teacher, UserTeacher>, RelationshipsRepository<User, Teacher, UserTeacher>>();
builder.Services.AddScoped<IBaseService<UserDto>, BaseService<UserDto, User>>();
builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

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

app.UseCookiePolicy(new CookiePolicyOptions{ MinimumSameSitePolicy = SameSiteMode.Strict });
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
