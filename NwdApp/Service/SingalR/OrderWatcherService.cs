using Dapper;
using Microsoft.AspNetCore.SignalR;
using NwdApp.Service;
using NwdApp.Service.Database;
using NwdApp.Service.SingalR;

namespace NwdApp.Services;

public class OrderWatcherService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<DashboardHub> _hubContext;
    private int _lastKnownOrderId;

    public OrderWatcherService(
        IServiceScopeFactory scopeFactory,
        IHubContext<DashboardHub> hubContext)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeLastKnownOrderIdAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbFactory = scope.ServiceProvider.GetRequiredService<IDBConnectionFactory>();

                using var conn = dbFactory.CreateConnection();

                var latestOrderId = await conn.ExecuteScalarAsync<int>(
                    "SELECT ISNULL(MAX(OrderID), 0) FROM Orders");

                if (latestOrderId > _lastKnownOrderId)
                {
                    _lastKnownOrderId = latestOrderId;

                    await _hubContext.Clients.All.SendAsync(
                        "OrderInserted",
                        latestOrderId,
                        cancellationToken: stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task InitializeLastKnownOrderIdAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbFactory = scope.ServiceProvider.GetRequiredService<IDBConnectionFactory>();

        using var conn = dbFactory.CreateConnection();

        _lastKnownOrderId = await conn.ExecuteScalarAsync<int>(
            "SELECT ISNULL(MAX(OrderID), 0) FROM Orders");
    }
}