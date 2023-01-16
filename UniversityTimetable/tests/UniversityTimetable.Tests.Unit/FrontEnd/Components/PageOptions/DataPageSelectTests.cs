using Bunit;
using UniversityTimetable.FrontEnd.Components.PageOptions;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Components.PageOptions;

public class DataPageSelectTests
{
    private readonly TestContext _context = new();

    [Fact]
    public void ChangePage_ShouldInvokeOnPageChanged()
    {
        var page = 0;

        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages,5)
                .Add(d => d.OnPageNumberChanged, p => page = p));

        var pageButtons = pageSelect.FindAll("a").ToList();
        pageButtons.Count.Should().Be(5);
        var page2 = pageButtons.First(e => e.ToMarkup().Contains(">2<"));
        page2.Click();
        page.Should().Be(2);
        var page3 = pageButtons.First(e => e.ToMarkup().Contains(">3<"));
        page3.Click();
        page.Should().Be(3);
    }

    [Fact]
    public void BuildComponent_ShouldHaveNoPoints_WhenTotalPagesLessOrEqualThan5()
    {
        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages, 5));
        var markup = pageSelect.Markup;

        markup.Should().ContainAll(">1<",">2<", ">3<", ">4<", ">5<");
        markup.Should().NotContain("...");
    }
    
    [Theory]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(100)]
    [InlineData(1000)]
    public void BuildComponent_ShouldHaveFrom1To5AndMaxPage(int maxPage)
    {
        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages, maxPage));
        var markup = pageSelect.Markup;

        markup.Should().ContainAll(">1<",">2<", ">3<", ">4<", ">5<", $">...{maxPage}<");
        markup.Should().NotContain(">6<");
    }

    // public void BuildComponent_ShouldHaveReferenceToFirstPage_WhenHaveALotOfPagesAndPageNumberMoreThan5()
    // {
    //     
    // }
}