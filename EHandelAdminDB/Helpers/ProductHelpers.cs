using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;
namespace EHandelAdminDB.Helpers;

public class ProductHelpers
{
    /// <summary>
    /// Lists all products
    /// </summary>
    public static async Task ListProductsAsync()
    {
        
        using var db  = new ShopContext();
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var rows = await db.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .OrderBy(p => p.ProductName)
            .ToListAsync();
        sw.Stop();
        Console.WriteLine($"Total  time for query {sw.ElapsedMilliseconds} ms");
        
        Console.WriteLine("Id | Name | Pris | Category");
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.ProductId} | {row.ProductName} | {row.Pris} | {row.Category?.CategoryName}");    
        }
    }
    
    /// <summary>
    /// Lists all products and the totalt Quantity sold
    /// </summary>
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
    
    /// <summary>
    /// Adds a product to be sold
    /// </summary>
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
        await CategoryHelpers.ListAsync();
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
    
    /// <summary>
    /// Edits a specific products atributes
    /// </summary>
    /// <param name="id">A specific product</param>
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
        {
            product.ProductName = name;
        }
        Console.WriteLine($"Pris ({product.Pris}):");
        var prisInput = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(prisInput) && decimal.TryParse(prisInput, out var pris))
        {
            product.Pris = pris;
        }
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

    /// <summary>
    /// Deletes a specific product
    /// </summary>
    /// <param name="id">A specific product</param>
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
    
    /// <summary>
    /// Creates a list of products that contains the search term
    /// </summary>
    public static async Task SearchProductAsync()
    {
        Console.WriteLine($"Searching for Product");
        var product = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(product) || product.Length > 150)
        {
            Console.WriteLine("Invalid Product name. Product name is required (max 100).");
            return;
        }
        using var db = new ShopContext();
        var products = await db.Products
            .Where(p => p.ProductName
                .ToLower()
                .Contains(product.ToLower()))
            .OrderBy(c => c.CategoryId)
            .ToListAsync();
        if (!products.Any())
        {
            Console.WriteLine("No product found.");
        }
        foreach (var p in products)
        {
            Console.WriteLine("ProductId | ProductName | Pris");
            Console.WriteLine($"{p.ProductId} | {p.ProductName} | {p.Pris}");
        }
    }
}