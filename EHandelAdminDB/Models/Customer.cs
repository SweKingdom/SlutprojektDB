using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

public class Customer
{
    public int CustomerId { get; set; }
    
    [Required, MaxLength(250)]
    public string Name { get; set; }

    [Required, MaxLength(250)]
    public string Email { get; set; } = null!;
    
    [MaxLength(250)]
    public string? City { get; set; }

    public List<Order> Orders { get; set; } = new();
}