using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

public class Category
{
    // Primary key
    public int CategoryId { get; set; }
    
    // Atributes
    [Required, MaxLength(100)]
    public string CategoryName { get; set; } = null!;
    [MaxLength(250)]
    public string? CategoryDescription { get; set; }
 
    // A list of all the products
    public List<Product> Products { get; set; } = new();
}