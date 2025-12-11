using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

/// <summary>
/// Represents a single line item within an order, linking a product,
/// its purchased quantity, and price at the time of ordering.
/// </summary>
public class OrderRow
{
    // Primary key
    public int OrderRowId  { get; set; }
    
    // Foreign Key
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    
    // Number of units purchased
    [Required]
    public int Quantity { get; set; }
    
    // Price per unit at time of purchase
    [Required]
    public decimal UnitPrice { get; set; }
    
    // Reference to the order this row belongs to
    public Order? Order { get; set; }
    
    // Reference to the product being purchased
    public Product? Product { get; set; }
}