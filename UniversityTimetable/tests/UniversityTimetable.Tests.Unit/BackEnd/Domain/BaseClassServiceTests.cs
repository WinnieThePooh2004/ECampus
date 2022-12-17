﻿using AutoMapper;
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
        private readonly IBaseService<ClassDTO> _baseService;
        private readonly BaseClassService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<BaseClassService> _logger;
        private readonly IClassRepository _repository;
        private readonly Fixture _fixture;

        public BaseClassServiceTests()
        {
            _logger = Substitute.For<ILogger<BaseClassService>>();
            _baseService = Substitute.For<IBaseService<ClassDTO>>();
            _repository = Substitute.For<IClassRepository>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClassProfile>()).CreateMapper();
            _service = new BaseClassService(_baseService, _logger, _repository, _mapper);
            _fixture = new();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetById_ReturnsFromBaseService()
        {
            var entity = _fixture.Create<ClassDTO>();
            _baseService.GetByIdAsync(1).Returns(entity);

            var result = await _service.GetByIdAsync(1);

            result.Should().Be(entity);
        }

        [Fact]
        public async Task Update_NoValidationErrors_ShouldReturnFromBaseService()
        {
            var entity = _fixture.Create<ClassDTO>();
            _baseService.UpdateAsync(entity).Returns(entity);
            _repository.ValidateAsync(Arg.Any<Class>()).Returns(new Dictionary<string, string>());

            var result = await _service.UpdateAsync(entity);
            result.Should().BeEquivalentTo(entity, opt => opt.ComparingByMembers<ClassDTO>());
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
            var entity = _fixture.Create<ClassDTO>();
            _baseService.CreateAsync(entity).Returns(entity);
            _repository.ValidateAsync(Arg.Any<Class>()).Returns(new Dictionary<string, string>());

            var result = await _service.CreateAsync(entity);
            result.Should().BeEquivalentTo(entity, opt => opt.ComparingByMembers<ClassDTO>());
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