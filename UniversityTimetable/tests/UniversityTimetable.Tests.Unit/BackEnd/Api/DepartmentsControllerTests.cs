using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api
{
    public class DepartmentsControllerTests
    {
        private readonly IParametersService<DepartmentDto, DepartmentParameters> _service =
            Substitute.For<IParametersService<DepartmentDto, DepartmentParameters>>();
        private readonly DepartmentsController _controller;
        private readonly Fixture _fixture;
        public DepartmentsControllerTests()
        {
            _controller = new DepartmentsController(_service);
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetById_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Build<DepartmentDto>().With(t => t.Id, 10).Create();

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
            var data = _fixture.Create<DepartmentDto>();
            _service.CreateAsync(data).Returns(data);

            var actionResult = await _controller.Post(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().CreateAsync(data);
        }

        [Fact]
        public async Task Update_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Create<DepartmentDto>();
            _service.UpdateAsync(data).Returns(data);

            var actionResult = await _controller.Put(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().UpdateAsync(data);
        }

        [Fact]
        public async Task GetByParameters_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Build<ListWithPaginationData<DepartmentDto>>()
                .With(l => l.Data, Enumerable.Range(0, 5)
                    .Select(_ => _fixture.Create<DepartmentDto>()).ToList())
                .Create();

            _service.GetByParametersAsync(Arg.Any<DepartmentParameters>()).Returns(data);
            var actionResult = await _controller.Get(new DepartmentParameters());

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().GetByParametersAsync(Arg.Any<DepartmentParameters>());
        }

        [Fact]
        public void PassedInvalidItem_ShouldHaveValidationError_ServiceCalled()
        {
            var invalidItem = new DepartmentDto { Name = "" };
            var validator = new DepartmentDtoValidator();

            var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
            var expectedErrors = new List<string>
            {
                "Please, enter name",
            };
            errors.Should().BeEquivalentTo(expectedErrors);
        }
    }
}
