using ECampus.Api.Controllers;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.BackEnd.Api.Controllers;

public class UserControllerTest
{
    private readonly IUserService _service;
    private readonly IBaseService<UserDto> _baseService = Substitute.For<IBaseService<UserDto>>();

    private readonly IParametersService<UserDto, UserParameters> _parametersService =
        Substitute.For<IParametersService<UserDto, UserParameters>>();

    private readonly IUserRelationsService _userRelationsService = Substitute.For<IUserRelationsService>();

    private readonly UsersController _sut;
    private readonly Fixture _fixture = new();

    public UserControllerTest()
    {
        _service = Substitute.For<IUserService>();
        _sut = new UsersController(_service, _baseService, _parametersService, _userRelationsService);
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _sut.ControllerContext.HttpContext = Substitute.For<HttpContext>();
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<UserDto>().With(t => t.Id, 10).Create();
        _baseService.GetByIdAsync(10).Returns(data);

        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService_ServiceCalled()
    {
        var data = _fixture.Build<UserDto>().With(t => t.Id, 10).Create();
        _baseService.DeleteAsync(10).Returns(data);

        var actionResult = await _sut.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task ValidateCreate_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList());
        _service.ValidateCreateAsync(data).Returns(errors);

        var actionResult = await _sut.ValidateCreate(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(errors);
        await _service.Received().ValidateCreateAsync(data);
    }

    [Fact]
    public async Task ValidateUpdate_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList());
        _service.ValidateUpdateAsync(data).Returns(errors);

        var actionResult = await _sut.ValidateUpdate(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(errors);
        await _service.Received().ValidateUpdateAsync(data);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ListWithPaginationData<UserDto>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<UserDto>()).ToList())
            .Create();

        _parametersService.GetByParametersAsync(Arg.Any<UserParameters>()).Returns(data);
        var actionResult = await _sut.Get(new UserParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _parametersService.Received().GetByParametersAsync(Arg.Any<UserParameters>());
    }

    [Fact]
    public async Task Create_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        _baseService.CreateAsync(data).Returns(data);

        var actionResult = await _sut.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<UserDto>();
        _baseService.UpdateAsync(data).Returns(data);

        var actionResult = await _sut.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().UpdateAsync(data);
    }

    [Fact]
    public async Task ChangePassword_ReturnsFromService_ServiceCalled()
    {
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _service.ChangePassword(passwordChange).Returns(passwordChange);

        var actionResult = await _sut.ChangePassword(passwordChange);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(passwordChange);
        await _service.Received().ChangePassword(passwordChange);
    }

    [Fact]
    public async Task ValidatePasswordChange_ShouldReturnFromService()
    {
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(10));
        _service.ValidatePasswordChange(passwordChange).Returns(errors);

        var actionResult = await _sut.ValidatePasswordChange(passwordChange);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(errors);
        await _service.Received(1).ValidatePasswordChange(passwordChange);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnFromPasswordService()
    {
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _service.ChangePassword(passwordChange).Returns(passwordChange);

        var actionResult = await _sut.ChangePassword(passwordChange);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(passwordChange);
        await _service.Received(1).ChangePassword(passwordChange);
    }

    [Fact]
    public async Task SaveAuditory_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _sut.SaveAuditory(10, 10);

        actionResult.Should().BeOfType<NoContentResult>();
        await _userRelationsService.Received().SaveAuditory(10, 10);
    }

    [Fact]
    public async Task RemoveSavedAuditory_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _sut.RemoveAuditory(10, 10);

        actionResult.Should().BeOfType<NoContentResult>();
        await _userRelationsService.Received().RemoveSavedAuditory(10, 10);
    }

    [Fact]
    public async Task SaveGroup_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _sut.SaveGroup(10, 10);

        actionResult.Should().BeOfType<NoContentResult>();
        await _userRelationsService.Received().SaveGroup(10, 10);
    }

    [Fact]
    public async Task RemoveSavedGroup_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _sut.RemoveGroup(10, 10);

        actionResult.Should().BeOfType<NoContentResult>();
        await _userRelationsService.Received().RemoveSavedGroup(10, 10);
    }

    [Fact]
    public async Task SaveTeacher_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _sut.SaveTeacher(10, 10);

        actionResult.Should().BeOfType<NoContentResult>();
        await _userRelationsService.Received().SaveTeacher(10, 10);
    }

    [Fact]
    public async Task RemovedSavedTeacher_ReturnsNoContent_ServiceCalled()
    {
        var actionResult = await _sut.RemoveTeacher(10, 10);

        actionResult.Should().BeOfType<NoContentResult>();
        await _userRelationsService.Received().RemoveSavedTeacher(10, 10);
    }
}