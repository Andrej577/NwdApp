using Dapper;
using NwdApp.Model.DTO;
using NwdApp.Service.Database;

namespace NwdApp.DAL
{
    public class OrdersDAL
    {
        private readonly IDBConnectionFactory _dbConnectionFactory;
        public OrdersDAL(IDBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<OrderDTO>> GetOrders()
        {
            using var conn = _dbConnectionFactory.CreateConnection();

            var sql = "SELECT * FROM Orders";

            return [.. (await conn.QueryAsync<OrderDTO>(sql))];
        }
    }
}
