using Bunit;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using UniversityTimetable.FrontEnd.Components.EditForms;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Components.EditForms;

public class SubjectEditFormTests
{
    private readonly TestContext _context = new();

    private readonly IParametersRequests<TeacherDto, TeacherParameters> _requests =
        Substitute.For<IParametersRequests<TeacherDto, TeacherParameters>>();

    private readonly Fixture _fixture = new();

    private readonly IValidator<SubjectDto> _validator = Substitute.For<IValidator<SubjectDto>>();

    public SubjectEditFormTests()
    {
        _context.Services.AddSingleton(_requests);
        _context.Services.AddSingleton(_validator);
    }

    [Fact]
    public void ClickOnTeacher_ShouldAddItToModel()
    {
        var model = new SubjectDto { Teachers = new List<TeacherDto>() };
        var teachers = _fixture.Build<TeacherDto>().Without(t => t.Subjects).CreateMany(5).ToList();
        _requests.GetByParametersAsync(Arg.Any<TeacherParameters>())
            .Returns(new ListWithPaginationData<TeacherDto> { Data = teachers });
        _validator.ValidateAsync(Arg.Any<IValidationContext>()).Returns(new ValidationResult());
        var form = RenderedComponent(model);
        var checkbox = form.FindAll("input").First(i => i.ToMarkup().Contains("form-check"));

        checkbox.Change(true);
        model.Teachers.Should().Contain(teachers[0]);
    }

    private IRenderedComponent<SubjectEditForm> RenderedComponent(SubjectDto model)
    {
        return _context.RenderComponent<SubjectEditForm>(options => options
            .Add(f => f.Model, model));
    }
}