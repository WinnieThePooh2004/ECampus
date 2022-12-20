using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Domain.Validation;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api
{
    public class TimetableControllerTests
    {
        private readonly IClassService _service = Substitute.For<IClassService>();
        private readonly TimetableController _controller;
        private readonly Fixture _fixture;
        public TimetableControllerTests()
        {
            _controller = new(_service);
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetById_ReturnsFromService()
        {
            var data = _fixture.Build<ClassDto>().With(t => t.Id, 10).Create();

            _service.GetByIdAsync(10).Returns(data);
            var actionResult = await _controller.Get(10);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
        }

        [Fact]
        public async Task Delete_ReturnsIdFromService()
        {
            var actionResult = await _controller.Delete(10);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(10);
        }

        [Fact]
        public async Task Create_ReturnsFromService()
        {
            var data = _fixture.Create<ClassDto>();
            _service.CreateAsync(data).Returns(data);

            var actionResult = await _controller.Post(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
        }

        [Fact]
        public async Task Update_ReturnsFromService()
        {
            var data = _fixture.Create<ClassDto>();
            _service.UpdateAsync(data).Returns(data);

            var actionResult = await _controller.Put(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
        }

        [Fact]
        public void PassedInvalidItem_ShouldHaveValidationError()
        {
            var invalidItem = new ClassDto { TeacherId = 0, GroupId = 0, AuditoryId = 0, SubjectId = 0 };
            var validator = new ClassDTOValidator();

            var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
            var expectedErrors = new List<string>
            {
                "Please, select auditory",
                "Please, select group",
                "Please, select teacher",
                "Please, select subject"
            };
            errors.Should().BeEquivalentTo(expectedErrors);
        }

        [Fact]
        public async Task TeacherTimetable_ReturnsFromService()
        {
            var timetable = CreateTimetable();
            _service.GetTimetableForTeacherAsync(1).Returns(timetable);

            var actionResult = await _controller.TeacherTimetable(1);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(timetable);
        }

        [Fact]
        public async Task GroupTimetable_ReturnsFromService()
        {
            var timetable = CreateTimetable();
            _service.GetTimetableForGroupAsync(1).Returns(timetable);

            var actionResult = await _controller.GroupTimetable(1);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(timetable);

        }

        [Fact]
        public async Task AuditoryTimetable_ReturnsFromService()
        {
            var timetable = CreateTimetable();
            _service.GetTimetableForAuditoryAsync(1).Returns(timetable);

            var actionResult = await _controller.AuditoryTimetable(1);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(timetable);

        }

        private Timetable CreateTimetable()
        {
            var rand = new Random();
            return new Timetable(Enumerable.Range(0, 10)
                .Select(i => _fixture.Build<ClassDto>()
                .With(c => c.Number, rand.Next(0, 5))
                .With(c => c.DayOfWeek, rand.Next(0, 6))
                .Create()));
        }
    }
}
