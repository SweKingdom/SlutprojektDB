using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Helpers;

public class OrderHelpers
{
    public static async Task ListOrdersAsync(int page, int  pageSize)
    {
        using var db = new ShopContext();
        var query = db.Orders
            .AsNoTracking()
            .OrderBy(o => o.OrderDate)
            .Include(o => o.Customer);
        
        var totalCount = await query.CountAsync();
        var  totalPages = (int)Math.Ceiling((totalCount / (double)pageSize));
        
        var orders = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        
        Console.WriteLine($"Page {page}/{totalPages}, pageSize = {pageSize}, totalPages = {totalPages}");
        Console.WriteLine("OrderId | OrderDate | Status | CustomerName | TotalAmount");
        foreach (var order in orders)
        {
            Console.WriteLine($"{order.OrderId} | {order.OrderDate} | {order.Status} | {order.Customer.Name} | {order.TotalAmount}");
        }
    }
    
    public static async Task AddOrder() // Change so Customer List function
    {
        using var  db = new ShopContext();
        Console.WriteLine("Available Customers");

        await CustomerHelpers.ListCustomersAsync();
        
        Console.WriteLine("Choose Customer ID:");
        if (!int.TryParse(Console.ReadLine(), out var customerId))
        {
            Console.WriteLine("Invalid Customer ID");
            return;
        }

        if (!await db.Customers.AnyAsync(c => c.CustomerId == customerId))
        {
            Console.WriteLine("Customer not found");
            return;
        }

        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Pending,
        };
        
        // Add orderRows
        while (true)
        {
            Console.WriteLine("\nAvailable products:");
            await ProductHelpers.ListProductsAsync();
            
            Console.WriteLine("Chose Product ID (press enter to finish): ");
            var input =  Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }

            if (!int.TryParse(input, out var productId))
            {
                Console.WriteLine("Invalid Product ID");
                continue;
            }
            
            var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                Console.WriteLine("Product not found");
                continue;
            }
            
            Console.WriteLine("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid Quantity");
            }
            
            //ADd order row
            order.OrderRows.Add(new OrderRow
            {
                ProductId = product.ProductId,
                Quantity = quantity,
                UnitPrice = product.Pris
            });
            
            Console.WriteLine($"Added {quantity} x {product.ProductName} ({product.Pris} each)");
            
        }

        if (order.OrderRows.Count == 0)
        {
            Console.WriteLine("Order canceled cause no items added");
            return;
        }
            
        // Save DB
        db.Orders.Add(order);
        await db.SaveChangesAsync();
            
        Console.WriteLine($"\nOrder created! OrderId: {order.OrderId}");

    }
    
    public static async Task OrderByCustomerAsync(int customerId)
    {
        using var db = new ShopContext();
        var orders = await db.Orders
            .Include(o => o.OrderRows)
            .Include(o => o.Customer)
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o => o.OrderDate)
            .ToListAsync();


        Console.WriteLine("OrderId | OrderDate | Status | CustomerName | TotalAmount");
        foreach (var order in orders)
        {
            Console.WriteLine($"{order.OrderId} | {order.OrderDate} | {order.Status} | {order.Customer.Name} | {order.TotalAmount}");
        }

    }

    public static async Task ChangeOrderStatusAsync(int id)
    {
        using var db = new ShopContext();
        var order = await db.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
        {
            Console.WriteLine("Order not found");
            return;
        }
        Console.WriteLine($"\nCurrent Status: {order.Status}");
        Console.WriteLine("Available Statuses:");
        
        var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
        for (int i = 0; i < statuses.Count; i++)
        {
            Console.WriteLine($"{i}. {statuses[i]}");
        }
        
        Console.Write("Choose new status: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) ||
            choice < 0 || choice >= statuses.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }
        order.Status = statuses[choice];
        await db.SaveChangesAsync();

        Console.WriteLine($"Order {id} updated to status: {order.Status}");


        
    }
}
