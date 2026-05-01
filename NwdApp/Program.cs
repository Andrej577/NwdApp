using Microsoft.AspNetCore.Connections;
using MudBlazor.Services;
using NwdApp.Components;
using NwdApp.DAL.Dashboard;
using NwdApp.Model.POCO;
using NwdApp.Service.Database;
using NwdApp.Service.SingalR;
using NwdApp.Services;

namespace NwdApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddMudServices();

            builder.Services.AddScoped<IDBConnectionFactory, DbConnectionFactory>();

            builder.Services.AddScoped<OrdersDashboardDAL>();
            builder.Services.AddScoped<EmployeeDashboardDAL>();
            builder.Services.AddScoped<ProductDashboardDAL>();
            builder.Services.AddScoped<ShippersDashboardDAL>();
            builder.Services.AddScoped<RegionsDashboardDAL>();

            builder.Services.AddSignalR();
            builder.Services.AddHostedService<OrderWatcherService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapHub<DashboardHub>("/dashboardHub");


            app.Run();
        }
    }
}
