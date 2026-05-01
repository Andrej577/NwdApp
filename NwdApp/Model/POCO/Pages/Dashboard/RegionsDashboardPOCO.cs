namespace NwdApp.Model.POCO.Pages.Dashboard
{
    public class RegionsDashboardPOCO
    {
        public required string RegionDescription { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int OrdersCount { get; set; }
    }
}
