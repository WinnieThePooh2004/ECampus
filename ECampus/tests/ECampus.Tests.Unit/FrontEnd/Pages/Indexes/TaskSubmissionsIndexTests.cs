using Bunit;
using ECampus.FrontEnd.Pages.TaskSubmissions;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Pages.Indexes;

public class TaskSubmissionsIndexTests
{
    private readonly IParametersRequests<TaskSubmissionDto, TaskSubmissionParameters> _parametersRequests =
        Substitute.For<IParametersRequests<TaskSubmissionDto, TaskSubmissionParameters>>();

    private readonly IBaseRequests<TaskSubmissionDto>
        _baseRequests = Substitute.For<IBaseRequests<TaskSubmissionDto>>();

    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContent = Substitute.For<HttpContext>();

    private static readonly IPropertySelector<TaskSubmissionDto> PropertySelector =
        new TaskSubmissionPropertySelector(new PropertySelector<TaskSubmissionDto>());

    private static readonly ISearchTermsSelector<TaskSubmissionParameters> SearchTermsSelector =
        new SearchTermsSelector<TaskSubmissionParameters>();

    private readonly TestContext _context = new();

    public TaskSubmissionsIndexTests()
    {
        _context.Services.AddSingleton(_parametersRequests);
        _context.Services.AddSingleton(_baseRequests);
        _context.Services.AddSingleton(_httpContextAccessor);
        _context.Services.AddSingleton(PropertySelector);
        _context.Services.AddSingleton(SearchTermsSelector);
        _httpContextAccessor.HttpContext.Returns(_httpContent);
    }

    [Fact]
    public void Build_ShouldShouldAdditionalHeadersFromPropertySelector()
    {
        var submission = new TaskSubmissionDto
        {
            CourseTask = new CourseTaskDto { MaxPoints = 20 },
            Student = new StudentDto { LastName = "ln", FirstName = "fn", UserEmail = "email" }
        };
        _parametersRequests.GetByParametersAsync(Arg.Is<TaskSubmissionParameters>(t => t.CourseTaskId == 10))
            .Returns(new ListWithPaginationData<TaskSubmissionDto>
                { Data = new List<TaskSubmissionDto> { submission } });
        
        var component = _context.RenderComponent<TeacherView>(opt => opt
            .Add(t => t.CourseTaskId, 10));
        var markup = component.Markup;

        markup.Should().ContainAll(">Student Name</th>", ">Student email</th>", ">Max points</th>",
            ">20</td>", ">ln fn</td>", ">email</td>");
    }

    [Fact]
    public void Build_ShouldNotBuildAnything_WhenRequestsReturnsNull()
    {
        var page = _context.RenderComponent<TeacherView>();
        page.Markup.Should().Be("<p><em>Loading...</em></p>");
    }
}