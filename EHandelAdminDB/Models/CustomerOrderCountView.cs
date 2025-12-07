using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Models;

[Keyless]
public class CustomerOrderCountView
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public int NmbrOfOrders { get; set; }
}