using NwdApp.Model.DTO;

namespace NwdApp.Model.POCO
{
    public class EmployeesDashboardPOCO : EmployeeDTO
    {
        public required string EmployeeFullName {  get; set; }
        public required int OrdersCount { get; set; }
    }
}
