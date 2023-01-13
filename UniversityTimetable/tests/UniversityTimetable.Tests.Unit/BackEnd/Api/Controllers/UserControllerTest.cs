using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api.Controllers;

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
    public async Task ValidateCreate_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        _service.ValidateCreateAsync(data).Returns(errors);

        var actionResult = await _controller.ValidateCreate(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(errors);
        await _service.Received().ValidateCreateAsync(data);
    }
    
    [Fact]
    public async Task ValidateUpdate_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        _service.ValidateUpdateAsync(data).Returns(errors);

        var actionResult = await _controller.ValidateUpdate(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(errors);
        await _service.Received().ValidateUpdateAsync(data);
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
    public async Task ChangePassword_ReturnsFromService_ServiceCalled()
    {
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _service.ChangePassword(passwordChange).Returns(passwordChange);

        var actionResult = await _controller.ChangePassword(passwordChange);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(passwordChange);
        await _service.Received().ChangePassword(passwordChange);
    }

    [Fact]
    public async Task ValidatePasswordChange_ShouldReturnFromService()
    {
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        _service.ValidatePasswordChange(passwordChange).Returns(errors);

        var actionResult = await _controller.ValidatePasswordChange(passwordChange);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(errors);
        await _service.Received(1).ValidatePasswordChange(passwordChange);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnFromPasswordService()
    {
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _service.ChangePassword(passwordChange).Returns(passwordChange);

        var actionResult = await _controller.ChangePassword(passwordChange);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(passwordChange);
        await _service.Received(1).ChangePassword(passwordChange);
    }

    [Fact]
    public async Task SaveAuditory_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.SaveAuditory(10, 10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().SaveAuditory(10, 10);
    }

    [Fact]
    public async Task RemoveSavedAuditory_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.RemoveAuditory(10, 10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().RemoveSavedAuditory(10, 10);
    }
    
    [Fact]
    public async Task SaveGroup_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.SaveGroup(10, 10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().SaveGroup(10, 10);
    }
    
    [Fact]
    public async Task RemoveSavedGroup_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.RemoveGroup(10, 10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().RemoveSavedGroup(10, 10);
    }
    
    [Fact]
    public async Task SaveTeacher_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.SaveTeacher(10, 10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().SaveTeacher(10, 10);
    }
    
    [Fact]
    public async Task RemovedSavedTeacher_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _controller.RemoveTeacher(10, 10);
        
        actionResult.Should().BeOfType<NoContentResult>();
        await _service.Received().RemoveSavedTeacher(10, 10);
    }
}