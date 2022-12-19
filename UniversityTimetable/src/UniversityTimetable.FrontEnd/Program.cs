using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using UniversityTimetable.Domain.Validation;
using UniversityTimetable.FrontEnd.Extentions;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.FrontEnd.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<Requests>(builder.Configuration.GetSection("Requests"));

builder.Services.AddRequests<FacultyDTO, FacultyParameters>();
builder.Services.AddRequests<GroupDTO, GroupParameters>();
builder.Services.AddRequests<DepartmentDTO, DepartmentParameters>();
builder.Services.AddRequests<TeacherDTO, TeacherParameters>();
builder.Services.AddRequests<SubjectDTO, SubjectParameters>();
builder.Services.AddRequests<AuditoryDTO, AuditoryParameters>();

builder.Services.AddScoped<IValidator<FacultyDTO>, FacultyDTOValidator>();
builder.Services.AddScoped<IValidator<AuditoryDTO>, AuditoryDTOValidator>();
builder.Services.AddScoped<IValidator<TeacherDTO>, TeacherDTOValidator>();
builder.Services.AddScoped<IValidator<GroupDTO>, GroupDTOValidator>();
builder.Services.AddScoped<IValidator<SubjectDTO>, SubjectDTOValidator>();
builder.Services.AddScoped<IValidator<DepartmentDTO>, DepartmentDTOValidator>();

builder.Services.AddScoped<IValidator<ClassDTO>, ClassDTOValidator>();
builder.Services.Decorate<IValidator<ClassDTO>, ExtendedClassDTOValidator>();

builder.Services.AddScoped<IClassRequests, ClassRequests>();
builder.Services.AddScoped<IBaseRequests<ClassDTO>, BaseRequests<ClassDTO>>();

builder.Services.AddMudServices();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services.AddHttpClient("UTApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7219/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
