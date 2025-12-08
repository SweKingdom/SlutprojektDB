using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

public class Customer
{
    public int CustomerId { get; set; }
    
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

    public List<Order> Orders { get; set; } = new();
}