using Bunit;
using ECampus.FrontEnd.Components.PageOptions;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.PageOptions;

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
    }

    [Fact]
    public void Build_ShouldHaveOnly1ATag_WhenTotalPagesIs1()
    {
        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages, 1));
        var markup = pageSelect.Markup;

        pageSelect.FindAll("a").Count.Should().Be(1);
        markup.Should().ContainAll(">1</u>");
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void BuildComponent_ShouldHaveNoPoints_WhenTotalPagesLessOrEqualThan5(int totalPages)
    {
        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages, 5));
        var markup = pageSelect.Markup;

        markup.Should().ContainAll(Enumerable.Range(1, totalPages - 1).Select(i => $">{i}</u>").ToList());
        markup.Should().NotContain("...");
    }
    
    [Theory]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(100)]
    [InlineData(1000)]
    public void BuildComponent_ShouldHaveFrom1To5AndMaxPage(int maxPage)
    {
        var page = 0;
        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages, maxPage)
                .Add(d => d.OnPageNumberChanged, p => page = p));
        var markup = pageSelect.Markup;

        markup.Should().ContainAll(">1</u>",">2</u>", ">3</u>", ">4</u>", ">5</u>", $">...{maxPage}</u>");
        markup.Should().NotContain(">6<");
        ToLastPage(pageSelect);
        page.Should().Be(maxPage);
    }

    [Fact]
    public void BuildComponent_ShouldHaveReferenceToFirstPage_WhenHaveALotOfPagesAndPageNumberMoreThan5()
    {
        var page = 0;
        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages, 20)
                .Add(d => d.OnPageNumberChanged, p => page = p));
        ToLastPage(pageSelect);
        page.Should().Be(20);

        var markup = pageSelect.Markup;

        markup.Should().Contain(">1...</u>");
        markup.Should().ContainAll(Enumerable.Range(13, 8).Select(i => $">{i}</u>"));
        pageSelect.FindAll("a").Last().Click();
        page.Should().Be(20);
        pageSelect.Find("a").Click();
        page.Should().Be(1);
    }

    [Fact]
    public void Build_ShouldHaveReferencesToFirstAndLastPages_WhenDifferenceBetweenFirstAndLastPagesAreBig()
    {
        var page = 0;
        var pageSelect = _context.RenderComponent<DataPageSelect>(options =>
            options.Add(d => d.TotalPages, 100)
                .Add(d => d.OnPageNumberChanged, p => page = p));

        GoToPage(pageSelect, 5);
        page.Should().Be(5);
        var markup = pageSelect.Markup;

        markup.Should().Contain(">1...</u>");
        markup.Should().Contain(">...100</u>");
        markup.Should().ContainAll(Enumerable.Range(page - 5 + 2, 5 + 2).Select(i => $">{i}</u>"));
        //this block is needed to get more test coverage
        ToFirstPage(pageSelect);
        page.Should().Be(1);
        GoToPage(pageSelect, 5);
        ToLastPage(pageSelect);
        page.Should().Be(100);
        ToFirstPage(pageSelect);
        GoToPage(pageSelect, 5);
        GoToPage(pageSelect,7);
        page.Should().Be(7);
    }

    private static void GoToPage(IRenderedFragment pageSelect, int page)
    {
        pageSelect.FindAll("a").First(a => a.ToMarkup().Contains($">{page}<")).Click();
    }

    private static void ToFirstPage(IRenderedFragment pageSelect)
    {
        pageSelect.FindAll("a").First().Click();
    }

    private static void ToLastPage(IRenderedFragment pageSelect)
    {
        pageSelect.FindAll("a").Last().Click();
    }
}