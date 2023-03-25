using LiteDB;

namespace CartingService.DAL.Interfaces;

public interface ILiteDBConnectionProvider
{
    ILiteDatabase GetConnection();
}
