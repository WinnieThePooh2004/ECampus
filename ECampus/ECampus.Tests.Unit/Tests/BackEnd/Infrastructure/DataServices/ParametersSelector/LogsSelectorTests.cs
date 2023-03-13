using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.Domain.Entities;
using ECampus.Domain.Requests.Log;
using ECampus.Infrastructure;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Serilog.Events;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class LogsSelectorTests
{
    private readonly LogsSelector _sut = new();
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    private readonly Log[] _data =
    {
        new() { Id = 1, Level = LogEventLevel.Verbose, TimeStamp = DateTime.Now - TimeSpan.FromDays(10) },
        new() { Id = 2, Level = LogEventLevel.Debug, TimeStamp = DateTime.Now - TimeSpan.FromDays(5) },
        new() { Id = 3, Level = LogEventLevel.Information, TimeStamp = DateTime.Now - TimeSpan.FromDays(0) },
        new() { Id = 4, Level = LogEventLevel.Warning, TimeStamp = DateTime.Now + TimeSpan.FromDays(5) },
        new() { Id = 5, Level = LogEventLevel.Error, TimeStamp = DateTime.Now + TimeSpan.FromDays(10) },
        new() { Id = 6, Level = LogEventLevel.Fatal, TimeStamp = DateTime.Now + TimeSpan.FromDays(15) }
    };

    public LogsSelectorTests()
    {
        var set = _data.AsDbSet();
        _context.Set<Log>().Returns(set);
    }

    [Fact]
    public void Select_ShouldSelectSuited()
    {
        var parameters = new LogParameters
        {
            From = DateTime.Now - TimeSpan.FromDays(8), To = DateTime.Now + TimeSpan.FromDays(8)
        };

        var result = _sut.SelectData(_context, parameters).ToList();

        var span = new ReadOnlySpan<Log>(_data).Slice(1, 3);
        result.Should().BeEquivalentTo(span.ToArray());
    }
}