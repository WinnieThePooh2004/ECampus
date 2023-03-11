using System.Security.Claims;
using System.Security.Principal;
using Bunit;
using ECampus.FrontEnd.Pages.Timetable;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TimetableData = ECampus.Shared.DataContainers.Timetable;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Pages.Timetable;

public class AuditoryTimetablePageTests
{
    private readonly TestContext _context = new();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IBaseRequests<UserProfile> _userRequests = Substitute.For<IBaseRequests<UserProfile>>();
    private readonly IClassRequests _classRequests = Substitute.For<IClassRequests>();
    private readonly IUserRelationshipsRequests _relationshipsRequests = Substitute.For<IUserRelationshipsRequests>();
    private readonly IBaseRequests<ClassDto> _baseClassRequests = Substitute.For<IBaseRequests<ClassDto>>();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();

    public AuditoryTimetablePageTests()
    {
        _context.Services.AddSingleton(_httpContextAccessor);
        _context.Services.AddSingleton(_userRequests);
        _context.Services.AddSingleton(_relationshipsRequests);
        _context.Services.AddSingleton(_classRequests);
        _context.Services.AddSingleton(_baseClassRequests);
        _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
        _httpContextAccessor.HttpContext.User = _user;
    }

    [Fact]
    public void Render_ShouldShowNotBuildPage_WhenRequestsReturnsNull()
    {
        var component = RenderedComponent(10);

        component.Markup.Should().Be("<p><em>Loading...</em></p>\r\n\r\n<br>");
    }

    [Fact]
    public void Render_ShouldShowCorrectTimetable_WhenRequestsReturnValue()
    {
        _classRequests.AuditoryTimetable(10).Returns(CreateTimetable());

        var component = RenderedComponent(10);
        var markup = component.Markup;

        markup.Should().Contain("s1 Lecture");
        markup.Should().Contain("Group g1");
        markup.Should().Contain("Teacher: t1fn t1ln");
    }

    [Fact]
    public async Task ClickOnDelete_ShouldCallRequests()
    {
        _classRequests.AuditoryTimetable(10).Returns(CreateTimetable());
        var component = RenderedComponent(10);
        var deleteButton = component.Find("button");

        deleteButton.Click();

        await _baseClassRequests.Received().DeleteAsync(1);
    }

    [Fact]
    public async Task ClickOnDelete_ShouldCallRequests_WhenClickedOnLastFoundButton()
    {
        _classRequests.AuditoryTimetable(10).Returns(CreateTimetable());
        var component = RenderedComponent(10);
        var deleteButton = component.FindAll("button")[component.FindAll("button").Count - 1];

        deleteButton.Click();

        await _baseClassRequests.Received().DeleteAsync(3);
    }

    [Fact]
    public async Task ClickOnDelete_ShouldCallRequests_WhenClickedOnPreLastFoundButton()
    {
        _classRequests.AuditoryTimetable(10).Returns(CreateTimetable());
        var component = RenderedComponent(10);
        var deleteButton = component.FindAll("button")[component.FindAll("button").Count - 2];

        deleteButton.Click();

        await _baseClassRequests.Received().DeleteAsync(4);
    }

    [Fact]
    public async Task Render_ShouldShowButtonSave_WhenCurrentUserDoesNotHaveSaveAuditory()
    {
        _user.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "10"));
        var user = new UserProfile { SavedAuditories = new List<AuditoryDto>() };
        _user.Identity.Returns(Substitute.For<IIdentity>());
        _user.Identity!.IsAuthenticated.Returns(true);
        _userRequests.GetByIdAsync(10).Returns(user);
        var component = RenderedComponent(11);
        var saveButton = component.Find("a");

        saveButton.Click();

        await _relationshipsRequests.Received().SaveAuditory(11);
    }

    [Fact]
    public async Task Render_ShouldShowButtonRemoveSave_WhenCurrentUserDoesNotHaveSaveAuditory()
    {
        _user.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "10"));
        var user = new UserProfile { SavedAuditories = new List<AuditoryDto> { new() { Id = 11 } } };
        _user.Identity.Returns(Substitute.For<IIdentity>());
        _user.Identity!.IsAuthenticated.Returns(true);
        _userRequests.GetByIdAsync(10).Returns(user);
        var component = RenderedComponent(11);
        var saveButton = component.Find("a");

        saveButton.Click();

        await _relationshipsRequests.Received().RemoveSavedAuditory(11);
    }

    private IRenderedComponent<Auditory> RenderedComponent(int auditoryId)
        => _context.RenderComponent<Auditory>(opt => opt
            .Add(a => a.AuditoryId, auditoryId));

    private static TimetableData CreateTimetable() =>
        new(new List<ClassDto>
        {
            new()
            {
                Teacher = new TeacherDto { FirstName = "t1fn", LastName = "t1ln" },
                Subject = new SubjectDto { Name = "s1" }, Id = 1,
                DayOfWeek = 1, WeekDependency = WeekDependency.None, ClassType = ClassType.Lecture,
                Group = new GroupDto { Name = "g1" }
            },
            new()
            {
                Teacher = new TeacherDto { FirstName = "t3fn", LastName = "t3ln" },
                Subject = new SubjectDto { Name = "s1" }, ClassType = ClassType.Lab,
                DayOfWeek = 3, WeekDependency = WeekDependency.AppearsOnOddWeeks,
                Group = new GroupDto { Name = "g2" }, Id = 2
            },
            new()
            {
                Teacher = new TeacherDto { FirstName = "t3fn", LastName = "t3ln" },
                Subject = new SubjectDto { Name = "s1" }, ClassType = ClassType.Lab,
                DayOfWeek = 4, WeekDependency = WeekDependency.AppearsOnOddWeeks,
                Group = new GroupDto { Name = "g2" }, Id = 5
            },
            new()
            {
                Teacher = new TeacherDto { FirstName = "t3fn", LastName = "t3ln" },
                Subject = new SubjectDto { Name = "s1" }, ClassType = ClassType.Lab,
                DayOfWeek = 4, WeekDependency = WeekDependency.AppearsOnEvenWeeks,
                Group = new GroupDto { Name = "g2" }, Id = 4
            },
            new()
            {
                Teacher = new TeacherDto { FirstName = "t2fn", LastName = "t2ln" },
                Subject = new SubjectDto { Name = "s2" }, ClassType = ClassType.Practical,
                DayOfWeek = 5, WeekDependency = WeekDependency.AppearsOnEvenWeeks,
                Group = new GroupDto { Name = "g2" }, Id = 3
            }
        })
        {
            Auditory = new AuditoryDto { Name = "auditory name", Building = "building name" }
        };
}