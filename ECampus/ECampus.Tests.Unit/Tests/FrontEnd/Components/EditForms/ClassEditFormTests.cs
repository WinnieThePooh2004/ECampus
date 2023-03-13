using Bunit;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Auditory;
using ECampus.Domain.Requests.Group;
using ECampus.Domain.Requests.Subject;
using ECampus.Domain.Requests.Teacher;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Auditory;
using ECampus.Domain.Responses.Group;
using ECampus.Domain.Responses.Subject;
using ECampus.Domain.Responses.Teacher;
using ECampus.FrontEnd.Components.EditForms;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.EditForms;

public class ClassEditFormTests
{
    private readonly IParametersRequests<MultipleTeacherResponse, TeacherParameters> _teacherRequests =
        Substitute.For<IParametersRequests<MultipleTeacherResponse, TeacherParameters>>();

    private readonly IParametersRequests<MultipleAuditoryResponse, AuditoryParameters> _auditoriesRequests =
        Substitute.For<IParametersRequests<MultipleAuditoryResponse, AuditoryParameters>>();

    private readonly IParametersRequests<MultipleSubjectResponse, SubjectParameters> _subjectRequests =
        Substitute.For<IParametersRequests<MultipleSubjectResponse, SubjectParameters>>();

    private readonly IParametersRequests<MultipleGroupResponse, GroupParameters> _groupRequests =
        Substitute.For<IParametersRequests<MultipleGroupResponse, GroupParameters>>();

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
            .Returns(new ListWithPaginationData<MultipleAuditoryResponse>());
        _teacherRequests.GetByParametersAsync(Arg.Any<TeacherParameters>())
            .Returns(new ListWithPaginationData<MultipleTeacherResponse>());
        _groupRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(new ListWithPaginationData<MultipleGroupResponse>());
        _subjectRequests.GetByParametersAsync(Arg.Any<SubjectParameters>())
            .Returns(new ListWithPaginationData<MultipleSubjectResponse>());
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