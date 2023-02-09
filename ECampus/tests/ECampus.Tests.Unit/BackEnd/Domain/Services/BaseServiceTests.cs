using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public sealed class BaseServiceTests
{
    private readonly BaseService<AuditoryDto, Auditory> _sut;
    private readonly Fixture _fixture;
    private readonly IMapper _mapper = MapperFactory.Mapper;
    private readonly IDataAccessManager _dataAccess = Substitute.For<IDataAccessManager>();

    public BaseServiceTests()
    {
        _sut = new BaseService<AuditoryDto, Auditory>(_mapper, _dataAccess);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    private async Task Create_ReturnsFromService_ServiceCalled_WhenNoValidationExceptions()
    {
        var item = _fixture.Create<AuditoryDto>();
        _dataAccess.CreateAsync(Arg.Any<Auditory>()).Returns(_mapper.Map<Auditory>(item));

        await _sut.CreateAsync(item);

        await _dataAccess.Received(1).CreateAsync(Arg.Is<Auditory>(a => a.Id == item.Id));
        await _dataAccess.Received().SaveChangesAsync();
    }

    [Fact]
    private async Task Update_ReturnsFromService_WhenNoValidationExceptions()
    {
        var item = _fixture.Create<AuditoryDto>();

        await _sut.UpdateAsync(item);

        await _dataAccess.Received(1).UpdateAsync(Arg.Is<Auditory>(a => a.Id == item.Id));
        await _dataAccess.Received().SaveChangesAsync();
    }

    [Fact]
    private async Task Delete_ShouldReturnFromService_WhenIdIsNotNull()
    {
        var item = _fixture.Create<AuditoryDto>();
        _dataAccess.Delete(Arg.Any<Auditory>()).Returns(_mapper.Map<Auditory>(item));

        var result = await _sut.DeleteAsync(10);

        result.Should().BeEquivalentTo(_mapper.Map<AuditoryDto>(_mapper.Map<Auditory>(item)),
            opt => opt.ComparingByMembers<AuditoryDto>());
        _dataAccess.Received(1).Delete(Arg.Any<Auditory>());
        await _dataAccess.Received().SaveChangesAsync();
    }

    [Fact]
    private async Task GetById_ShouldReturnFromRepository_WhenIdIsNotNull()
    {
        var item = _fixture.Create<AuditoryDto>();
        _dataAccess.GetByIdAsync<Auditory>(10).Returns(_mapper.Map<Auditory>(item));

        var result = await _sut.GetByIdAsync(10);

        result.Should().BeEquivalentTo(_mapper.Map<AuditoryDto>(_mapper.Map<Auditory>(item)),
            opt => opt.ComparingByMembers<AuditoryDto>());
        await _dataAccess.Received(1).GetByIdAsync<Auditory>(10);
    }
}