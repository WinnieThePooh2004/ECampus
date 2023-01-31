using Bunit;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Components.EditForms;

public class ClassEditFormTests
{
    private readonly IParametersRequests<TeacherDto, TeacherParameters> _teacherRequests =
        Substitute.For<IParametersRequests<TeacherDto, TeacherParameters>>();

    private readonly IParametersRequests<AuditoryDto, AuditoryParameters> _auditoriesRequests =
        Substitute.For<IParametersRequests<AuditoryDto, AuditoryParameters>>();

    private readonly IParametersRequests<SubjectDto, SubjectParameters> _subjectRequests =
        Substitute.For<IParametersRequests<SubjectDto, SubjectParameters>>();

    private readonly IParametersRequests<GroupDto, GroupParameters> _groupRequests =
        Substitute.For<IParametersRequests<GroupDto, GroupParameters>>();

    private readonly TestContext _context = new();
    private readonly IValidator<ClassDto> _validator = Substitute.For<IValidator<ClassDto>>();
    private readonly IBaseRequests<ClassDto> _requests = Substitute.For<IBaseRequests<ClassDto>>();

    public ClassEditFormTests()
    {
        _context.Services.AddSingleton(_subjectRequests);
        _context.Services.AddSingleton(_teacherRequests);
        _context.Services.AddSingleton(_groupRequests);
        _context.Services.AddSingleton(_auditoriesRequests);
        _context.Services.AddSingleton(_validator);
        _context.Services.AddSingleton(_requests);
        _context.Services.AddSingleton(typeof(IPropertySelector<>), typeof(PropertySelector<>));
        _context.Services.AddSingleton(typeof(ISearchTermsSelector<>), typeof(SearchTermsSelector<>));
        _auditoriesRequests.GetByParametersAsync(Arg.Any<AuditoryParameters>())
            .Returns(new ListWithPaginationData<AuditoryDto>());
        _teacherRequests.GetByParametersAsync(Arg.Any<TeacherParameters>())
            .Returns(new ListWithPaginationData<TeacherDto>());
        _groupRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(new ListWithPaginationData<GroupDto>());
        _subjectRequests.GetByParametersAsync(Arg.Any<SubjectParameters>())
            .Returns(new ListWithPaginationData<SubjectDto>());
    }

    [Theory]
    [InlineData(TimetableMode.Auditory, "Select auditory")]
    [InlineData(TimetableMode.Teacher, "Select teacher")]
    [InlineData(TimetableMode.Group, "Select group")]
    public void Render_ShouldIgnoreOneOfSelectors_WhenItsModeProvided(TimetableMode currentMode,
        string ignoredSelectorTitle)
    {
        var allTitles = new List<string> { "Select teacher", "Select auditory", "Select group" };
        allTitles.Remove(ignoredSelectorTitle);
        var component = _context.RenderComponent<ClassEditForm>(opt => opt
            .Add(c => c.TimetableMode, currentMode)
            .Add(c => c.Model, new ClassDto()));

        var markup = component.Markup;

        markup.Should().ContainAll(allTitles);
        markup.Should().NotContain(ignoredSelectorTitle);
    }
    
    [Fact]
    public void Render_ShouldShowAllValuesRelatedEnums()
    {
        var component = _context.RenderComponent<ClassEditForm>(opt => opt
            .Add(c => c.Model, new ClassDto()));

        foreach (var weekDependency in Enum.GetValues<WeekDependency>())
        {
            component.Markup.Should().Contain($">{weekDependency}</option>");
        }
        
        foreach (var classType in Enum.GetValues<ClassType>())
        {
            component.Markup.Should().Contain($">{classType}</option>");
        }
    }
}