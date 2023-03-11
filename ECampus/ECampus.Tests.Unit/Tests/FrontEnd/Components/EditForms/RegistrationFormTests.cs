using Bunit;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.EditForms;

public class RegistrationFormTests
{
    private readonly TestContext _context = new();
    private readonly IValidator<RegistrationDto> _validator = Substitute.For<IValidator<RegistrationDto>>();
    private bool _onSubmitInvoked;

    public RegistrationFormTests()
    {
        _context.Services.AddSingleton(_validator);
    }

    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormIsInvalid()
    {
        _validator.ValidateAsync(Arg.Any<RegistrationDto>()).Returns(new ValidationResult(
            new[] { new ValidationFailure("Name", "abc") }));
        var form = RenderedComponent();
        var button = form.Find("button");

        button.Click();

        _onSubmitInvoked.Should().BeFalse();
    }
    
    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormInvalid()
    {
        _validator.ValidateAsync(Arg.Any<RegistrationDto>()).Returns(new ValidationResult());
        var form = RenderedComponent();
        var button = form.Find("button");
        
        button.Click();
        
        _onSubmitInvoked.Should().BeTrue();
    }

    private IRenderedComponent<RegistrationsForm> RenderedComponent()
        => _context.RenderComponent<RegistrationsForm>(opt => opt
            .Add(r => r.OnSubmit, InvokeSubmit)
            .Add(r => r.Model, new RegistrationDto()));

    private void InvokeSubmit()
    {
        _onSubmitInvoked = true;
    }
}