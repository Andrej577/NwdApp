using Dapper;
using Microsoft.Data.SqlClient;
using MudBlazor;
using NwdApp.Model.DTO;
using NwdApp.Model.POCO.Pages.Orders;

namespace NwdApp.DAL.Pages;

public class OrdersPageDAL
{
    private readonly string _connectionString;

    public OrdersPageDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<GridData<OrderGridRowPOCO>> GetOrdersGridPageAsync(GridState<OrderGridRowPOCO> state, int paginationNumber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var conn = new SqlConnection(_connectionString);

        var skip = state.Page * state.PageSize;
        var take = state.PageSize <= 0 ? paginationNumber : state.PageSize;

        const string countSql = """
        SELECT COUNT(*)
        FROM Orders ord
        WHERE ord.OrderDate IS NOT NULL;
        """;

        const string sql = """
        SELECT
            ord.OrderID,
            ord.CustomerID,
            ord.EmployeeID,
            ord.OrderDate,
            ord.RequiredDate,
            ord.ShippedDate,
            ord.ShipVia,
            ord.Freight,
            ord.ShipName,
            ord.ShipAddress,
            ord.ShipCity,
            ord.ShipRegion,
            ord.ShipPostalCode,
            ord.ShipCountry,

            e.EmployeeID AS EmployeeSplit,
            e.EmployeeID,
            e.LastName,
            e.FirstName,
            e.Title,
            e.City,
            e.Country,

            cus.CustomerID AS CustomerSplit,
            cus.CustomerID,
            cus.CompanyName,
            cus.ContactName,
            cus.ContactTitle,
            cus.Address,
            cus.City,
            cus.Region,
            cus.PostalCode,
            cus.Country,
            cus.Phone,
            cus.Fax,

            s.ShipperID AS ShipperSplit,
            s.ShipperID,
            s.CompanyName,
            s.Phone
        FROM Orders ord
        LEFT JOIN Employees e ON e.EmployeeID = ord.EmployeeID
        LEFT JOIN Customers cus ON cus.CustomerID = ord.CustomerID
        LEFT JOIN Shippers s ON s.ShipperID = ord.ShipVia
        WHERE ord.OrderDate IS NOT NULL
        ORDER BY ord.OrderDate DESC, ord.OrderID DESC
        OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
        """;

        var totalItems = await conn.ExecuteScalarAsync<int>(countSql);
        cancellationToken.ThrowIfCancellationRequested();

        var rows = await conn.QueryAsync<OrderDTO, EmployeeDTO, CustomerDTO, ShipperDTO, OrderGridRowPOCO>(sql, MapOrderGridRow, new { Skip = skip, Take = take }, splitOn: "EmployeeSplit,CustomerSplit,ShipperSplit");

        return new GridData<OrderGridRowPOCO> { Items = rows.ToList(), TotalItems = totalItems };
    }

    private static OrderGridRowPOCO MapOrderGridRow(OrderDTO order, EmployeeDTO employee, CustomerDTO customer, ShipperDTO shipper)
    {
        return new OrderGridRowPOCO { Order = order, Employee = employee, Customer = customer, Shipper = shipper };
    }
}