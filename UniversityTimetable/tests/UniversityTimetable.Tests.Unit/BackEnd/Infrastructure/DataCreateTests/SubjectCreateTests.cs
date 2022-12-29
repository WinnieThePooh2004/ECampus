using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataCreate;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataCreateTests;

public class SubjectCreateTests
{
    private readonly SubjectCreate _sut;
    private readonly IRelationshipsDataAccess<Subject, Teacher, SubjectTeacher> _relationships;
    private readonly IDataCreate<Subject> _baseService;

    public SubjectCreateTests()
    {
        _relationships = Substitute.For<IRelationshipsDataAccess<Subject, Teacher, SubjectTeacher>>();
        _baseService = Substitute.For<IDataCreate<Subject>>();
        _sut = new SubjectCreate(_baseService, _relationships);
    }

    [Fact]
    public async Task Create_BaseServiceCalled_RelationshipsRepositoryCalled()
    {
        var model = new Subject();
        var context = Substitute.For<ApplicationDbContext>();
        await _sut.CreateAsync(model, context);

        _relationships.Received(1).CreateRelationModels(model);
        await _baseService.Received(1).CreateAsync(model, context);
    }
}