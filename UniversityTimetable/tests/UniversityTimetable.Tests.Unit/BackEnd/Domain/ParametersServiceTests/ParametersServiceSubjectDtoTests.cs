using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.ParametersServiceTests;

public class ParametersServiceSubjectDtoTests : ParametersServiceTests<SubjectDto, SubjectParameters, Subject>,
    IClassFixture<SubjectFactory>
{
    [Fact] protected override Task Delete_ShouldCallBaseService() => base.Delete_ShouldCallBaseService();

    [Fact] protected override Task GetByParameters_ReturnsFromRepository() => base.GetByParameters_ReturnsFromRepository();

    [Fact] protected override Task Create_ReturnsFromBaseService_BaseServiceCalled() => base.Create_ReturnsFromBaseService_BaseServiceCalled();

    [Fact] protected override Task Update_ReturnsFromBaseService_BaseServiceCalled() => base.Update_ReturnsFromBaseService_BaseServiceCalled();

    [Fact] protected override Task GetById_ReturnsFromBaseService_BaseServiceCalled() => base.GetById_ReturnsFromBaseService_BaseServiceCalled();
    
    public ParametersServiceSubjectDtoTests(SubjectFactory dataFactory) : base(dataFactory)
    {
    }
}