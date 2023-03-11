using Bunit;
using ECampus.FrontEnd.Pages.Auditories;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Pages.EditPages;

public class AuditoryEditPageTests
{
    private readonly TestContext _context = new();
    private readonly IBaseRequests<AuditoryDto> _baseRequests = Substitute.For<IBaseRequests<AuditoryDto>>();
    private readonly IValidator<AuditoryDto> _validator = Substitute.For<IValidator<AuditoryDto>>();

    public AuditoryEditPageTests()
    {
        _context.Services.AddSingleton(_validator);
        _context.Services.AddSingleton(_baseRequests);
    }

    [Fact]
    public void Build_ShouldNotBuildPage_WhenRequestsReturnsNull()
    {
        var component = RenderedComponent(10);

        component.Markup.Should().Be("<p><em>Loading...</em></p>");
    }

    [Fact]
    public async Task Build_ShouldSendRequest_WhenClickedOnSaveButton()
    {
        var auditory = new AuditoryDto();
        _validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        _validator.ValidateAsync(auditory).Returns(new ValidationResult());
        _baseRequests.GetByIdAsync(10).Returns(auditory);

        var component = RenderedComponent(10);
        var saveButton = component.Find("button");

        saveButton.Click();
        await _baseRequests.Received().UpdateAsync(auditory);
    }

    private IRenderedComponent<Edit> RenderedComponent(int id) =>
        _context.RenderComponent<Edit>(opt => opt
            .Add(e => e.Id, id));
}