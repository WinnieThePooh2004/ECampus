using Bunit;
using ECampus.Domain.Requests.Group;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Group;
using ECampus.FrontEnd.Components.DataSelectors;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.ItemSelect;

public class SingleItemSelectTests
{
    private readonly TestContext _context = new();

    private readonly IParametersRequests<MultipleGroupResponse, GroupParameters> _parametersRequests =
        Substitute.For<IParametersRequests<MultipleGroupResponse, GroupParameters>>();
    
    private static readonly IPropertySelector<MultipleGroupResponse> PropertySelector = new PropertySelector<MultipleGroupResponse>();

    private static readonly ISearchTermsSelector<GroupParameters> SearchTermsSelector =
        new SearchTermsSelector<GroupParameters>();


    private readonly Fixture _fixture = new();

    public SingleItemSelectTests()
    {
        _context.Services.AddSingleton(_parametersRequests);
        _context.Services.AddSingleton(PropertySelector);
        _context.Services.AddSingleton(SearchTermsSelector);
    }

    [Fact]
    public void Build_ShouldHaveH3Tag_WhenTitleIsNotNullOrEmpty()
    {
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>())
            .Returns(new ListWithPaginationData<MultipleGroupResponse>());
        var component = RenderSelector(0, "title");

        var title = component.Find("h3");
        title.ToMarkup().Should().Contain("title");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Build_ShouldNotHaveH3Tag_WhenTitleIsNullOrEmpty(string title)
    {
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>())
            .Returns(new ListWithPaginationData<MultipleGroupResponse>());

        RenderSelector(0, title).FindAll("h3").Should().BeEmpty();
    }

    [Fact]
    public void Build_ShouldNowBuildTable_WhenRequestsReturnsNull()
    {
        RenderSelector(0).Markup.Should().Be("<p><em>Loading...</em></p>");
    }

    [Fact]
    public void SelectItem_ShouldInvokeSelectChange_WhenArgIsTrue()
    {
        var select = 0;
        var data = new ListWithPaginationData<MultipleGroupResponse>
        {
            Metadata = new PaginationData { TotalCount = 5, PageNumber = 1, PageSize = 5 },
            Data = _fixture.CreateMany<MultipleGroupResponse>(5).ToList()
        };
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(data);
        var selector = RenderSelector(0, g => select = g);

        var checkbox = selector.Find("input");
        checkbox.Change(new ChangeEventArgs { Value = true });

        select.Should().Be(data.Data[0].Id);
    }

    [Fact]
    public void SelectItem_ShouldIgnore_WhenArgIsFalse()
    {
        int? select = null;
        var data = new ListWithPaginationData<MultipleGroupResponse>
        {
            Metadata = new PaginationData { TotalCount = 5, PageNumber = 1, PageSize = 5 },
            Data = _fixture.CreateMany<MultipleGroupResponse>(5).ToList()
        };
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(data);
        var selector = RenderSelector(0, g => select = g);

        var checkbox = selector.Find("input");
        checkbox.Change(new ChangeEventArgs { Value = false });

        select.Should().BeNull();
        checkbox = selector.Find("input");
        
        checkbox.Change(new ChangeEventArgs { Value = true });
        select.Should().NotBeNull();
    }

    [Fact]
    public void SelectItem_ShouldChangeSelectedItem_WhenCurrentIsNotNull()
    {
        var select = 0;
        var data = new ListWithPaginationData<MultipleGroupResponse>
        {
            Metadata = new PaginationData { TotalCount = 5, PageNumber = 1, PageSize = 5 },
            Data = _fixture.CreateMany<MultipleGroupResponse>(5).ToList()
        };
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(data);
        var selector = RenderSelector(0, g => select = g);

        var checkboxes = selector.FindAll("input").ToList();
        checkboxes.Count.Should().Be(5);
        for (var i = 0; i < 5; ++i)
        {
            selector.FindAll("input").ToList()[i].Change(new ChangeEventArgs { Value = true });

            select.Should().Be(data.Data[i].Id);
        }
    }
    
    [Fact]
    public async Task ClickOnTableHeader_ShouldChangeOrderBy()
    {
        var items = Enumerable.Range(0, 10).Select(i => _fixture
            .Build<MultipleGroupResponse>().With(f => f.Id, i).Create()).ToList();
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>())
            .Returns(new ListWithPaginationData<MultipleGroupResponse>
                { Data = items, Metadata = new PaginationData { TotalCount = 10, PageNumber = 1, PageSize = 5 } });
        var selector = RenderSelector(1);
        var th = selector.Find("th");
        _parametersRequests.ClearReceivedCalls();

        th.Click();

        await _parametersRequests.Received(1).GetByParametersAsync(Arg.Any<GroupParameters>());
    }

    private IRenderedComponent<SingleItemSelect<MultipleGroupResponse, GroupParameters>> RenderSelector(int selectedId,
        Action<int> selectChanged, string title = "")
    {
        return _context.RenderComponent<SingleItemSelect<MultipleGroupResponse, GroupParameters>>(options =>
            options.Add(s => s.SelectedId, selectedId)
                .Add(s => s.Title, title)
                .Add(s => s.SelectedIdChanged, selectChanged));
    }
    
    private IRenderedComponent<SingleItemSelect<MultipleGroupResponse, GroupParameters>> RenderSelector(int selectedId, string title = "")
    {
        return _context.RenderComponent<SingleItemSelect<MultipleGroupResponse, GroupParameters>>(options =>
            options.Add(s => s.SelectedId, selectedId)
                .Add(s => s.Title, title));
    }
}