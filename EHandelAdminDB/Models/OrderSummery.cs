using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Models;



// Represents a read-only projection of order information.
/// This maps to the SQL view

[Keyless] // Frivillig
public class OrderSummary
{
    // The ID of the order
    public int OrderId { get; set; }

    // Date when the order was created
    public DateTime OrderDate { get; set; }
    
    // Name of the customer who placed the order
    public string CustomerName { get; set; } = string.Empty;

    // Email of the customer
    public string CustomerEmail { get; set; } = string.Empty;

    // Total value of the order
    public decimal TotalPrice { get; set; }
}
