using ECampus.Infrastructure;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class BaseDataAccessFacadeTests
{
    private readonly ApplicationDbContext _context;
    private readonly BaseDataAccessFacade<Auditory> _sut;
    private readonly Fixture _fixture;
    private readonly IAbstractFactory<Auditory> _dataFactory;
    private readonly ISingleItemSelector<Auditory> _selector;
    private readonly IDataUpdateService<Auditory> _updateService = Substitute.For<IDataUpdateService<Auditory>>();
    private readonly IDataCreateService<Auditory> _createService = Substitute.For<IDataCreateService<Auditory>>();
    private readonly IDataDeleteService<Auditory> _deleteService = Substitute.For<IDataDeleteService<Auditory>>();

    public BaseDataAccessFacadeTests()
    {
        _dataFactory = new AuditoryFactory();
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _context = Substitute.For<ApplicationDbContext>();
        _selector = Substitute.For<ISingleItemSelector<Auditory>>();
        _sut = new BaseDataAccessFacade<Auditory>(_context, _selector, _deleteService, _updateService,
            _createService);
    }

    [Fact]
    public async Task Create_AddedToDb_CreateCalled()
    {
        var model = CreateModel();
        model.Id = 0;

        await _sut.CreateAsync(model);

        await _createService.Received(1).CreateAsync(model, _context);
    }

    [Fact]
    public async Task Update_UpdatedInDb_IfExistsInDb()
    {
        var updatedItem = CreateModel();

        var result = await _sut.UpdateAsync(updatedItem);

        result.Should().Be(updatedItem);
    }

    [Fact]
    public async Task Update_ShouldThrowInfrastructureException_WhenSaveChangesThrowsException()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new Exception());

        await new Func<Task>(() => _sut.UpdateAsync(new Auditory())).Should()
            .ThrowAsync<InfrastructureExceptions>();
    }

    [Fact]
    public async Task Update_ShouldThrowException_IfSaveChangeThrowsException()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateConcurrencyException());

        await new Func<Task>(() => _sut.UpdateAsync(new Auditory())).Should()
            .ThrowAsync<ObjectNotFoundByIdException>();
    }

    [Fact]
    public async Task GetById_ReturnsFromSelector_IfSelectorReturnsItem()
    {
        var item = CreateModel();
        _selector.SelectModel(1, Arg.Any<DbSet<Auditory>>()).Returns(item);

        var model = await _sut.GetByIdAsync(1);

        model.Should().Be(item);
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenSelectorReturnsNull()
    {
        await new Func<Task>(() => _sut.GetByIdAsync(-1)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>();
    }

    [Fact]
    public async Task Delete_ShouldCall_DeleteService()
    {
        var item = CreateModel();
        _deleteService.DeleteAsync(1, Arg.Any<DbContext>()).Returns(item);

        var model = await _sut.DeleteAsync(1);

        model.Should().Be(item);
    }

    [Fact]
    public async Task Delete_ShouldThrowException_WhenSaveChangeThrow()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateConcurrencyException());

        await new Func<Task>(() => _sut.DeleteAsync(10)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>();
    }

    private Auditory CreateModel()
    {
        return _dataFactory.CreateModel(_fixture);
    }
}