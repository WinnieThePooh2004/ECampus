﻿using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Requests.Subject;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Subject;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class SubjectsControllerTests
{
    private readonly IParametersService<MultipleSubjectResponse, SubjectParameters> _service =
        Substitute.For<IParametersService<MultipleSubjectResponse, SubjectParameters>>();

    private readonly IBaseService<SubjectDto> _baseService = Substitute.For<IBaseService<SubjectDto>>();
    private readonly SubjectsController _sut;
    private readonly Fixture _fixture;

    public SubjectsControllerTests()
    {
        _sut = new SubjectsController(_service, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<SubjectDto>().With(t => t.Id, 10).Create();
        _baseService.GetByIdAsync(10).Returns(data);
        
        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService_ServiceCalled()
    {
        var data = _fixture.Build<SubjectDto>().With(t => t.Id, 10).Create();
        _baseService.DeleteAsync(10).Returns(data);
        
        var actionResult = await _sut.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<SubjectDto>();
        _baseService.CreateAsync(data).Returns(data);

        var actionResult = await _sut.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<SubjectDto>();
        _baseService.UpdateAsync(data).Returns(data);

        var actionResult = await _sut.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().UpdateAsync(data);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ListWithPaginationData<MultipleSubjectResponse>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<MultipleSubjectResponse>()).ToList())
            .Create();

        _service.GetByParametersAsync(Arg.Any<SubjectParameters>()).Returns(data);
        var actionResult = await _sut.Get(new SubjectParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().GetByParametersAsync(Arg.Any<SubjectParameters>());
    }
}