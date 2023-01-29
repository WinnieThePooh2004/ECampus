using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.DataTransferObjects;
using Microsoft.Extensions.Configuration;

namespace ECampus.Tests.Unit.FrontEnd.Requests;

public class RequestOptionsTests
{
    [Fact]
    public void CreateOptions_ShouldReturnValuesFromProvidedDictionary()
    {
        var configuration = Substitute.For<IConfiguration>();
        configuration[Arg.Any<string>()].Returns(info => info.Arg<string>());

        var options = new RequestOptions(configuration);

        options.GetControllerName(typeof(FacultyDto)).Should().Be(configuration["Requests:Faculties"]);
    }
}