using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

/// <summary>
/// Represents a product available for purchase
/// </summary>
public class Product
{
    // Primary Key
    public int ProductId { get; set; }
    
    // Product price
    [Required]
    public decimal Pris {get ; set;}

    // Name of the product
    [Required, MaxLength(150)]
    public string ProductName { get; set; } = null!;
    
    // The category Id this product belongs to
    public int? CategoryId { get; set; }

    // Reference to the products category
    public Category? Category { get; set; }
}