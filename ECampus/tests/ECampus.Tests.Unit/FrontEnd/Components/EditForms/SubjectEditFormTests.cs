using Bunit;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.QueryParameters;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Components.EditForms;

public class SubjectEditFormTests
{
    private readonly TestContext _context = new();

    private readonly IParametersRequests<TeacherDto, TeacherParameters> _requests =
        Substitute.For<IParametersRequests<TeacherDto, TeacherParameters>>();

    private readonly Fixture _fixture = new();

    private readonly IValidator<SubjectDto> _validator = Substitute.For<IValidator<SubjectDto>>();
    
    private static readonly IPropertySelector<TeacherDto> PropertySelector = new PropertySelector<TeacherDto>();

    private static readonly ISearchTermsSelector<TeacherParameters> SearchTermsSelector =
        new SearchTermsSelector<TeacherParameters>();

    public SubjectEditFormTests()
    {
        _context.Services.AddSingleton(_requests);
        _context.Services.AddSingleton(_validator);
        _context.Services.AddSingleton(PropertySelector);
        _context.Services.AddSingleton(SearchTermsSelector);
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