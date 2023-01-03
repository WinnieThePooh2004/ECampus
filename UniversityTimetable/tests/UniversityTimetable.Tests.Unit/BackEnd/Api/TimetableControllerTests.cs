using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api
{
    public class TimetableControllerTests
    {
        private readonly IClassService _service = Substitute.For<IClassService>();
        private readonly TimetableController _controller;
        private readonly Fixture _fixture;
        public TimetableControllerTests()
        {
            _controller = new TimetableController(_service);
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetById_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Build<ClassDto>().With(t => t.Id, 10).Create();

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
            var data = _fixture.Create<ClassDto>();
            _service.CreateAsync(data).Returns(data);

            var actionResult = await _controller.Post(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().CreateAsync(data);
        }

        [Fact]
        public async Task Update_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Create<ClassDto>();
            _service.UpdateAsync(data).Returns(data);

            var actionResult = await _controller.Put(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().UpdateAsync(data);
        }

        [Fact]
        public async Task TeacherTimetable_ReturnsFromService_ServiceCalled()
        {
            var timetable = CreateTimetable();
            _service.GetTimetableForTeacherAsync(1).Returns(timetable);

            var actionResult = await _controller.TeacherTimetable(1);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(timetable);
            await _service.Received().GetTimetableForTeacherAsync(1);
        }

        [Fact]
        public async Task GroupTimetable_ReturnsFromService_ServiceCalled()
        {
            var timetable = CreateTimetable();
            _service.GetTimetableForGroupAsync(1).Returns(timetable);

            var actionResult = await _controller.GroupTimetable(1);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(timetable);
            await _service.Received().GetTimetableForGroupAsync(1);
        }

        [Fact]
        public async Task AuditoryTimetable_ReturnsFromService_ServiceCalled()
        {
            var timetable = CreateTimetable();
            _service.GetTimetableForAuditoryAsync(1).Returns(timetable);

            var actionResult = await _controller.AuditoryTimetable(1);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(timetable);
            await _service.Received().GetTimetableForAuditoryAsync(1);
        }
        
        [Fact]
        public async Task Validate_ReturnsFromService_ServiceCalled()
        {
            var errors = new List<KeyValuePair<string, string>>(_fixture.CreateMany<KeyValuePair<string, string>>(5));
            var @class = new ClassDto();
            _service.ValidateAsync(@class).Returns(errors);

            var actionResult = await _controller.Validate(@class);

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
}
