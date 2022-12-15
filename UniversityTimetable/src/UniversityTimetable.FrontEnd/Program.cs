using MudBlazor.Services;
using UniversityTimetable.FrontEnd.Extentions;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Requests.Options;

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

builder.Services.AddScoped<IClassRequests, ClassRequests>();
builder.Services.AddScoped<IBaseRequests<ClassDTO>, ClassRequests>();

builder.Services.AddMudServices();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
