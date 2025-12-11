using System.ComponentModel.DataAnnotations;

namespace EHandelAdminDB.Models;

/// <summary>
/// Represents the status of an order
/// </summary>
public enum OrderStatus
{
    Pending,
    Paid,
    Shipped
}

/// <summary>
/// Represents a customers order
/// </summary>
public class Order
{
    // Primary Key
    public int OrderId { get; set; }
    
    // Foreign Key
    public int CustomerId { get; set; }
    
    // Date when the order was created
    public DateTime OrderDate { get; set; }

    // Curent status of the order
    [Required]
    public OrderStatus Status { get; set; }
    
    // Total amount for the order
    public decimal TotalAmount  { get; set; }
    
    // The customer entity assosiated with this order
    public Customer? Customer { get; set; }

    // List of all order rows
    public List<OrderRow> OrderRows { get; set; } = new();
}