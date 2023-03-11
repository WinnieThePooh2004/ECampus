using Bunit;
using ECampus.Domain.DataTransferObjects;
using ECampus.FrontEnd.Pages.AdminPages;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Validation.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Pages.CreatePages;

public class CreateNewUserTests
{
    private readonly TestContext _context = new();
    private readonly IBaseRequests<UserDto> _userRolesRequests = Substitute.For<IBaseRequests<UserDto>>();
    private readonly IUserValidatorFactory _userValidatorFactory = Substitute.For<IUserValidatorFactory>();
    private readonly IValidator<UserDto> _validator = Substitute.For<IValidator<UserDto>>();

    public CreateNewUserTests()
    {
        _userValidatorFactory.UpdateValidator().Returns(_validator);
        _context.Services.AddSingleton(_userRolesRequests);
        _context.Services.AddSingleton(_userValidatorFactory);
    }

    [Fact]
    public void Save_ShouldCallRequests()
    {
        var component = RenderedComponent();
        var saveButton = component.Find("button");
        var passwordChangeLabel = component.FindAll("input")[component.FindAll("input").Count - 1];

        passwordChangeLabel.Change("new password");
        saveButton.Click();
    }

    private IRenderedComponent<CreateNewUser> RenderedComponent()
        => _context.RenderComponent<CreateNewUser>();
}