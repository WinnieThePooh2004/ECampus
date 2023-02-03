﻿using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Services.Services;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public sealed class ParametersServiceTests
{
    private readonly IAbstractFactory<Auditory> _dataFactory;
    private readonly ParametersService<AuditoryDto, AuditoryParameters, Auditory> _service;
    private readonly IParametersDataAccessManager _dataAccess = Substitute.For<IParametersDataAccessManager>();
    private readonly Fixture _fixture;

    public ParametersServiceTests()
    {
        _dataFactory = new AuditoryFactory();
        var mapper = MapperFactory.Mapper;

        Substitute.For<IBaseService<AuditoryDto>>();
        _service = new ParametersService<AuditoryDto, AuditoryParameters, Auditory>(mapper, _dataAccess);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromRepository()
    {
        var parameters = new AuditoryParameters { PageSize = 10, PageNumber = 1 };
        var data = _dataFactory.CreateMany(_fixture, 10);
        var expected = new DbSetMock<Auditory>(data).Object;
        _dataAccess.GetByParameters<Auditory, AuditoryParameters>(parameters).Returns(expected);

        var result = await _service.GetByParametersAsync(parameters);

        result.Metadata.Should().BeEquivalentTo(new PaginationData { TotalCount = 10, PageNumber = 1, PageSize = 10 });
        result.Data.Select(a => a.Id).Should().BeEquivalentTo(data.Select(d => d.Id));
    }
}