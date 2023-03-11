using System.Security.Claims;
using Bunit;
using ECampus.Domain.DataContainers;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.QueryParameters;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Index = ECampus.FrontEnd.Pages.Auditories.Index;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Pages.Indexes;

public class AuditoryIndexTests
{
    private readonly IParametersRequests<AuditoryDto, AuditoryParameters> _parametersRequests =
        Substitute.For<IParametersRequests<AuditoryDto, AuditoryParameters>>();

    private readonly IBaseRequests<AuditoryDto> _baseRequests = Substitute.For<IBaseRequests<AuditoryDto>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContent = Substitute.For<HttpContext>();
    private readonly Fixture _fixture = new();

    private static readonly IPropertySelector<AuditoryDto> PropertySelector = new PropertySelector<AuditoryDto>();

    private static readonly ISearchTermsSelector<AuditoryParameters> SearchTermsSelector =
        new SearchTermsSelector<AuditoryParameters>();

    private readonly TestContext _context = new();

    public AuditoryIndexTests()
    {
        _context.Services.AddSingleton(_parametersRequests);
        _context.Services.AddSingleton(_baseRequests);
        _context.Services.AddSingleton(_httpContextAccessor);
        _context.Services.AddSingleton(PropertySelector);
        _context.Services.AddSingleton(SearchTermsSelector);
        _httpContextAccessor.HttpContext.Returns(_httpContent);
    }

    [Fact]
    public void Build_ShouldNotBuildAnything_WhenRequestsReturnsNull()
    {
        var page = _context.RenderComponent<Index>();
        page.Markup.Should().Be("<h3>Auditories</h3>\r\n<br>\r\n<p><em>Loading...</em></p>");
    }

    [Fact]
    public void Build_ShouldCreateCellsFromPropertySelectors()
    {
        var data = TestData(5);
        _parametersRequests.GetByParametersAsync(Arg.Any<AuditoryParameters>()).Returns(data);
        
        var page = _context.RenderComponent<Index>();
        var markup = page.Markup;

        markup.Should().ContainAll(">Name</th>", ">Building</th>", "placeholder=\"AuditoryName\"", "placeholder=\"BuildingName\"");
    }

    [Theory]
    [InlineData(UserRole.Guest)]
    [InlineData(UserRole.Student)]
    [InlineData(UserRole.Teacher)]
    public void Build_ShouldNotShowEditCreateAndDeleteButtons_WhenRoleIsNotAdmin(UserRole role)
    {
        SetRole(role);
        var data = TestData(5);
        _parametersRequests.GetByParametersAsync(Arg.Any<AuditoryParameters>()).Returns(data);
        var page = _context.RenderComponent<Index>();

        var markup = page.Markup;
        markup.Should().NotContainAll(">Create new</a>", ">Edit</a>", ">Delete</a>");
    }

    [Fact]
    public void Build_ShouldShowEditCreateAndDeleteButtons_WhenRoleIsAdmin()
    {
        SetRole(UserRole.Admin);
        var data = TestData(5);
        _parametersRequests.GetByParametersAsync(Arg.Any<AuditoryParameters>()).Returns(data);
        var page = _context.RenderComponent<Index>();

        var markup = page.Markup;
        markup.Should().ContainAll(">Create new</a>", ">Edit</a>", ">Delete</a>");
    }

    [Fact]
    public async Task ClickOnDelete_ShouldCallRequests()
    {
        SetRole(UserRole.Admin);
        var data = TestData(5);
        _parametersRequests.GetByParametersAsync(Arg.Any<AuditoryParameters>()).Returns(data);
        var page = _context.RenderComponent<Index>();

        var delete = page.FindAll("a").First(a => a.ToMarkup().Contains("Delete"));
        delete.Click();

        await _baseRequests.Received(1).DeleteAsync(data.Data[0].Id);
    }

    [Fact]
    public void ChangeSearch_ShouldInvokeRequests_WhenBlur()
    {
        SetRole(UserRole.Admin);
        var data = TestData(5);
        AuditoryParameters? parameters = null;
        _parametersRequests.GetByParametersAsync(Arg.Do<AuditoryParameters>(p => parameters = p)).Returns(data);
        
        var page = _context.RenderComponent<Index>();
        var searchTerm = page.Find("input");

        searchTerm.Change("abc");
        searchTerm.Blur();

        parameters?.AuditoryName.Should().Be("abc");
    }

    [Fact]
    public void ChangePageNumber_ShouldInvokeRequests()
    {
        SetRole(UserRole.Admin);
        var data = TestData(5);
        AuditoryParameters? parameters = null;
        _parametersRequests.GetByParametersAsync(Arg.Do<AuditoryParameters>(p => parameters = p)).Returns(data);
        var page = _context.RenderComponent<Index>();
        var pageSelect = page.FindAll("a").First(a => a.ToMarkup().Contains(">2<"));

        pageSelect.Click();

        parameters?.PageNumber.Should().Be(2);
    }

    [Fact]
    public void ChangePageSize_ShouldInvokeRequests()
    {
        SetRole(UserRole.Admin);
        var data = TestData(5);
        AuditoryParameters? parameters = null;
        _parametersRequests.GetByParametersAsync(Arg.Do<AuditoryParameters>(p => parameters = p)).Returns(data);
        var page = _context.RenderComponent<Index>();
        var pageSizeSelect = page.Find("select");

        pageSizeSelect.Change("10");

        parameters?.PageSize.Should().Be(10);
    }

    [Fact]
    public void ClickOnTableHeader_ShouldChangeOrderBy()
    {
        SetRole(UserRole.Admin);
        var data = TestData(5);
        AuditoryParameters? parameters = null;
        _parametersRequests.GetByParametersAsync(Arg.Do<AuditoryParameters>(p => parameters = p)).Returns(data);

        var page = _context.RenderComponent<Index>();
        var nameHeader = page.FindAll("th").Single(th => th.ToMarkup().ToLower().Contains("name"));
        nameHeader.Click();

        parameters.Should().NotBeNull();
        parameters?.OrderBy.Should().Be("Name");
        parameters?.SortOrder.Should().Be(SortOrder.Ascending);
    }
    
    [Fact]
    public void ClickOnTableHeader_ShouldChangeOrderByAndSortOrder_WhenClickedTwice()
    {
        SetRole(UserRole.Admin);
        var data = TestData(5);
        AuditoryParameters? parameters = null;
        _parametersRequests.GetByParametersAsync(Arg.Do<AuditoryParameters>(p => parameters = p)).Returns(data);

        var page = _context.RenderComponent<Index>();
        var nameHeader = page.FindAll("th").Single(th => th.ToMarkup().ToLower().Contains("name"));
        nameHeader.Click();
        nameHeader = page.FindAll("th").Single(th => th.ToMarkup().ToLower().Contains("name"));
        nameHeader.Click();

        parameters.Should().NotBeNull();
        parameters?.OrderBy.Should().Be("Name");
        parameters?.SortOrder.Should().Be(SortOrder.Descending);
    }

    private void SetRole(UserRole role)
    {
        _httpContextAccessor.HttpContext!.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Role, role.ToString())
        }));
    }

    private ListWithPaginationData<AuditoryDto> TestData(int pageSize) =>
        new()
        {
            Metadata = new PaginationData
            {
                PageSize = pageSize,
                PageNumber = 1,
                TotalCount = 10
            },
            Data = _fixture.CreateMany<AuditoryDto>(10).ToList()
        };
}