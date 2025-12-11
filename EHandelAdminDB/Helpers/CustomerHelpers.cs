using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Helpers;

public class CustomerHelpers
{
    /// <summary>
    /// Creates a list to read all customers ordered by Customer ID
    /// </summary>
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
    
    /// <summary>
    /// Creates a new customer
    /// </summary>
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
        Console.Write("Enter SSN (optional): ");
        var ssn = Console.ReadLine()?.Trim();
        if (ssn.Length > 12)
        {
            Console.WriteLine("SSN cant be longer than 12 characters.");
            return;
        }

        string? salt = null;
        string? hash =  null;
        
        if (!string.IsNullOrWhiteSpace(ssn))
        {
            salt = HashingHelper.GenerateSalt();
            hash = HashingHelper.HashWithSalt(ssn, salt);
        }


        var customer = new Customer
        {
            Name = name,
            Email = email,
            City = city,
            CustomerSSNSalt = salt,
            CustomerSSNHash = hash
        };
        

        db.Customers.Add(customer);
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
    
    /// <summary>
    /// Lists customers by total order count
    /// </summary>
    public static async Task ListCustomerOrderCountsAsync()
    {
        using var db = new ShopContext();
        var rows = await db.CustomerOrderCountViews
            .AsNoTracking()
            .OrderBy(r => r.CustomerId)
            .ToListAsync();
        Console.WriteLine("CustomerId | Name | Number Of Orders");
        foreach (var r in rows)
        {
            Console.WriteLine($"{r.CustomerId} | {r.CustomerName} | {r.NmbrOfOrders}");
        }
    }
}