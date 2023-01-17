using System.Security.Claims;
using Bunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.QueryParameters;
using Index = UniversityTimetable.FrontEnd.Pages.Auditories.Index;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Pages.Indexes;

public class AuditoryIndexTests
{
    private readonly IParametersRequests<AuditoryDto, AuditoryParameters> _parametersRequests =
        Substitute.For<IParametersRequests<AuditoryDto, AuditoryParameters>>();

    private readonly IBaseRequests<AuditoryDto> _baseRequests = Substitute.For<IBaseRequests<AuditoryDto>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContent = Substitute.For<HttpContext>();
    private readonly Fixture _fixture = new();

    private readonly TestContext _context = new();

    public AuditoryIndexTests()
    {
        _context.Services.AddSingleton(_parametersRequests);
        _context.Services.AddSingleton(_baseRequests);
        _context.Services.AddSingleton(_httpContextAccessor);
        _httpContextAccessor.HttpContext.Returns(_httpContent);
    }

    [Fact]
    public void Build_ShouldNotBuildAnything_WhenRequestsReturnsNull()
    {
        var page = _context.RenderComponent<Index>();
        page.Markup.Should().Be("<h3>Auditories</h3>\r\n<br>\r\n<p><em>Loading...</em></p>");
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
    public void ChangePlaceSearch_ShouldInvokeRequests_WhenBlur()
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

    private void SetRole(UserRole role)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new NullReferenceException();
        }

        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
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