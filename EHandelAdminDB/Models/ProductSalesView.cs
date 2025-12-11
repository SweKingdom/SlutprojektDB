namespace EHandelAdminDB.Models;

/// <summary>
/// Represents SQL view: ProductSalesView
/// Total sales per product
/// </summary>
public class ProductSalesView
{
    // Unique ID of the product
    public int ProductId { get; set; }
    
    // Name of the product
    public string Name { get; set; } = null!;
    
    // Total number of units sold across all orders
    public int TotalQuantitySold { get; set; }

}