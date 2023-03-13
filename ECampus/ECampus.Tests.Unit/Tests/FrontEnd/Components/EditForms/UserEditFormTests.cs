using Bunit;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Student;
using ECampus.Domain.Requests.Teacher;
using ECampus.Domain.Requests.User;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Student;
using ECampus.Domain.Responses.Teacher;
using ECampus.Domain.Responses.User;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.EditForms;

public class UserEditFormTests
{
    private readonly TestContext _context = new();
    private readonly IBaseRequests<UserDto> _baseRequests = Substitute.For<IBaseRequests<UserDto>>();
    private readonly IValidator<UserDto> _validator = Substitute.For<IValidator<UserDto>>();

    private readonly IParametersRequests<MultipleUserResponse, UserParameters> _parametersRequests =
        Substitute.For<IParametersRequests<MultipleUserResponse, UserParameters>>();

    public UserEditFormTests()
    {
        _context.Services.AddSingleton(_baseRequests);
        _context.Services.AddSingleton(_parametersRequests);
        _context.Services.AddSingleton(typeof(IPropertySelector<>), typeof(PropertySelector<>));
        _context.Services.AddSingleton(typeof(ISearchTermsSelector<>), typeof(SearchTermsSelector<>));
        _context.Services.AddSingleton(_validator);
    }

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Guest)]
    public void Build_ShouldNotShowSelectors_WhenUserRoleIsAdminOrGuest(UserRole role)
    {
        var user = new UserDto { Role = role };

        var component = RenderedComponent(user);
        var markup = component.Markup;

        markup.Should().NotContain("input");
    }

    [Fact]
    public void ClickOnChangeRole_ShouldChangeModelRole()
    {
        var user = new UserDto();
        var component = RenderedComponent(user);
        var select = component.Find("select");

        select.Change(nameof(UserRole.Admin));

        user.Role.Should().Be(UserRole.Admin);
    }

    [Fact]
    public void Build_ShouldShowTeacherSelector_WhenRoleIsTeacher()
    {
        var user = new UserDto { Role = UserRole.Teacher };
        var parametersSelector = Substitute.For<IParametersRequests<MultipleTeacherResponse, TeacherParameters>>();
        _context.Services.AddSingleton(parametersSelector);
        parametersSelector.GetByParametersAsync(Arg.Any<TeacherParameters>())
            .Returns(new ListWithPaginationData<MultipleTeacherResponse>());

        var component = RenderedComponent(user);
        var markup = component.Markup;

        markup.Should().Contain(">Select teacher</h3>");
    }

    [Fact]
    public void Build_ShouldShowStudentSelector_WhenRoleIsStudent()
    {
        var user = new UserDto { Role = UserRole.Student };
        var parametersSelector = Substitute.For<IParametersRequests<MultipleStudentResponse, StudentParameters>>();
        _context.Services.AddSingleton(parametersSelector);
        parametersSelector.GetByParametersAsync(Arg.Any<StudentParameters>())
            .Returns(new ListWithPaginationData<MultipleStudentResponse>());

        var component = RenderedComponent(user);
        var markup = component.Markup;

        markup.Should().Contain(">Select student</h3>");
    }

    [Fact]
    public void ClickOnStudent_ShouldSelectStudent_WhenRoleIsStudent()
    {
        var user = new UserDto { Role = UserRole.Student };
        var parametersSelector = Substitute.For<IParametersRequests<MultipleStudentResponse, StudentParameters>>();
        _context.Services.AddSingleton(parametersSelector);
        parametersSelector.GetByParametersAsync(Arg.Any<StudentParameters>())
            .Returns(new ListWithPaginationData<MultipleStudentResponse>
                { Data = new List<MultipleStudentResponse> { new() { Id = 1 } } });
        var component = RenderedComponent(user);
        var checkbox = component.FindAll("input").Single();

        checkbox.Change(new ChangeEventArgs { Value = true });

        user.StudentId.Should().Be(1);
    }
    
    [Fact]
    public void ClickOnTeacher_ShouldSelectTeacher_WhenRoleIsStudent()
    {
        var user = new UserDto { Role = UserRole.Teacher };
        var parametersSelector = Substitute.For<IParametersRequests<MultipleTeacherResponse, TeacherParameters>>();
        _context.Services.AddSingleton(parametersSelector);
        parametersSelector.GetByParametersAsync(Arg.Any<TeacherParameters>())
            .Returns(new ListWithPaginationData<MultipleTeacherResponse>{Data = new List<MultipleTeacherResponse>{new() {Id = 1}}});
        var component = RenderedComponent(user);
        var checkbox = component.FindAll("input").Single();

        checkbox.Change(new ChangeEventArgs { Value = true });

        user.TeacherId.Should().Be(1);
    }

    private IRenderedComponent<UserRoleEditForm> RenderedComponent(UserDto user)
    {
        var component = _context.RenderComponent<UserRoleEditForm>(opt => opt
            .Add(u => u.Model, user)
            .AddCascadingValue(new EditContext(user)));
        return component;
    }
}