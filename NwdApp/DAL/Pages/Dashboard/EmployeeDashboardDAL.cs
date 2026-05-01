using Dapper;
using NwdApp.Model.POCO.Pages.Dashboard;
using NwdApp.Service.Database;

namespace NwdApp.DAL.Pages.Dashboard
{
    public class EmployeeDashboardDAL
    {
        private readonly IDBConnectionFactory _dbConnectionFactory;

        public EmployeeDashboardDAL(IDBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<EmployeesDashboardPOCO>> GetTop3Employees()
        {
            using var conn = _dbConnectionFactory.CreateConnection();

            var sql = @"SELECT TOP 3
                            emp.EmployeeID,
                            emp.FirstName + ' ' + emp.LastName AS EmployeeFullName,
                            COUNT(ord.OrderID) AS OrdersCount
                        FROM Employees emp
                        LEFT JOIN Orders ord ON ord.EmployeeID = emp.EmployeeID
                        GROUP BY
                            emp.EmployeeID,
                            emp.FirstName,
                            emp.LastName
                        ORDER BY COUNT(ord.OrderID) DESC;";

            return (await conn.QueryAsync<EmployeesDashboardPOCO>(sql)).ToList();
        }
    }
}
