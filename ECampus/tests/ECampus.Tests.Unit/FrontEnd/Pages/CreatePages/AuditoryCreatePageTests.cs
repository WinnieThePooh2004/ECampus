using Bunit;
using ECampus.FrontEnd.Pages.Auditories;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Pages.CreatePages;

public class AuditoryCreatePageTests
{
    private readonly TestContext _context = new();
    private readonly IBaseRequests<AuditoryDto> _baseRequests = Substitute.For<IBaseRequests<AuditoryDto>>();
    private readonly IValidator<AuditoryDto> _validator = Substitute.For<IValidator<AuditoryDto>>();

    public AuditoryCreatePageTests()
    {
        _context.Services.AddSingleton(_baseRequests);
        _context.Services.AddSingleton(_validator);
    }

    [Fact]
    public async Task Build_ShouldSendRequest_WhenClickedOnSaveButton()
    {
        _validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        _validator.ValidateAsync(Arg.Any<AuditoryDto>()).Returns(new ValidationResult());

        var component = RenderedComponent();
        var saveButton = component.Find("button");

        saveButton.Click();
        await _baseRequests.Received().CreateAsync(Arg.Any<AuditoryDto>());
    }

    private IRenderedComponent<Create> RenderedComponent() =>
        _context.RenderComponent<Create>();
}