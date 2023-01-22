using System.Net;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace ECampus.Tests.Unit.BackEnd.Api.Filters;

public class MiddlewareExceptionFilterTests
{
    private readonly MiddlewareExceptionFilter _sut;

    public MiddlewareExceptionFilterTests()
    {
        _sut = new MiddlewareExceptionFilter(Substitute.For<ILogger>());
    }

    [Fact]
    public void OnException_ShouldReturnExceptionDetails_WhenExceptionsIsHttpResponseException()
    {
        var exception = new HttpResponseException(HttpStatusCode.Forbidden, "message", new object());
        var context = new ExceptionContext(new ActionContext(Substitute.For<HttpContext>(), new RouteData(),
            new ActionDescriptor()), new List<IFilterMetadata>())
        {
            Exception = exception
        };

        _sut.OnException(context);

        context.Result.Should().NotBeNull();
        context.Result.Should().BeOfType<ObjectResult>();
        var result = (ObjectResult)context.Result!;
        result.StatusCode.Should().Be(exception.StatusCode);
        result.Value.Should().BeOfType<BadResponseObject>();
        var resultObject = (BadResponseObject)result.Value!;
        resultObject.Message.Should().Be(exception.Message);
        resultObject.ResponseObject.Should().Be(exception.Object);
    }

    [Fact]
    public void OnException_ShouldReturnExceptionMessageAnd500_WhenExceptionsIsNotHttpResponseException()
    {
        var exception = new Exception("message");
        var context = new ExceptionContext(new ActionContext(Substitute.For<HttpContext>(), new RouteData(),
            new ActionDescriptor()), new List<IFilterMetadata>())
        {
            Exception = exception
        };
        
        _sut.OnException(context);

        context.Result.Should().NotBeNull();
        context.Result.Should().BeOfType<ObjectResult>();
        var result = (ObjectResult)context.Result!;
        result.StatusCode.Should().Be(500);
        result.Value.Should().Be(exception.Message);
    }

    [Fact]
    public void Order_ShouldBeMaxIntMinus10()
    {
        _sut.Order.Should().Be(int.MaxValue - 10);
    }
}