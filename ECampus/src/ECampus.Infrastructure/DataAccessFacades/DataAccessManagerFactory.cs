using ECampus.Contracts.DataAccess;

namespace ECampus.Infrastructure.DataAccessFacades;

public class DataAccessManagerFactory : IDataAccessManagerFactory
{
    private readonly Lazy<IDataAccessManager> _complex;
    private readonly Lazy<PrimitiveDataAccessManager> _primitive;

    public DataAccessManagerFactory(Lazy<IDataAccessManager> complex, Lazy<PrimitiveDataAccessManager> primitive)
    {
        _complex = complex;
        _primitive = primitive;
    }

    public IDataAccessManager Primitive => _complex.Value;
    public IDataAccessManager Complex => _primitive.Value;
}