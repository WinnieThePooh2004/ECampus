using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.FrontEnd.Auth;
using UniversityTimetable.FrontEnd.Extensions;
using UniversityTimetable.FrontEnd.HttpHandlers;
using UniversityTimetable.FrontEnd.PropertySelectors;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Interfaces.Validation;
using UniversityTimetable.FrontEnd.Requests.Options;
using UniversityTimetable.FrontEnd.Requests.ValidationRequests;
using UniversityTimetable.FrontEnd.Validation;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.Configure<RequestOptions>(builder.Configuration.GetSection("Requests"));

builder.Services.AddRequests<FacultyDto, FacultyParameters>();
builder.Services.AddRequests<GroupDto, GroupParameters>();
builder.Services.AddRequests<DepartmentDto, DepartmentParameters>();
builder.Services.AddRequests<TeacherDto, TeacherParameters>();
builder.Services.AddRequests<SubjectDto, SubjectParameters>();
builder.Services.AddRequests<AuditoryDto, AuditoryParameters>();
builder.Services.AddRequests<StudentDto, StudentParameters>();
builder.Services.AddRequests<UserDto, UserParameters>();

builder.Services.AddValidatorsFromAssembly(Assembly.Load("UniversityTimetable.Domain"));

builder.Services.AddScoped<IValidator<ClassDto>, ClassDtoValidator>();
builder.Services.AddScoped<IValidationRequests<ClassDto>, ClassValidationRequests>();
builder.Services.Decorate<IValidator<ClassDto>, HttpCallingValidator<ClassDto>>();

builder.Services.AddScoped<IClassRequests, ClassRequests>();
builder.Services.AddScoped<IBaseRequests<ClassDto>, BaseRequests<ClassDto>>();

builder.Services.AddScoped<IUserRelationshipsRequests, UserRelationshipsRequests>();
builder.Services.AddScoped<IPasswordChangeRequests, PasswordChangeRequests>();

builder.Services.AddScoped<IAuthRequests, AuthRequests>();

builder.Services.AddScoped<IUserValidatorFactory, UserValidatorFactory>();
builder.Services.AddScoped<IUpdateValidationRequests<UserDto>, UserUpdateValidationRequests>();
builder.Services.AddScoped<ICreateValidationRequests<UserDto>, UserCreateValidationRequests>();
builder.Services.Decorate<IValidator<UserDto>, ValidatorWithAnotherTypesIgnore<UserDto>>();

builder.Services.Decorate<IValidator<SubjectDto>, ValidatorWithAnotherTypesIgnore<SubjectDto>>();

builder.Services.AddScoped<IValidator<PasswordChangeDto>, PasswordChangeDtoValidator>();
builder.Services.AddScoped<IValidationRequests<PasswordChangeDto>, PasswordChangeValidationRequests>();
builder.Services.Decorate<IValidator<PasswordChangeDto>, HttpCallingValidator<PasswordChangeDto>>();

builder.Services.AddSingleton<IRequestOptions>(new RequestOptions(builder.Configuration));

builder.Services.AddSingleton(typeof(IPropertySelector<>), typeof(PropertySelector<>));
builder.Services.AddSingleton(typeof(ISearchTermsSelector<>), typeof(SearchTermsSelector<>));

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
}).AddCookie();

builder.Services.AddScoped<TokenHandler>();

builder.Services.AddHttpClient("UTApi", client =>
{
    client.BaseAddress =
        new Uri(builder.Configuration["Api"] ?? throw new Exception("Cannot find section 'Api'"));
}).AddHttpMessageHandler<TokenHandler>();

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