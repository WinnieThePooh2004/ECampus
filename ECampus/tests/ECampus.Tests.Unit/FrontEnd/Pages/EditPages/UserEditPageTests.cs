using Bunit;
using ECampus.FrontEnd.Pages.AdminPages;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Pages.EditPages;

public class UserEditPageTests
{
    private readonly TestContext _context = new();
    private readonly IBaseRequests<UserDto> _requests = Substitute.For<IBaseRequests<UserDto>>();

    public UserEditPageTests()
    {
        _context.Services.AddSingleton(_requests);
    }

    [Fact]
    public void Build_ShouldShowStudentDetails_IfUserRoleIsStudent()
    {
        var user = new UserDto { Student = new StudentDto { FirstName = "fn", LastName = "ln", UserEmail = "email" } };
        _requests.GetByIdAsync(1).Returns(user);
        _context.Services.AddSingleton<IPropertySelector<StudentDto>, PropertySelector<StudentDto>>();

        var component = RenderComponent(1);
        var markup = component.Markup;

        markup.Should().ContainAll(">First name: fn</label>", ">Last name: ln</label>",
            "This user is student with next details:", ">Email: email</label>");
    }
    
    [Fact]
    public void Build_ShouldShowTeacherDetails_IfUserRoleIsTeacher()
    {
        var user = new UserDto { Teacher = new TeacherDto { FirstName = "fn", LastName = "ln", UserEmail = "email" } };
        _requests.GetByIdAsync(1).Returns(user);
        _context.Services.AddSingleton<IPropertySelector<TeacherDto>, PropertySelector<TeacherDto>>();

        var component = RenderComponent(1);
        var markup = component.Markup;

        markup.Should().ContainAll(">First name: fn</label>", ">Last name: ln</label>",
            "This user is teacher with next details:", ">Email: email</label>");
    }

    [Fact]
    public async Task ClickOnSave_ShouldCallRequests()
    {
        var user = new UserDto();
        _requests.GetByIdAsync(1).Returns(user);
        _context.Services.AddSingleton<IPropertySelector<TeacherDto>, PropertySelector<TeacherDto>>();
        var component = RenderComponent(1);
        var save = component.Find("button");

        save.Click();

        await _requests.Received(1).UpdateAsync(user);
    }

    [Fact]
    public void Build_ShouldShowLoading_WhenNotUserReturnedFromRequests()
    {
        var component = RenderComponent(0);

        component.Markup.Should().Be("<p><em>Loading...</em></p>");
    }

    private IRenderedComponent<EditUserRole> RenderComponent(int id)
        => _context.RenderComponent<EditUserRole>(opt => opt
            .Add(e => e.Id, id));
}