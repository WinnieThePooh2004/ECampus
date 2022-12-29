using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Controllers;
using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Tests.Unit.BackEnd.Api
{
    public class SubjectsControllerTests
    {
        private readonly IParametersService<SubjectDto, SubjectParameters> _service = Substitute.For<IParametersService<SubjectDto, SubjectParameters>>();
        private readonly SubjectsController _controller;
        private readonly Fixture _fixture;
        public SubjectsControllerTests()
        {
            _controller = new SubjectsController(_service);
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetById_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Build<SubjectDto>().With(t => t.Id, 10).Create();

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
            var data = _fixture.Create<SubjectDto>();
            _service.CreateAsync(data).Returns(data);

            var actionResult = await _controller.Post(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().CreateAsync(data);
        }

        [Fact]
        public async Task Update_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Create<SubjectDto>();
            _service.UpdateAsync(data).Returns(data);

            var actionResult = await _controller.Put(data);

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().UpdateAsync(data);
        }

        [Fact]
        public async Task GetByParameters_ReturnsFromService_ServiceCalled()
        {
            var data = _fixture.Build<ListWithPaginationData<SubjectDto>>()
                .With(l => l.Data, Enumerable.Range(0, 5)
                    .Select(_ => _fixture.Create<SubjectDto>()).ToList())
                .Create();

            _service.GetByParametersAsync(Arg.Any<SubjectParameters>()).Returns(data);
            var actionResult = await _controller.Get(new SubjectParameters());

            actionResult.Should().BeOfType<OkObjectResult>();
            actionResult.As<OkObjectResult>().Value.Should().Be(data);
            await _service.Received().GetByParametersAsync(Arg.Any<SubjectParameters>());
        }

        [Fact]
        public void PassedInvalidItem_ShouldHaveValidationError_ServiceCalled()
        {
            var invalidItem = new SubjectDto { Name = "" };
            var validator = new SubjectDtoValidator();
            
            var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
            var expectedErrors = new List<string>
            {
                "Please, enter name"
            };
            errors.Should().BeEquivalentTo(expectedErrors);
        }
    }
}
