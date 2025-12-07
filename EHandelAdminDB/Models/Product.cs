using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

public class Product
{
    public int ProductId { get; set; }
    
    [Required]
    public decimal Pris {get ; set;}

    [Required, MaxLength(150)]
    public string ProductName { get; set; } = null!;
}