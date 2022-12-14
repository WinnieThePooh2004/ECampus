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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
        ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddScoped<IService<FacultyDTO, FacultyParameters>, FacultyService>();
builder.Services.AddScoped<IRepository<Faculty, FacultyParameters>, FacultyRepository>();
builder.Services.AddScoped<IService<DepartmentDTO, DepartmentParameters>, DepartmentService>();
builder.Services.AddScoped<IRepository<Department, DepartmentParameters>, DepartmentRepository>();
builder.Services.AddScoped<IService<AuditoryDTO, AuditoryParameters>, AuditoryService>();
builder.Services.AddScoped<IRepository<Auditory, AuditoryParameters>, AuditoryRepository>();
builder.Services.AddScoped<IService<GroupDTO, GroupParameters>, GroupService>();
builder.Services.AddScoped<IRepository<Group, GroupParameters>, GroupRepository>();
builder.Services.AddScoped<IService<TeacherDTO, TeacherParameters>, TeacherService>();
builder.Services.AddScoped<IRepository<Teacher, TeacherParameters>, TeacherRepository>();
builder.Services.AddScoped<IService<SubjectDTO, SubjectParameters>, SubjectService>();
builder.Services.AddScoped<IRepository<Subject, SubjectParameters>, SubjectRepository>();
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
