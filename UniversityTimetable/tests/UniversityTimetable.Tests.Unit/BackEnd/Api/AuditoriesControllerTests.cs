using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Domain.Validation;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api
{
    public class AuditoriesControllerTests
    {
        private readonly IService<AuditoryDto, AuditoryParameters> _service = Substitute.For<IService<AuditoryDto, AuditoryParameters>>();
        private readonly AuditoriesController _controller;
        private readonly Fixture _fixture;
        public AuditoriesControllerTests()
        {
            _controller = new(_service);
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetById_ReturnsFromService()
        {
            var data = _fixture.Build<AuditoryDto>().With(t => t.Id, 10).Create();

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
            var data = _fixture.Create<AuditoryDto>();
            _service.CreateAsync(data).Returns(data);

            var actionResult = await _controller.Post(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
        }

        [Fact]
        public async Task Update_ReturnsFromService()
        {
            var data = _fixture.Create<AuditoryDto>();
            _service.UpdateAsync(data).Returns(data);

            var actionResult = await _controller.Put(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
        }

        [Fact]
        public async Task GetByParameters_ReturnsFromService()
        {
            var data = _fixture.Build<ListWithPaginationData<AuditoryDto>>()
                .With(l => l.Data, Enumerable.Range(0, 5).Select(i => _fixture.Create<AuditoryDto>()).ToList())
                .Create();

            _service.GetByParametersAsync(Arg.Any<AuditoryParameters>()).Returns(data);
            var actionResult = await _controller.Get(new AuditoryParameters());

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
        }

        [Fact]
        public void PassedInvalidItem_ShouldHaveValidationError()
        {
            var invalidItem = new AuditoryDto { Name = "", Building = "" };
            var validator = new AuditoryDtoValidator();

            var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
            var expectedErrors = new List<string>
            {
                "Please, enter name",
                "Please, enter building`s name"
            };
            errors.Should().BeEquivalentTo(expectedErrors);
        }
    }
}
