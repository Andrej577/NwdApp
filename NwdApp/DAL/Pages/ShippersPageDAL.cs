using Dapper;
using Microsoft.Data.SqlClient;
using NwdApp.Model.DTO;

namespace NwdApp.DAL.Pages;

public class ShippersPageDAL
{
    private readonly string _connectionString;

    public ShippersPageDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<ShipperDTO>> GetShippersAsync()
    {
        await using var conn = new SqlConnection(_connectionString);

        const string sql = """
            SELECT ShipperID, CompanyName, Phone
            FROM Shippers
            ORDER BY CompanyName;
            """;

        return (await conn.QueryAsync<ShipperDTO>(sql)).ToList();
    }

    public async Task<int> InsertShipperAsync(ShipperDTO shipper)
    {
        await using var conn = new SqlConnection(_connectionString);

        const string sql = """
            INSERT INTO Shippers (CompanyName, Phone)
            OUTPUT INSERTED.ShipperID
            VALUES (@CompanyName, @Phone);
            """;

        return await conn.ExecuteScalarAsync<int>(sql, shipper);
    }
}