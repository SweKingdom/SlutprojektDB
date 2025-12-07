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

}