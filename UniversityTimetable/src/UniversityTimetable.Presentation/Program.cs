using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")
        ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddScoped<IService<FacultacyDTO, FacultacyParameters>, FacultacyService>();
builder.Services.AddScoped<IRepository<Facultacy, FacultacyParameters>, FacultacyRepository>();
builder.Services.AddScoped<IService<DepartmentDTO, DepartmentParameters>, DepartmentService>();
builder.Services.AddScoped<IRepository<Department, DepartmentParameters>, DepartmentRepository>();
builder.Services.AddScoped<IService<AuditoryDTO, AuditoryParameters>, AuditoryService>();
builder.Services.AddScoped<IRepository<Auditory, AuditoryParameters>, AuditoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
