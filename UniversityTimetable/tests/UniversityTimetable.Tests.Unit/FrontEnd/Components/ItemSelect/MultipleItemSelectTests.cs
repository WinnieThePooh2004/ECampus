using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using UniversityTimetable.FrontEnd.Components.DataSelectors;
using UniversityTimetable.FrontEnd.PropertySelectors;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Components.ItemSelect;

public class MultipleItemSelectTests
{
    private readonly TestContext _context = new();

    private readonly IParametersRequests<FacultyDto, FacultyParameters> _parametersRequests =
        Substitute.For<IParametersRequests<FacultyDto, FacultyParameters>>();

    private readonly Fixture _fixture = new();
    private bool _onChangedInvoked;
    
    private static readonly IPropertySelector<FacultyDto> PropertySelector = new PropertySelector<FacultyDto>();

    private static readonly ISearchTermsSelector<FacultyParameters> SearchTermsSelector =
        new SearchTermsSelector<FacultyParameters>();


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
            .Build<FacultyDto>().With(f => f.Id, i).Create()).ToList();
        var selectTo = new List<FacultyDto>();
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<FacultyDto> { Data = items });

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
            .Build<FacultyDto>().With(f => f.Id, i).Create()).ToList();
        var selectTo = new List<FacultyDto> { new() { Id = 0 } };
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<FacultyDto> { Data = items });

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
            .Build<FacultyDto>().With(f => f.Id, i).Create()).ToList();
        var selectTo = new List<FacultyDto> { new() { Id = 0 } };
        _parametersRequests.GetByParametersAsync(Arg.Any<FacultyParameters>())
            .Returns(new ListWithPaginationData<FacultyDto>
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
            .Returns(new ListWithPaginationData<FacultyDto>());
        var component = RenderSelector(new List<FacultyDto>(), "title");

        var title = component.Find("h3");
        title.ToMarkup().Should().Contain("title");
    }

    [Fact]
    public void Build_ShouldNotBuildTable_WhenRequestsReturnsNull()
    {
        RenderSelector(new List<FacultyDto>()).Markup.Should().Be("<p><em>Loading...</em></p>");
    }
    
    
    private IRenderedComponent<MultipleItemsSelect<FacultyDto, FacultyParameters>> RenderSelector(
        List<FacultyDto> selectTo, string title = "")
    {
        return _context.RenderComponent<MultipleItemsSelect<FacultyDto, FacultyParameters>>(
            parameters => parameters
                .Add(s => s.SelectTo, selectTo)
                .Add(s => s.Title, title)
                .Add(s => s.OnChanged, 
                    () => _onChangedInvoked = true));
    }
}