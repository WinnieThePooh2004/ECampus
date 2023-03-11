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

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.EditForms;

public class TeacherEditFormTests
{
    private readonly TestContext _context = new();

    private readonly IParametersRequests<SubjectDto, SubjectParameters> _requests =
        Substitute.For<IParametersRequests<SubjectDto, SubjectParameters>>();

    private readonly Fixture _fixture = new();

    private readonly IValidator<TeacherDto> _validator = Substitute.For<IValidator<TeacherDto>>();
    
    private static readonly IPropertySelector<SubjectDto> PropertySelector = new PropertySelector<SubjectDto>();

    private static readonly ISearchTermsSelector<SubjectParameters> SearchTermsSelector =
        new SearchTermsSelector<SubjectParameters>();

    public TeacherEditFormTests()
    {
        _context.Services.AddSingleton(_requests);
        _context.Services.AddSingleton(_validator);
        _context.Services.AddSingleton(PropertySelector);
        _context.Services.AddSingleton(SearchTermsSelector);
    }

    [Fact]
    public void ClickOnSubject_ShouldAddItToModel()
    {
        var model = new TeacherDto { Subjects = new List<SubjectDto>() };
        var teachers = _fixture.Build<SubjectDto>().Without(t => t.Teachers).CreateMany(5).ToList();
        _requests.GetByParametersAsync(Arg.Any<SubjectParameters>())
            .Returns(new ListWithPaginationData<SubjectDto> { Data = teachers });
        _validator.ValidateAsync(Arg.Any<IValidationContext>()).Returns(new ValidationResult());
        var form = RenderedComponent(model);
        var checkbox = form.FindAll("input").First(i => i.ToMarkup().Contains("form-check"));

        checkbox.Change(true);
        model.Subjects.Should().Contain(teachers[0]);
    }

    private IRenderedComponent<TeacherEditForm> RenderedComponent(TeacherDto model)
    {
        return _context.RenderComponent<TeacherEditForm>(options => options
            .Add(f => f.Model, model));
    }
}