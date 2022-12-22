using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.TestDatabase;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure
{
    public class BaseSubjectRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly BaseSubjectRepository _repository;

        public BaseSubjectRepositoryTests()
        {
            _context = TestDatabaseFactory.CreateContext();
            _repository = default!;
            _context.Database.EnsureCreated();
            _context.SeedData();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        //[Fact]
        public async Task GetById_ReturnsFromDb()
        {
            var entity = await _repository.GetByIdAsync(1);

            entity.Should().BeEquivalentTo(DataSeeder.Subjects.First(), opt => opt.ComparingByMembers<Subject>());
        }

        //[Fact]
        public async Task GetById_ObjectNotFound_ShouldThrowException()
        {
            await new Func<Task>(async () => await _repository.GetByIdAsync(100)).Should()
                .ThrowAsync<ObjectNotFoundByIdException>()
                .WithMessage(new ObjectNotFoundByIdException(typeof(Subject), 100).Message);
        }
    }
}
