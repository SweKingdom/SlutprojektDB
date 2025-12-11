using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

public class Customer
{
    // Primary key
    public int CustomerId { get; set; }
    
    // Atributes
    [Required, MaxLength(250)]
    public string Name { get; set; }
    private string? _email;
    [Required, MaxLength(250)]
    public string? Email
    {
        get => _email is null ? null : EncryptionHelper.Decrypt(_email);
        set => _email = string.IsNullOrEmpty(value) ? null : EncryptionHelper.Encrypt(value);
    }
    [MaxLength(250)]
    public string? City { get; set; }

    
    // A list of all the orders
    public List<Order> Orders { get; set; } = new();
    
    // Hash and salt
    
    public string? CustomerSSNHash { get; set; }
    public string? CustomerSSNSalt { get; set; }
}