using Bunit;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Requests.Faculty;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Faculty;
using ECampus.FrontEnd.Components.DataSelectors;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.ItemSelect;

public class MultipleItemSelectTests
{
    private static readonly IPropertySelector<MultipleFacultyResponse> PropertySelector = 
        new PropertySelector<MultipleFacultyResponse>();

    private static readonly ISearchTermsSelector<FacultyParameters> SearchTermsSelector =
        new SearchTermsSelector<FacultyParameters>();


    private readonly TestContext _context = new();

    private readonly IParametersRequests<MultipleFacultyResponse, FacultyParameters> _parametersRequests =
        Substitute.For<IParametersRequests<MultipleFacultyResponse, FacultyParameters>>();

    private readonly Fixture _fixture = new();
    private bool _onChangedInvoked;
    
    public MultipleItemSelectTests()
    {
        _context.Services.AddSingleton(_parametersRequests);
        _context.Services.AddSingleton(PropertySelector);
        _context.Services.AddSingleton(SearchTermsSelector);
    }

    [Fact]
    public void CheckItem_ShouldAddItemToList_IfItemWasNotChecked()
    {
        var items = Enumerable.Range(0, 10).Select(i => _fixture
            .Build<MultipleFacultyResponse>().With(f => f.Id, i).Create()).ToList();
        var selectTo = new List<MultipleFacultyResponse>();
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<MultipleFacultyResponse> { Data = items });

        var selector = RenderSelector(selectTo);

        var checkbox = selector.FindAll("input").First(input => input.ToMarkup().Contains("type=\"checkbox\""));
        checkbox.Change(new ChangeEventArgs { Value = true });
        selectTo.Count.Should().Be(1);
        _onChangedInvoked.Should().Be(true);
    }

    [Fact]
    public async Task CheckItem_ShouldRemoveItemToList_IfItemWasChecked()
    {
        var items = Enumerable.Range(0, 10).Select(i => _fixture
            .Build<MultipleFacultyResponse>().With(f => f.Id, i).Create()).ToList();
        var selectTo = new List<MultipleFacultyResponse> { new() { Id = 0 } };
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<MultipleFacultyResponse> { Data = items });

        var selector = RenderSelector(selectTo);

        var checkbox = selector.FindAll("input").First(input => input.ToMarkup().Contains("type=\"checkbox\""));
        await checkbox.ChangeAsync(new ChangeEventArgs { Value = false });
        _onChangedInvoked.Should().Be(true);
        selectTo.Count.Should().Be(0);
    }

    [Fact]
    public void Select_ShouldSaveSelectTo_WhenPageNumberChanged()
    {
        var items = Enumerable.Range(0, 10).Select(i => _fixture
            .Build<MultipleFacultyResponse>().With(f => f.Id, i).Create()).ToList();
        var selectTo = new List<MultipleFacultyResponse> { new() { Id = 0 } };
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<MultipleFacultyResponse>
                { Data = items, Metadata = new PaginationData { TotalCount = 10, PageNumber = 1, PageSize = 5 } });

        var selector = RenderSelector(items);

        var secondPage = selector.FindAll("a").First(a => a.ToMarkup().Contains(">2<"));
        secondPage.Click();
        var firstPage = selector.FindAll("a").First(a => a.ToMarkup().Contains(">1<"));
        firstPage.Click();
        selectTo.Count.Should().Be(1);
    }

    [Fact]
    public void Build_ShouldHaveH3Tag_WhenTitleIsNotNullOrEmpty()
    {
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<MultipleFacultyResponse>());
        var component = RenderSelector(new List<MultipleFacultyResponse>(), "title");

        var title = component.Find("h3");
        title.ToMarkup().Should().Contain("title");
    }

    [Fact]
    public async Task ClickOnTableHeader_ShouldChangeOrderBy()
    {
        var items = Enumerable.Range(0, 10).Select(i => _fixture
            .Build<MultipleFacultyResponse>().With(f => f.Id, i).Create()).ToList();
        var selectTo = new List<MultipleFacultyResponse> { new() { Id = 0 } };
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<MultipleFacultyResponse>
                { Data = items, Metadata = new PaginationData { TotalCount = 10, PageNumber = 1, PageSize = 5 } });
        var selector = RenderSelector(selectTo);
        var th = selector.Find("th");
        _parametersRequests.ClearReceivedCalls();

        th.Click();

        await _parametersRequests.Received(1).GetByParametersAsync(Arg.Any<FacultyParameters>());
    }

    [Fact]
    public void Build_ShouldNotBuildTable_WhenRequestsReturnsNull()
    {
        RenderSelector(new List<MultipleFacultyResponse>()).Markup.Should().Be("<p><em>Loading...</em></p>");
    }


    private IRenderedComponent<MultipleItemsSelect<MultipleFacultyResponse, FacultyParameters>> RenderSelector(
        List<MultipleFacultyResponse> selectTo, string title = "")
    {
        return _context.RenderComponent<MultipleItemsSelect<MultipleFacultyResponse, FacultyParameters>>(
            parameters => parameters
                .Add(s => s.SelectTo, selectTo)
                .Add(s => s.Title, title)
                .Add(s => s.OnChanged,
                    () => _onChangedInvoked = true));
    }
}