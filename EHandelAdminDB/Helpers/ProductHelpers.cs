using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;
namespace EHandelAdminDB.Helpers;

public class ProductHelpers
{
    public static async Task ListProductsAsync()
    {
        using var db  = new ShopContext();
    
        var rows = await db.Products
            .AsNoTracking()
            .OrderBy(p => p.ProductName)
            .ToListAsync();
    
        Console.WriteLine("Id | Name | Pris");
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.ProductId} | {row.ProductName} | {row.Pris}");    
        }
    }
    
    public static async Task ListProductSalesAsync()
    {
        using var db = new ShopContext();
        var rows = await db.ProductSalesView
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .ToListAsync();

        Console.WriteLine("Id | Name | TotalQuantitySold");
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.ProductId} | {row.Name} | {row.TotalQuantitySold}");
        }
    }
    
    public static async Task AddProductAsync()
    {
        Console.WriteLine("Name:");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(name) || name.Length > 100)
        {
            Console.WriteLine("Name is required.");
            return;
        }

        Console.WriteLine("Description:");
        var desc = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Pris:");
        if (!decimal.TryParse(Console.ReadLine(), out var pris))
        {
            Console.WriteLine("Pris must be a number.");
            return;
        }
        Console.WriteLine("Available categories:");
        await ListProductsAsync();
        Console.WriteLine("Choose CategoryId:");
        var CIDInput = Console.ReadLine()?.Trim() ?? string.Empty;
    
        if (!int.TryParse(CIDInput, out var categoryId))
        {
            Console.WriteLine("Category must be a number.");
            return;
        }


        using var db = new ShopContext();
        await db.Products.AddAsync(new Product
        {
            ProductName = name,
            Pris = pris,
            CategoryId  = categoryId
        });

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product added.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Db Error! " + ex.GetBaseException().Message);
        }
    }
    
    
    public static async Task EditProductAsync(int id)
    {
        using var db = new ShopContext();

        var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.WriteLine($"Name ({product.ProductName}):");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(name))
            product.ProductName = name;

        Console.WriteLine($"Pris ({product.Pris}):");
        var prisInput = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(prisInput) && decimal.TryParse(prisInput, out var pris))
            product.Pris = pris;

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product updated.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    public static async Task DeleteProductAsync(int id)
    {
        using var db = new ShopContext();

        var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        db.Products.Remove(product);

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product deleted.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}