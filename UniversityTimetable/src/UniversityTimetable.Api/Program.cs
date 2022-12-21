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
using UniversityTimetable.Api.Extentions;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Infrastructure.Auth;
using UniversityTimetable.Infrastructure.DataSelectors;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models.RelationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<MiddlewareExceptionFilter>();
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
        ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddDataSelector<Auditory, AuditoryParameters, AuditorySelector>();
builder.Services.AddDataSelector<Department, DepartmentParameters, DepartmentSelector>();
builder.Services.AddDataSelector<Faculty, FacultyParameters, FacultySelector>();
builder.Services.AddDataSelector<Group, GroupParameters, GroupSelector>();
builder.Services.AddDataSelector<Subject, SubjectParameters, SubjectSelector>();
builder.Services.AddDataSelector<Teacher, TeacherParameters, TeacherSelector>();

builder.Services.AddDefaultServices<Auditory, AuditoryDto, AuditoryParameters>();
builder.Services.AddDefaultServices<Department, DepartmentDTO, DepartmentParameters>();
builder.Services.AddDefaultServices<Faculty, FacultyDto, FacultyParameters>();
builder.Services.AddDefaultServices<Group, GroupDto, GroupParameters>();
builder.Services.AddDefaultServices<Subject, SubjectDto, SubjectParameters>();
builder.Services.AddDefaultServices<Teacher, TeacherDto, TeacherParameters>();

builder.Services.AddScoped<IRelationshipsRepository<Subject, Teacher, SubjectTeacher>, RelationshipsRepository<Subject, Teacher, SubjectTeacher>>();
builder.Services.Decorate<IBaseRepository<Subject>, BaseSubjectRepository>();

builder.Services.AddScoped<IRelationshipsRepository<Teacher, Subject, SubjectTeacher>, RelationshipsRepository<Teacher, Subject, SubjectTeacher>>();
builder.Services.Decorate<IBaseRepository<Teacher>, BaseTeacherRepository>();

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
builder.Services.Decorate<IBaseRepository<User>, BaseUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();
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
