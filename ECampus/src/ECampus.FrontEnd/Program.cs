using ECampus.Domain;
using ECampus.FrontEnd;
using ECampus.FrontEnd.HttpHandlers;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Requests.Options;
using ECampus.FrontEnd.Requests.ValidationRequests;
using ECampus.FrontEnd.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<RequestOptions>(builder.Configuration.GetSection("Requests"));

builder.Services.AddUniqueServices(typeof(FrontEndAssemblyMarker));
builder.Services.InstallServices<FrontEndAssemblyMarker>(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<DomainAssemblyMarker>();

builder.Services.Decorate<IValidator<ClassDto>, HttpCallingValidator<ClassDto>>();

builder.Services.AddScoped<IUpdateValidationRequests<UserDto>, UserUpdateValidationRequests>();
builder.Services.AddScoped<ICreateValidationRequests<UserDto>, UserCreateValidationRequests>();
builder.Services.Decorate<IValidator<UserDto>, ValidatorWithAnotherTypesIgnore<UserDto>>();

builder.Services.Decorate<IValidator<SubjectDto>, ValidatorWithAnotherTypesIgnore<SubjectDto>>();
builder.Services.Decorate<IValidator<TeacherDto>, ValidatorWithAnotherTypesIgnore<TeacherDto>>();

builder.Services.Decorate<IValidator<PasswordChangeDto>, HttpCallingValidator<PasswordChangeDto>>();

builder.Services.Decorate<IValidator<CourseDto>, ValidatorWithAnotherTypesIgnore<CourseDto>>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();