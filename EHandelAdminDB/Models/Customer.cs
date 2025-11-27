using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

public class Customer
{
    public int CustomerId { get; set; }
    
    [Required, StringLength(250)]
    public string Name { get; set; }

    [Required, StringLength(250)]
    public string Email { get; set; } = null!;
    
    [StringLength(250)]
    public string? City { get; set; }

    public List<Order> Orders { get; set; } = new();
}