using Dapper;
using NwdApp.Model.POCO;
using NwdApp.Service.Database;

namespace NwdApp.DAL
{
    public class RegionsDashboardDAL
    {
        public readonly IDBConnectionFactory _dbConnectionFactory;

        public RegionsDashboardDAL(IDBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<RegionsDashboardPOCO>> GetOrdersByRegionFrom20000Orders()
        {
            using var con = _dbConnectionFactory.CreateConnection();

            var sql = """
                WITH Last20000Orders AS
                (
                    SELECT TOP 20000
                        ord.OrderID,
                        ord.EmployeeID,
                        ord.OrderDate
                    FROM Orders ord
                    WHERE ord.OrderDate IS NOT NULL
                    ORDER BY ord.OrderDate DESC
                )
                SELECT
                    ISNULL(r.RegionDescription, 'Unknown') AS RegionDescription,
                    YEAR(lo.OrderDate) AS [Year],
                    MONTH(lo.OrderDate) AS [Month],
                    COUNT(DISTINCT lo.OrderID) AS OrdersCount
                FROM Last20000Orders lo
                LEFT JOIN Employees em ON em.EmployeeID = lo.EmployeeID
                LEFT JOIN EmployeeTerritories et ON et.EmployeeID = em.EmployeeID
                LEFT JOIN Territories t ON t.TerritoryID = et.TerritoryID
                LEFT JOIN Region r ON r.RegionID = t.RegionID
                GROUP BY
                    ISNULL(r.RegionDescription, 'Unknown'),
                    YEAR(lo.OrderDate),
                    MONTH(lo.OrderDate)
                ORDER BY [Year], [Month], RegionDescription
                """;

            return (await con.QueryAsync<RegionsDashboardPOCO>(sql)).ToList();
        }
    }
}
