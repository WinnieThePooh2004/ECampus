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
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using UniversityTimetable.Api.MiddlewareFilters;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Infrastructure.Auth;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models.RelationModels;
using Newtonsoft.Json;
using UniversityTimetable.Api.Extensions;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Shared.Interfaces.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<MiddlewareExceptionFilter>();
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
        ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddMultipleDataSelector<Auditory, AuditoryParameters, AuditorySelector>();
builder.Services.AddMultipleDataSelector<Department, DepartmentParameters, DepartmentSelector>();
builder.Services.AddMultipleDataSelector<Faculty, FacultyParameters, FacultySelector>();
builder.Services.AddMultipleDataSelector<Group, GroupParameters, GroupSelector>();
builder.Services.AddMultipleDataSelector<Subject, SubjectParameters, SubjectSelector>();
builder.Services.AddMultipleDataSelector<Teacher, TeacherParameters, TeacherSelector>();

builder.Services.AddDefaultSingleItemSelector<Auditory>();
builder.Services.AddDefaultSingleItemSelector<Department>();
builder.Services.AddDefaultSingleItemSelector<Faculty>();
builder.Services.AddDefaultSingleItemSelector<Group>();
builder.Services.AddDefaultSingleItemSelector<Class>();

builder.Services.AddScoped<ISingleItemSelector<User>, SingleUserSelector>();
builder.Services.AddScoped<ISingleItemSelector<Subject>, SingleSubjectSelector>();
builder.Services.AddScoped<ISingleItemSelector<Teacher>, SingleTeacherSelector>();

builder.Services.AddDefaultServices<Auditory, AuditoryDto, AuditoryParameters>();
builder.Services.AddDefaultServices<Department, DepartmentDTO, DepartmentParameters>();
builder.Services.AddDefaultServices<Faculty, FacultyDto, FacultyParameters>();
builder.Services.AddDefaultServices<Group, GroupDto, GroupParameters>();
builder.Services.AddDefaultServices<Subject, SubjectDto, SubjectParameters>();
builder.Services.AddDefaultServices<Teacher, TeacherDto, TeacherParameters>();

builder.Services.AddScoped<IRelationshipsRepository<Subject, Teacher, SubjectTeacher>, RelationshipsRepository<Subject, Teacher, SubjectTeacher>>();
builder.Services.Decorate<IBaseService<SubjectDto>, BaseSubjectService>();

builder.Services.AddScoped<IRelationshipsRepository<Teacher, Subject, SubjectTeacher>, RelationshipsRepository<Teacher, Subject, SubjectTeacher>>();
builder.Services.Decorate<IBaseService<TeacherDto>, BaseTeacherService>();

builder.Services.AddScoped<IBaseService<ClassDto>, BaseService<ClassDto, Class>>();
builder.Services.Decorate<IBaseService<ClassDto>, BaseClassService>();
builder.Services.AddScoped<IBaseRepository<Class>, BaseRepository<Class>>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();

builder.Services.AddScoped<IRelationshipsRepository<User, Auditory, UserAuditory>, RelationshipsRepository<User, Auditory, UserAuditory>>();
builder.Services.AddScoped<IRelationshipsRepository<User, Group, UserGroup>, RelationshipsRepository<User, Group, UserGroup>>();
builder.Services.AddScoped<IRelationshipsRepository<User, Teacher, UserTeacher>, RelationshipsRepository<User, Teacher, UserTeacher>>();
builder.Services.AddScoped<IBaseService<UserDto>, BaseService<UserDto, User>>();
builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
