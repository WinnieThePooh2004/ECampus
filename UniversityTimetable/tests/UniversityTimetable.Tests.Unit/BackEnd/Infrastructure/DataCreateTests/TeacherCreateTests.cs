using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataCreate;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataCreateTests;

public class TeacherCreateTests
{
    private readonly TeacherCreate _sut;
    private readonly IRelationshipsDataAccess<Teacher, Subject, SubjectTeacher> _relationships;
    private readonly IDataCreate<Teacher> _baseService;

    public TeacherCreateTests()
    {
        _relationships = Substitute.For<IRelationshipsDataAccess<Teacher, Subject, SubjectTeacher>>();
        _baseService = Substitute.For<IDataCreate<Teacher>>();
        _sut = new TeacherCreate(_baseService, _relationships);
    }

    [Fact]
    public async Task Create_BaseServiceCalled_RelationshipsRepositoryCalled()
    {
        var model = new Teacher();
        var context = Substitute.For<ApplicationDbContext>();
        await _sut.CreateAsync(model, context);

        _relationships.Received(1).CreateRelationModels(model);
        await _baseService.Received(1).CreateAsync(model, context);
    }
}