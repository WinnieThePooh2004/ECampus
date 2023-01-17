using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using UniversityTimetable.FrontEnd.Components.DataSelectors;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Components.ItemSelect;

public class SingleItemSelectTests
{
    private readonly TestContext _context = new();

    private readonly IParametersRequests<GroupDto, GroupParameters> _parametersRequests =
        Substitute.For<IParametersRequests<GroupDto, GroupParameters>>();

    private readonly Fixture _fixture = new();

    public SingleItemSelectTests()
    {
        _context.Services.AddSingleton(_parametersRequests);
    }

    [Fact]
    public void Build_ShouldHaveH3Tag_WhenTitleIsNotNullOrEmpty()
    {
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>())
            .Returns(new ListWithPaginationData<GroupDto>());
        var component = RenderSelector(null, _ => { }, "title");

        var title = component.Find("h3");
        title.ToMarkup().Should().Contain("title");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Build_ShouldNotHaveH3Tag_WhenTitleIsNullOrEmpty(string title)
    {
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>())
            .Returns(new ListWithPaginationData<GroupDto>());

        RenderSelector(null, _ => { }, title).FindAll("h3").Should().BeEmpty();
    }

    [Fact]
    public void Build_ShouldNowBuildTable_WhenRequestsReturnsNull()
    {
        RenderSelector(null, _ => { }).Markup.Should().Be("<p><em>Loading...</em></p>");
    }

    [Fact]
    public void SelectItem_ShouldInvokeSelectChange_WhenArgIsTrue()
    {
        GroupDto? select = null;
        var data = new ListWithPaginationData<GroupDto>
        {
            Metadata = new PaginationData { TotalCount = 5, PageNumber = 1, PageSize = 5 },
            Data = _fixture.CreateMany<GroupDto>(5).ToList()
        };
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(data);
        var selector = RenderSelector(null, g => select = g);

        var checkbox = selector.Find("input");
        checkbox.Change(new ChangeEventArgs { Value = true });

        select.Should().Be(data.Data[0]);
    }

    [Fact]
    public void SelectItem_ShouldIgnore_WhenArgIsFalse()
    {
        GroupDto? select = null;
        var data = new ListWithPaginationData<GroupDto>
        {
            Metadata = new PaginationData { TotalCount = 5, PageNumber = 1, PageSize = 5 },
            Data = _fixture.CreateMany<GroupDto>(5).ToList()
        };
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(data);
        var selector = RenderSelector(null, g => select = g);

        var checkbox = selector.Find("input");
        checkbox.Change(new ChangeEventArgs { Value = false });

        select.Should().BeNull();
    }

    [Fact]
    public void SelectItem_ShouldChangeSelectedItem_WhenCurrentIsNotNull()
    {
        GroupDto? select = null;
        var data = new ListWithPaginationData<GroupDto>
        {
            Metadata = new PaginationData { TotalCount = 5, PageNumber = 1, PageSize = 5 },
            Data = _fixture.CreateMany<GroupDto>(5).ToList()
        };
        _parametersRequests.GetByParametersAsync(Arg.Any<GroupParameters>()).Returns(data);
        var selector = RenderSelector(null, g => select = g);

        var checkboxes = selector.FindAll("input").ToList();
        checkboxes.Count.Should().Be(5);
        for (var i = 0; i < 5; ++i)
        {
            selector.FindAll("input").ToList()[i].Change(new ChangeEventArgs { Value = true });

            select.Should().Be(data.Data[i]);
        }
    }

    private IRenderedComponent<SingleItemSelect<GroupDto, GroupParameters>> RenderSelector(GroupDto? select,
        Action<GroupDto> selectChanged, string title = "")
    {
        return _context.RenderComponent<SingleItemSelect<GroupDto, GroupParameters>>(options =>
            options.Add(s => s.SelectedItem, select)
                .Add(s => s.PropertyNames, new List<string> { "Name" })
                .Add(s => s.PropertiesToShow,
                    new List<Func<GroupDto, object>> { f => f.Name })
                .Add(s => s.Title, title)
                .Add(s => s.SelectedItemChanged, selectChanged));
    }
}