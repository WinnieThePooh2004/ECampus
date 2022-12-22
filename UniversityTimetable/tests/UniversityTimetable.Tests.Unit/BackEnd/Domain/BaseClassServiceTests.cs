using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain
{
    public class BaseClassServiceTests
    {
        private readonly IBaseService<ClassDto> _baseService;
        private readonly BaseClassService _service;
        private readonly IClassRepository _repository;
        private readonly Fixture _fixture;

        public BaseClassServiceTests()
        {
            var logger = Substitute.For<ILogger<BaseClassService>>();
            _baseService = Substitute.For<IBaseService<ClassDto>>();
            _repository = Substitute.For<IClassRepository>();
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClassProfile>()).CreateMapper();
            _service = new BaseClassService(_baseService, logger, _repository, mapper);
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetById_ReturnsFromBaseService()
        {
            var entity = _fixture.Create<ClassDto>();
            _baseService.GetByIdAsync(1).Returns(entity);

            var result = await _service.GetByIdAsync(1);

            result.Should().Be(entity);
        }

        [Fact]
        public async Task Update_NoValidationErrors_ShouldReturnFromBaseService()
        {
            var entity = _fixture.Create<ClassDto>();
            _baseService.UpdateAsync(entity).Returns(entity);
            _repository.ValidateAsync(Arg.Any<Class>()).Returns(new Dictionary<string, string>());

            var result = await _service.UpdateAsync(entity);
            result.Should().BeEquivalentTo(entity, opt => opt.ComparingByMembers<ClassDto>());
        }

        [Fact]
        public async Task Update_OneValidationError_ShouldThrowException()
        {
            _repository.ValidateAsync(Arg.Any<Class>()).Returns(new Dictionary<string, string> { [""] = "" });

            await new Func<Task>(async () => await _service.UpdateAsync(new())).Should()
                .ThrowAsync<ClassValidationException>()
                .WithMessage("One or more validation errors occured\nError code: 400");
        }

        [Fact]
        public async Task Create_NoValidationErrors_ShouldReturnFromBaseService()
        {
            var entity = _fixture.Create<ClassDto>();
            _baseService.CreateAsync(entity).Returns(entity);
            _repository.ValidateAsync(Arg.Any<Class>()).Returns(new Dictionary<string, string>());

            var result = await _service.CreateAsync(entity);
            result.Should().BeEquivalentTo(entity, opt => opt.ComparingByMembers<ClassDto>());
        }

        [Fact]
        public async Task Create_OneValidationError_ShouldThrowException()
        {
            _repository.ValidateAsync(Arg.Any<Class>()).Returns(new Dictionary<string, string> { [""] = "" });

            await new Func<Task>(async () => await _service.CreateAsync(new())).Should()
                .ThrowAsync<ClassValidationException>()
                .WithMessage("One or more validation errors occured\nError code: 400");
        }
    }
}
