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
using UniversityTimetable.Api.MiddlewareFilters;
using UniversityTimetable.Api.Extentions;
using UniversityTimetable.Infrastructure.DataSelectors;

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

builder.Services.AddDefaultRepositoryWithDefaultService<Auditory, AuditoryDTO, AuditoryParameters>();
builder.Services.AddDefaultRepositoryWithDefaultService<Department, DepartmentDTO, DepartmentParameters>();
builder.Services.AddDefaultRepositoryWithDefaultService<Faculty, FacultyDTO, FacultyParameters>();
builder.Services.AddDefaultRepositoryWithDefaultService<Group, GroupDTO, GroupParameters>();

builder.Services.AddScoped<IBaseRepository<Subject>, BaseRepository<Subject>>();
builder.Services.AddScoped<IBaseService<SubjectDTO>, BaseService<SubjectDTO, Subject>>();
builder.Services.AddScoped<IService<SubjectDTO, SubjectParameters>, Service<SubjectDTO, SubjectParameters, Subject>>();
builder.Services.AddScoped<IRepository<Subject, SubjectParameters>, Repository<Subject, SubjectParameters>>();
builder.Services.AddScoped<IRelationshipsRepository<Subject, Teacher, SubjectTeacher>, RelationshipsRepository<Subject, Teacher, SubjectTeacher>>();
builder.Services.Decorate<IBaseRepository<Subject>, BaseSubjectRepository>();

builder.Services.AddScoped<IBaseRepository<Teacher>, BaseRepository<Teacher>>();
builder.Services.AddScoped<IBaseService<TeacherDTO>, BaseService<TeacherDTO, Teacher>>();
builder.Services.AddScoped<IService<TeacherDTO, TeacherParameters>, Service<TeacherDTO, TeacherParameters, Teacher>>();
builder.Services.AddScoped<IRepository<Teacher, TeacherParameters>, Repository<Teacher, TeacherParameters>>();
builder.Services.AddScoped<IRelationshipsRepository<Teacher, Subject, SubjectTeacher>, RelationshipsRepository<Teacher, Subject, SubjectTeacher>>();
builder.Services.Decorate<IBaseRepository<Teacher>, BaseTeacherRepository>();

builder.Services.AddScoped<IBaseService<ClassDTO>, BaseService<ClassDTO, Class>>();
builder.Services.Decorate<IBaseService<ClassDTO>, BaseClassService>();
builder.Services.AddScoped<IBaseRepository<Class>, BaseRepository<Class>>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
