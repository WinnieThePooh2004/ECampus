using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Services.ValidationServices;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Services;

public class ServiceWithCreateValidationTests
{
    private readonly ServiceWithCreateValidation<GroupDto> _sut;
    private readonly ICreateValidator<GroupDto> _validator = Substitute.For<ICreateValidator<GroupDto>>();
    private readonly IBaseService<GroupDto> _baseService = Substitute.For<IBaseService<GroupDto>>();
    private readonly Fixture _fixture = new();

    public ServiceWithCreateValidationTests()
    {
        _sut = new ServiceWithCreateValidation<GroupDto>(_baseService, _validator);
    }

    [Fact]
    public async Task GetById_ReturnsFromBaseService()
    {
        var group = _fixture.Create<GroupDto>();
        _baseService.GetByIdAsync(10).Returns(group);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(group);
    }

    [Fact]
    public async Task Update_ReturnsFromBaseService()
    {
        var group = _fixture.Create<GroupDto>();
        _baseService.UpdateAsync(group).Returns(group);

        var result = await _sut.UpdateAsync(group);

        result.Should().Be(group);
    }

    [Fact]
    public async Task Delete_CallsBaseService()
    {
        await _sut.DeleteAsync(10);

        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromBaseService_WhenNoValidationErrors()
    {
        var group = _fixture.Create<GroupDto>();
        var errors = new ValidationResult();
        _validator.ValidateAsync(group).Returns(errors);
        _baseService.CreateAsync(group).Returns(group);

        var result = await _sut.CreateAsync(group);

        result.Should().Be(group);
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenValidationErrorOccured()
    {
        var group = _fixture.Create<GroupDto>();
        _validator.ValidateAsync(group)
            .Returns(new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList()));
        _baseService.CreateAsync(group).Returns(group);

        await new Func<Task>(() => _sut.CreateAsync(group)).Should().ThrowAsync<ValidationException>();
    }
}