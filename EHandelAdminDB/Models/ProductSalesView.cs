namespace EHandelAdminDB.Models;

public class ProductSalesView
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public int TotalQuantitySold { get; set; }

}