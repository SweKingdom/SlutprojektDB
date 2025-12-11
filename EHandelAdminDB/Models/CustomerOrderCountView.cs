using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Models;

// Represents SQL view: CustomerOrderCountView
[Keyless]
public class CustomerOrderCountView
{
    // Customer ID from the Customer table
    public int CustomerId { get; set; }

    // Customer Name
    public string CustomerName { get; set; } = string.Empty;

    // Decrypted email, the SQL view returns plaintext values
    public string CustomerEmail { get; set; } = string.Empty;

    // Number of orders for this customer
    public int NmbrOfOrders { get; set; }
}