using MudBlazor;
using MudBlazor.Services;
using NwdApp.Components;
using NwdApp.DAL.Pages;
using NwdApp.DAL.Pages.Dashboard;
using NwdApp.Service.Database;
using NwdApp.Service.SingalR;
using NwdApp.Service.Util;
using NwdApp.Services;

namespace NwdApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();

            builder.Services.AddMudServices();
            builder.Services.AddScoped<AppThemeState>();

            builder.Services.AddScoped<IDBConnectionFactory, DbConnectionFactory>();

            builder.Services.AddScoped<OrdersDashboardDAL>();
            builder.Services.AddScoped<EmployeeDashboardDAL>();
            builder.Services.AddScoped<ProductDashboardDAL>();
            builder.Services.AddScoped<ShippersDashboardDAL>();
            builder.Services.AddScoped<RegionsDashboardDAL>();

            builder.Services.AddScoped<OrdersPageDAL>();
            builder.Services.AddScoped<ShippersPageDAL>();

            builder.Services.AddSignalR();
            builder.Services.AddHostedService<OrderWatcherService>();

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.PreventDuplicates = true;
                config.SnackbarConfiguration.NewestOnTop = true;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 3000;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

            app.MapHub<DashboardHub>("/dashboardHub");


            app.Run();
        }
    }
}
