﻿using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Auth;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api.Controllers;

public class AuthControllerTests
{
    private readonly AuthController _sut;
    private readonly IAuthorizationService _service;
    private readonly Fixture _fixture = new();

    public AuthControllerTests()
    {
        _service = Substitute.For<IAuthorizationService>();
        _sut = new AuthController(_service);
        
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Login_ReturnsFromService_ServiceCalled()
    {
        var loginResult = _fixture.Create<LoginResult>();
        var login = _fixture.Create<LoginDto>();
        _service.Login(login).Returns(loginResult);

        var actionResult = await _sut.Login(login);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(loginResult);
        await _service.Received().Login(login);
    }
}