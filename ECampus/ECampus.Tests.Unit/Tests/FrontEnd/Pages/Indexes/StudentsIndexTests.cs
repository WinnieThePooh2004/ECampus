using Bunit;
using ECampus.Domain.Requests.Student;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Student;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Index = ECampus.FrontEnd.Pages.Students.Index;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Pages.Indexes;

public class StudentsIndexTests
{
    private readonly IParametersRequests<MultipleStudentResponse, StudentParameters> _parametersRequests =
        Substitute.For<IParametersRequests<MultipleStudentResponse, StudentParameters>>();

    private readonly IBaseRequests<MultipleStudentResponse> _baseRequests = Substitute.For<IBaseRequests<MultipleStudentResponse>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContent = Substitute.For<HttpContext>();
    private readonly Fixture _fixture = new();
    
    private static readonly IPropertySelector<MultipleStudentResponse> PropertySelector = new PropertySelector<MultipleStudentResponse>();

    private static readonly ISearchTermsSelector<StudentParameters> SearchTermsSelector =
        new SearchTermsSelector<StudentParameters>();

    private readonly TestContext _context = new();

    public StudentsIndexTests()
    {
        _context.Services.AddSingleton(_parametersRequests);
        _context.Services.AddSingleton(_baseRequests);
        _context.Services.AddSingleton(_httpContextAccessor);
        _context.Services.AddSingleton(PropertySelector);
        _context.Services.AddSingleton(SearchTermsSelector);
        _httpContextAccessor.HttpContext.Returns(_httpContent);
    }

    [Fact]
    public async Task Build_ShouldCreateTableHeader_ReadFromAttribute()
    {
        _parametersRequests.GetByParametersAsync(Arg.Any<StudentParameters>()).Returns(TestData(5));

        var page = _context.RenderComponent<Index>(opt => opt
            .Add(i => i.GroupId, 10));
        
        page.Markup.Should().Contain(">First name</th>");
        page.Markup.Should().NotContain(">FirstName</th>");
        await _parametersRequests.Received(1).GetByParametersAsync(Arg.Is<StudentParameters>(s => s.GroupId == 10));
    }
    
    private ListWithPaginationData<MultipleStudentResponse> TestData(int pageSize) =>
        new()
        {
            Metadata = new PaginationData
            {
                PageSize = pageSize,
                PageNumber = 1,
                TotalCount = 10
            },
            Data = _fixture.CreateMany<MultipleStudentResponse>(10).ToList()
        };
}