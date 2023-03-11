using ECampus.FrontEnd.Requests.Options;
using Microsoft.Extensions.Configuration;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Requests;

public class RequestOptionsTests
{
    [Fact]
    public void CreateOptions_ShouldThrow_WhenCannotCreateDictionary()
    {
        var configuration = Substitute.For<IConfiguration>();
        configuration[Arg.Any<string>()].Returns("");

        new Func<RequestOptions>(() => new RequestOptions(configuration)).Should().Throw<InvalidOperationException>();
    }
}