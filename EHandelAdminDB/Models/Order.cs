using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;


public enum OrderStatus
{
    Pending,
    Paid,
    Shipped
}

public class Order
{
    // PK
    public int OrderId { get; set; }
    
    // FK
    public int CustomerId { get; set; }
    
    public DateTime OrderDate { get; set; }

    [Required]
    public OrderStatus Status { get; set; }
    
    public decimal TotalAmount  { get; set; }
    
    public Customer? Customer { get; set; }

    public List<OrderRow> OrderRows { get; set; } = new();
}