using ECampus.DataAccess.DataDeleteServices;
using ECampus.Domain.Models;
using ECampus.Infrastructure;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.DataDelete;

public class DataDeleteTests 
{
    private readonly DataDeleteService<Auditory> _sut;

    public DataDeleteTests()
    {
        _sut = new DataDeleteService<Auditory>();
    }

    [Fact]
    public void Delete_ModelRemovedFromContext()
    {
        var auditory = new Auditory();
        var context = Substitute.For<ApplicationDbContext>();
        
        _sut.Delete(auditory, context);

        context.Received(1).Remove(auditory);
    }
}