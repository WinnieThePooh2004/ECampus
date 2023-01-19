using Bunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using UniversityTimetable.FrontEnd.PropertySelectors;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.QueryParameters;
using Index = UniversityTimetable.FrontEnd.Pages.Students.Index;


namespace UniversityTimetable.Tests.Unit.FrontEnd.Pages.Indexes;

public class StudentsIndexTests
{
    private readonly IParametersRequests<StudentDto, StudentParameters> _parametersRequests =
        Substitute.For<IParametersRequests<StudentDto, StudentParameters>>();

    private readonly IBaseRequests<StudentDto> _baseRequests = Substitute.For<IBaseRequests<StudentDto>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContent = Substitute.For<HttpContext>();
    private readonly Fixture _fixture = new();
    
    private static readonly IPropertySelector<StudentDto> PropertySelector = new PropertySelector<StudentDto>();

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
    public void Build_ShouldCreateTableHeader_ReadFromAttribute()
    {
        _parametersRequests.GetByParametersAsync(Arg.Any<StudentParameters>()).Returns(TestData(5));

        var page = _context.RenderComponent<Index>();
        
        page.Markup.Should().Contain(">First name</th>");
        page.Markup.Should().NotContain(">FirstName</th>");
    }
    
    private ListWithPaginationData<StudentDto> TestData(int pageSize) =>
        new()
        {
            Metadata = new PaginationData
            {
                PageSize = pageSize,
                PageNumber = 1,
                TotalCount = 10
            },
            Data = _fixture.CreateMany<StudentDto>(10).ToList()
        };
}