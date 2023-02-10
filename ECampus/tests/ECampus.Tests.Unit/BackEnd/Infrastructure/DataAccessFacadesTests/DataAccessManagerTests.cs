using ECampus.DataAccess.DataAccessFacades;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class DataAccessManagerTests
{
    private readonly ApplicationDbContext _context;
    private readonly DataAccessManager _sut;
    private readonly Fixture _fixture;
    private readonly IAbstractFactory<Auditory> _dataFactory;
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();

    public DataAccessManagerTests()
    {
        _dataFactory = new AuditoryFactory();
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new DataAccessManager(_context, _serviceProvider);
    }

    [Fact]
    public async Task Create_AddedToDb_CreateCalled()
    {
        var model = CreateModel();
        var createService = Substitute.For<IDataCreateService<Auditory>>();
        _serviceProvider.GetService(typeof(IDataCreateService<Auditory>)).Returns(createService);

        await _sut.CreateAsync(model);

        await createService.Received(1).CreateAsync(model, _context);
    }

    [Fact]
    public async Task Update_UpdatedInDb_IfExistsInDb()
    {
        var updatedItem = CreateModel();
        var createService = Substitute.For<IDataUpdateService<Auditory>>();
        _serviceProvider.GetService(typeof(IDataUpdateService<Auditory>)).Returns(createService);

        await _sut.UpdateAsync(updatedItem);

        await createService.Received(1).UpdateAsync(updatedItem, _context);
    }

    [Fact]
    public async Task SaveChanges_ShouldThrowInfrastructureException_WhenSaveChangesThrowsException()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new Exception());

        await new Func<Task>(() => _sut.SaveChangesAsync()).Should()
            .ThrowAsync<UnhandledInfrastructureException>();
    }

    [Fact]
    public async Task SaveChanges_ShouldThrowException_WhenSaveChangeThrowsDbUpdateConcurrencyException()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateConcurrencyException());

        await new Func<Task>(() => _sut.SaveChangesAsync()).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Error occured while saving changes to db\n" +
                         "Please, check is object(s) you are trying to update/delete exist\nError code: 404");
    }

    [Fact]
    public async Task SaveChanges_ShouldThrowException_WhenSaveChangeThrowsDbUpdateException()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateConcurrencyException());

        await new Func<Task>(() => _sut.SaveChangesAsync()).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Error occured while saving changes to db\n" +
                         "Please, check is object(s) you are trying to update/delete exist\nError code: 404");
    }

    [Fact]
    public async Task GetById_ReturnsFromSelector_IfSelectorReturnsItem()
    {
        var item = CreateModel();
        var selector = Substitute.For<ISingleItemSelector<Auditory>>();
        selector.SelectModel(1, Arg.Any<DbSet<Auditory>>()).Returns(item);
        _serviceProvider.GetService(typeof(ISingleItemSelector<Auditory>)).Returns(selector);

        var model = await _sut.GetByIdAsync<Auditory>(1);

        model.Should().Be(item);
    }

    [Fact]
    public void Delete_ShouldCall_DeleteService()
    {
        var item = CreateModel();
        var deleteService = Substitute.For<IDataDeleteService<Auditory>>();
        _serviceProvider.GetService(typeof(IDataDeleteService<Auditory>)).Returns(deleteService);
        deleteService.Delete(item, _context).Returns(item);

        var model = _sut.Delete(item);

        model.Should().Be(item);
    }

    private Auditory CreateModel()
    {
        return _dataFactory.CreateModel(_fixture);
    }
}