using Bunit;
using UniversityTimetable.FrontEnd.Components.PageOptions;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Components.PageOptions;

public class DataPageSizeSelectTests
{
    private readonly TestContext _context = new();

    [Fact]
    public void Change_ShouldInvokeOnPageSizeChanged_WhenInputSelectChanged()
    {
        var pageSize = 0;
        var select = _context.RenderComponent<DataPageSizeSelect>(options =>
            options.Add(s => s.PageSize, 5)
                .Add(s => s.OnPageSizeChanged, size => pageSize = size));

        var inputSelect = select.Find("select");
        inputSelect.Change("10");

        pageSize.Should().Be(10);
    }

    [Fact]
    public void Build_ShouldHave5SelectOptions()
    {
        var select = _context.RenderComponent<DataPageSizeSelect>();

        select.Markup.Should()
            .ContainAll("""option value="5" label="5""",
                """option value="10" label="10""",
                """option value="20" label="20""",
                """option value="50" label="50""",
                """option value="100" label="100""");
    }
}