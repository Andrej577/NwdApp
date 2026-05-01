using System.Data;

namespace NwdApp.Service.Database
{
    public interface IDBConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
