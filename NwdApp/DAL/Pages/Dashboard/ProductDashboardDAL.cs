using Dapper;
using NwdApp.Model.POCO.Pages.Dashboard;
using NwdApp.Service.Database;

namespace NwdApp.DAL.Pages.Dashboard
{
    public class ProductDashboardDAL
    {
        private readonly IDBConnectionFactory _dbConnectionFactory;

        public ProductDashboardDAL(IDBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<ProductDashboardPOCO>> GetTop3ProductsByRevenue()
        {
            using var conn = _dbConnectionFactory.CreateConnection();

            var sql = @"SELECT TOP 3
                        prod.ProductName,
                        SUM(ordDet.UnitPrice * ordDet.Quantity * (1 - ordDet.Discount)) AS TotalRevenue
                    FROM Products prod
                    LEFT JOIN [Order Details] ordDet ON ordDet.ProductID = prod.ProductID
                    GROUP BY prod.ProductName
                    ORDER BY TotalRevenue DESC;";

            return (await conn.QueryAsync<ProductDashboardPOCO>(sql)).ToList();
        }
    }
}
