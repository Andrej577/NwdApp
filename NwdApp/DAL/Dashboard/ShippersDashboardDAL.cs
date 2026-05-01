using Dapper;
using NwdApp.Model.POCO;
using NwdApp.Service.Database;

namespace NwdApp.DAL.Dashboard
{
    public class ShippersDashboardDAL
    {
        private readonly IDBConnectionFactory _dbConnectionFactory;

        public ShippersDashboardDAL(IDBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<ShippersDashboardPOCO>> GetTop3Shippers()
        {
            using var conn = _dbConnectionFactory.CreateConnection();

            var sql = @"SELECT TOP 3 sh.CompanyName, COUNT(ord.OrderID ) AS OrdersCount FROM Shippers sh 
                            LEFT JOIN Orders ord ON ord.ShipVia = sh.ShipperID 
                            GROUP BY sh.CompanyName";

            return (await conn.QueryAsync<ShippersDashboardPOCO>(sql)).ToList();
        }
    }
}

