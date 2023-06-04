using CartingService.DAL.Interfaces;
using LiteDB;

namespace CartingService.DAL.Implementation;

public class LiteDBConnectionProvider : ILiteDBConnectionProvider
{
    private readonly string _connectionString;

    public LiteDBConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ILiteDatabase GetConnection()
    {
        return new LiteDatabase(_connectionString);
    }
}
