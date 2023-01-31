using Bunit;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.FrontEnd.Validation.Interfaces;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Components.EditForms;

public class RegistrationFormTests
{
    private readonly TestContext _context = new();
    private readonly IValidator<UserDto> _validator = Substitute.For<IValidator<UserDto>>();
    private bool _onSubmitInvoked;

    public RegistrationFormTests()
    {
        var validatorFactory = Substitute.For<IUserValidatorFactory>();
        validatorFactory.CreateValidator().Returns(_validator);
        _context.Services.AddSingleton(validatorFactory);
    }

    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormIsInvalid()
    {
        _validator.ValidateAsync(Arg.Any<UserDto>()).Returns(new ValidationResult(
            new[] { new ValidationFailure("Name", "abc") }));
        var form = RenderedComponent();
        var button = form.Find("button");

        button.Click();

        _onSubmitInvoked.Should().BeFalse();
    }
    
    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormInvalid()
    {
        _validator.ValidateAsync(Arg.Any<UserDto>()).Returns(new ValidationResult());
        var form = RenderedComponent();
        var button = form.Find("button");
        
        button.Click();
        
        _onSubmitInvoked.Should().BeTrue();
    }

    private IRenderedComponent<RegistrationsForm> RenderedComponent()
        => _context.RenderComponent<RegistrationsForm>(opt => opt
            .Add(r => r.OnSubmit, InvokeSubmit));

    private void InvokeSubmit()
    {
        _onSubmitInvoked = true;
    }
}