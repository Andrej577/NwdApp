using Dapper;
using NwdApp.Model.DTO;
using NwdApp.Model.POCO;
using NwdApp.Service.Database;

namespace NwdApp.DAL.Dashboard
{
    public class OrdersDashboardDAL
    {
        private readonly IDBConnectionFactory _dbConnectionFactory;

        public OrdersDashboardDAL(IDBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<OrderDTO>> GetLast20000Orders()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            
            var sql = """
            SELECT TOP (20000) cus.ContactName, emp.FirstName + ' ' + emp.LastName as Employee, ord.OrderDate, ord.ShipCountry 
            FROM Orders ord 
            JOIN Employees emp ON emp.EmployeeID = ord.EmployeeID
            JOIN Customers cus ON cus.CustomerID = ord.CustomerID
            ORDER BY ord.OrderDate DESC;
            """;

            var result = await connection.QueryAsync<OrderDTO>(sql);
            return result.ToList();
        }

        public async Task<List<OrdersListDahsboardPOCO>> GetLast3Orders()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var sql = """
                    SELECT TOP 8 c.ContactName, o.OrderDate  FROM Orders o
                    INNER JOIN Customers c ON c.CustomerID = o.CustomerID 
                    ORDER BY o.OrderID DESC
            """;

            var result = await connection.QueryAsync<OrdersListDahsboardPOCO>(sql);
            return result.ToList();
        }
    }
}
