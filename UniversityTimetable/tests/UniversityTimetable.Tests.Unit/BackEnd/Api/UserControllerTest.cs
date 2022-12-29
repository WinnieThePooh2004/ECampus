using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api;

public class UserControllerTest
{
    private readonly IUserService _service;
    private readonly UsersController _controller;
    private readonly Fixture _fixture = new();
    public UserControllerTest()
    {
        _service = Substitute.For<IUserService>();
        _controller = new UsersController(_service);
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
    }
    
    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<UserDto>().With(t => t.Id, 10).Create();

        _service.GetByIdAsync(10).Returns(data);
        var actionResult = await _controller.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService_ServiceCalled()
    {
        var actionResult = await _controller.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(10);
        await _service.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        _service.CreateAsync(data).Returns(data);

        var actionResult = await _controller.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        _service.UpdateAsync(data).Returns(data);

        var actionResult = await _controller.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().UpdateAsync(data);
    }

    [Fact]
    public void Validate_InvalidItemPasses_ShouldHaveValidationError()
    {
        var invalidItem = new UserDto{ Username = "", Email = "", Password = "", PasswordConfirm = "a" };
        var validator = new UserDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Enter valid email",
            "Password must be at least 8 symbols",
            "Enter some password, please.",
            "Password length must be at least 8 characters.",
            "Password must contain at least one uppercase letter.",
            "Password must contain at least one lowercase letter.",
            "Password must contain at least one number.",
            "Passwords don't match"
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task SaveAuditory_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.SaveAuditory(10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().SaveAuditory(Arg.Any<ClaimsPrincipal>(), 10);
    }
    
    [Fact]
    public async Task RemoveSavedAuditory_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.RemoveAuditory(10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().RemoveSavedAuditory(Arg.Any<ClaimsPrincipal>(), 10);
    }
    
    [Fact]
    public async Task SaveGroup_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.SaveGroup(10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().SaveGroup(Arg.Any<ClaimsPrincipal>(), 10);
    }
    
    [Fact]
    public async Task RemoveSavedGroup_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.RemoveGroup(10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().RemoveSavedGroup(Arg.Any<ClaimsPrincipal>(), 10);
    }
    
    [Fact]
    public async Task SaveTeacher_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.SaveTeacher(10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().SaveTeacher(Arg.Any<ClaimsPrincipal>(), 10);
    }
    
    [Fact]
    public async Task RemovedSavedTeacher_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.RemoveTeacher(10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().RemoveSavedTeacher(Arg.Any<ClaimsPrincipal>(), 10);
    }
}