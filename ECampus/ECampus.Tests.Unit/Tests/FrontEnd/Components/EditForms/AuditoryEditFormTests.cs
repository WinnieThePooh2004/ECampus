using Bunit;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.EditForms;

public class AuditoryEditFormTests
{
    private readonly IValidator<AuditoryDto> _validator = Substitute.For<IValidator<AuditoryDto>>();
    private readonly TestContext _context;
    private bool _onSubmitedInvoked;

    public AuditoryEditFormTests()
    {
        _context = new TestContext();
        _context.Services.AddSingleton(_validator);
    }

    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormIsInvalid()
    {
        var model = new AuditoryDto();
        _validator.ValidateAsync(model).Returns(new ValidationResult(new []{new ValidationFailure("Name", "abc")}));
        var form = _context
            .RenderComponent<AuditoryEditForm>(parameters => parameters
            .Add(form => form.Model, model)
            .Add(form => form.OnSubmit, Submit));
        var button = form.Find("button");
        
        button.Click();
        
        _onSubmitedInvoked.Should().BeFalse();
    }
    
    [Fact]
    public void SubmitForm_ShouldNotInvokeOnSubmit_WhenFormInvalid()
    {
        var model = new AuditoryDto();
        _validator.ValidateAsync(model).Returns(new ValidationResult());
        var form = _context
            .RenderComponent<AuditoryEditForm>(parameters => parameters
                .Add(form => form.Model, model)
                .Add(form => form.OnSubmit, Submit));
        var button = form.Find("button");
        
        button.Click();
        
        _onSubmitedInvoked.Should().BeTrue();
    }

    private void Submit()
    {
        _onSubmitedInvoked = true;
    }
}