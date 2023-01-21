using Bunit;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Components.EditForms;

public class AuditoryEditFormTests
{
    private readonly IValidator<AuditoryDto> _validator = Substitute.For<IValidator<AuditoryDto>>();
    private readonly TestContext _context;

    public AuditoryEditFormTests()
    {
        _context = new TestContext();
        _context.Services.AddSingleton(_validator);
    }

    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormIsInvalid()
    {
        var onSubmittedInvoked = false;
        var model = new AuditoryDto();
        _validator.ValidateAsync(model).Returns(new ValidationResult(new []{new ValidationFailure("Name", "abc")}));
        var form = _context
            .RenderComponent<AuditoryEditForm>(parameters => parameters
            .Add(form => form.Model, model)
            .Add(form => form.OnSubmit, _ => onSubmittedInvoked = true));
        var button = form.Find("button");
        
        button.Click();
        
        onSubmittedInvoked.Should().BeFalse();
    }
    
    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormInvalid()
    {
        var onSubmittedInvoked = false;
        var model = new AuditoryDto();
        _validator.ValidateAsync(model).Returns(new ValidationResult());
        var form = _context
            .RenderComponent<AuditoryEditForm>(parameters => parameters
                .Add(form => form.Model, model)
                .Add(form => form.OnSubmit, _ => onSubmittedInvoked = true));
        var button = form.Find("button");
        
        button.Click();
        
        onSubmittedInvoked.Should().BeTrue();
    }
}