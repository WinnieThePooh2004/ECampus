using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Responses.Class;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class TimetableControllerTests
{
    private readonly ITimetableService _service = Substitute.For<ITimetableService>();
    private readonly IBaseService<ClassDto> _baseService = Substitute.For<IBaseService<ClassDto>>();
    private readonly TimetableController _sut;
    private readonly Fixture _fixture;

    public TimetableControllerTests()
    {
        _sut = new TimetableController(_service, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ClassDto>().With(t => t.Id, 10).Create();
        _baseService.GetByIdAsync(10).Returns(data);
            
        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService_ServiceCalled()
    {
        var data = _fixture.Build<ClassDto>().With(t => t.Id, 10).Create();
        _baseService.DeleteAsync(10).Returns(data);
            
        var actionResult = await _sut.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<ClassDto>();
        _baseService.CreateAsync(data).Returns(data);

        var actionResult = await _sut.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<ClassDto>();
        _baseService.UpdateAsync(data).Returns(data);

        var actionResult = await _sut.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().UpdateAsync(data);
    }

    [Fact]
    public async Task TeacherTimetable_ReturnsFromService_ServiceCalled()
    {
        var timetable = CreateTimetable();
        _service.GetTimetableForTeacherAsync(1).Returns(timetable);

        var actionResult = await _sut.TeacherTimetable(1);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(timetable);
        await _service.Received().GetTimetableForTeacherAsync(1);
    }

    [Fact]
    public async Task GroupTimetable_ReturnsFromService_ServiceCalled()
    {
        var timetable = CreateTimetable();
        _service.GetTimetableForGroupAsync(1).Returns(timetable);

        var actionResult = await _sut.GroupTimetable(1);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(timetable);
        await _service.Received().GetTimetableForGroupAsync(1);
    }

    [Fact]
    public async Task AuditoryTimetable_ReturnsFromService_ServiceCalled()
    {
        var timetable = CreateTimetable();
        _service.GetTimetableForAuditoryAsync(1).Returns(timetable);

        var actionResult = await _sut.AuditoryTimetable(1);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(timetable);
        await _service.Received().GetTimetableForAuditoryAsync(1);
    }

    [Fact]
    public async Task Validate_ReturnsFromService_ServiceCalled()
    {
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(5));
        var @class = new ClassDto();
        _service.ValidateAsync(@class).Returns(errors);

        var actionResult = await _sut.Validate(@class);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(errors);
        await _service.Received().ValidateAsync(@class);
    }

    private Timetable CreateTimetable()
    {
        var rand = new Random();
        return new Timetable(Enumerable.Range(0, 10)
            .Select(_ => _fixture.Build<ClassDto>()
                .With(c => c.Number, rand.Next(0, 5))
                .With(c => c.DayOfWeek, rand.Next(0, 6))
                .Create()).ToList());
    }
}