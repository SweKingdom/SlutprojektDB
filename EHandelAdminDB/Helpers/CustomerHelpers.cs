using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Helpers;

public class CustomerHelpers
{


    // Customer List funktion
    public static async Task ListCustomersAsync()
    {
        using var db = new ShopContext();
        var customers = await db.Customers
            .AsNoTracking()
            .OrderBy(c => c.CustomerId)
            .ToListAsync();
        Console.WriteLine("Id | Name | Email | City");
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId} | {customer.Name} | {customer.Email} | {customer.City}");
        }
    }
    
    // Add Customer
    public static async Task AddCustomerAsync()
    {
        var db = new ShopContext();
        Console.Write("Enter Customer Name: ");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;
        
        if (string.IsNullOrEmpty(name) || name.Length > 100)
        {
            Console.WriteLine("Name is required.");
            return;
        }
        
        Console.Write("Enter Customer Email: ");
        var email = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrEmpty(email) || name.Length > 250)
        {
            Console.WriteLine("Email is required.");
            return;
        }
        
        Console.WriteLine("City:");
        var city = Console.ReadLine() ?? string.Empty;
        if (city.Length > 250)
        {
            Console.WriteLine("City name can't be longer than 250 characters.");
            return;
        }

        await db.Customers.AddAsync(new Customer
        {
            Name = name,
            Email = email,
            City = city
        });

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer added.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Db Error! " + ex.GetBaseException().Message);
        }
    }
    
    public static async Task ListCustomerOrderCountsAsync()
    {
        using var db = new ShopContext();

        var rows = await db.CustomerOrderCountViews
            .AsNoTracking()
            .OrderBy(r => r.CustomerId)
            .ToListAsync();

        Console.WriteLine("CustomerId | Name | Email | NmbrOfOrders");

        foreach (var r in rows)
        {
            Console.WriteLine($"{r.CustomerId} | {r.CustomerName} | {r.CustomerEmail} | {r.NmbrOfOrders}");
        }
    }

}