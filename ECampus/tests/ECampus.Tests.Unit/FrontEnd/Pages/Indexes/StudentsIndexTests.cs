using Bunit;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Index = ECampus.FrontEnd.Pages.Students.Index;

namespace ECampus.Tests.Unit.FrontEnd.Pages.Indexes;

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