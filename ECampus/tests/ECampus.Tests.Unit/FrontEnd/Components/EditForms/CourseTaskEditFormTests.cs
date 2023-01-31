using Bunit;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Components.EditForms;

public class CourseTaskEditFormTests
{
    private readonly IValidator<CourseTaskDto> _validator = Substitute.For<IValidator<CourseTaskDto>>();
    private readonly TestContext _context;

    public CourseTaskEditFormTests()
    {
        _context = new TestContext();
        _context.Services.AddSingleton(_validator);
    }

    [Fact]
    public void Render_ShouldShowAllValuesFromTaskType()
    {
        var component = _context.RenderComponent<CourseTaskEditForm>(opt => opt
            .Add(c => c.Model, new CourseTaskDto()));

        foreach (var taskType in Enum.GetValues<TaskType>())
        {
            component.Markup.Should().Contain($">{taskType}</option>");
        }
    }
}