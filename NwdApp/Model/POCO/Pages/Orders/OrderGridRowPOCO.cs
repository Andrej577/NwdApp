using NwdApp.Model.DTO;

namespace NwdApp.Model.POCO.Pages.Orders;

public sealed class OrderGridRowPOCO
{
    public OrderDTO Order { get; set; } = new();
    public EmployeeDTO? Employee { get; set; }
    public CustomerDTO? Customer { get; set; }
    public ShipperDTO? Shipper { get; set; }

    public int OrderID => Order.OrderID;
    public DateTime? OrderDate => Order.OrderDate;
    public decimal? Freight => Order.Freight;
    public string CustomerName => Customer?.CompanyName ?? "Unknown";
    public string EmployeeName => Employee is null ? "Unknown" : $"{Employee.FirstName} {Employee.LastName}";
    public string ShipperName => Shipper?.CompanyName ?? "Unknown";
}