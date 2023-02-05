namespace ECampus.Contracts.DataAccess;

public interface IDataAccessManagerFactory
{
    /// <summary>
    /// return DataAccessManager that directly invokes DbContext methods
    /// </summary>
    IDataAccessManager Primitive { get; }
    /// <summary>
    /// returns DataAccessManager that returns object with all required relationships using data services
    /// </summary>
    IDataAccessManager Complex { get; }
}