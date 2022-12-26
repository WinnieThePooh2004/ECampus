using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using UniversityTimetable.Domain.Validation;
using UniversityTimetable.FrontEnd.Extentions;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.FrontEnd.Validation;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<RequestOptions>(builder.Configuration.GetSection("Requests"));

builder.Services.AddRequests<FacultyDto, FacultyParameters>();
builder.Services.AddRequests<GroupDto, GroupParameters>();
builder.Services.AddRequests<DepartmentDto, DepartmentParameters>();
builder.Services.AddRequests<TeacherDto, TeacherParameters>();
builder.Services.AddRequests<SubjectDto, SubjectParameters>();
builder.Services.AddRequests<AuditoryDto, AuditoryParameters>();

builder.Services.AddScoped<IValidator<FacultyDto>, FacultyDtoValidator>();
builder.Services.AddScoped<IValidator<AuditoryDto>, AuditoryDtoValidator>();
builder.Services.AddScoped<IValidator<TeacherDto>, TeacherDtoValidator>();
builder.Services.AddScoped<IValidator<GroupDto>, GroupDtoValidator>();
builder.Services.AddScoped<IValidator<SubjectDto>, SubjectDtoValidator>();
builder.Services.AddScoped<IValidator<DepartmentDto>, DepartmentDtoValidator>();
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();

builder.Services.AddScoped<IValidator<ClassDto>, ClassDtoValidator>();
builder.Services.Decorate<IValidator<ClassDto>, ExtendedClassDTOValidator>();

builder.Services.AddScoped<IClassRequests, ClassRequests>();
builder.Services.AddScoped<IBaseRequests<ClassDto>, BaseRequests<ClassDto>>();

builder.Services.AddScoped<IBaseRequests<UserDto>, BaseRequests<UserDto>>();
builder.Services.AddScoped<IUserRequests, UserRequests>();

builder.Services.AddScoped<IAuthRequests, AuthRequests>();

builder.Services.AddScoped<IUserValidatorFactory, UserValidatorFactory>();

builder.Services.AddSingleton<IRequestOptions>(new RequestOptions(builder.Configuration));

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
    client.BaseAddress = new Uri(builder.Configuration["Api"] ?? throw new Exception("Cannot find section 'Api'"));
});

builder.Services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions{ PropertyNameCaseInsensitive = true});

builder.Services.AddHttpContextAccessor();

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
app.UseCookiePolicy();

app.MapFallbackToPage("/_Host");
app.MapBlazorHub();
app.MapRazorPages();

app.Run();
