using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Helpers;

public class CategoryHelpers
{
    // READ: Lista alla kategorier
    public static async Task ListAsync()
    {
        var db = new ShopContext();
    
        // AsNoTracking = snabare för read-only scenarion. ( Ingen change tracking)
        var rows = await db.Categories.AsNoTracking().OrderBy(category => category.CategoryName).ToListAsync();
        Console.WriteLine("Id | Name | Description ");
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.CategoryId} | {row.CategoryName} | {row.CategoryDescription} ");
        }
    }

//CREATE: Lägg till en ny kategori
    public static async Task AddAsync()
    {
        Console.WriteLine("Name: ");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;
    
        // Enkel validering
        if (string.IsNullOrEmpty(name) || name.Length > 100)
        {
            Console.WriteLine("Name is required (max 100).");
            return;
        }
        Console.WriteLine("Description (optional): ");
        var desc = Console.ReadLine()?.Trim() ?? string.Empty;
    
        using var db = new ShopContext();
        await db.Categories.AddAsync(new Category { CategoryName = name, CategoryDescription = desc });
        try
        {
            // Spara våra ändringar; Trigga en INSERT + all validering/constraints i databasen
            await db.SaveChangesAsync();
            Console.WriteLine("Category added");
        }
        catch (DbUpdateException exception)
        {
            // Hit kommer vi tex om UNIQUE - Indexet op CategoryName bryts
            Console.WriteLine("Db Error (Maby duplicate?)! "+ exception.GetBaseException().Message);
        }
    }
    
    public static async Task EditAsync(int id)
    {
        using var db = new ShopContext();
    
        // Hämta raden vi vill uppdatera
        var category = await db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        if (category == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }
    
        // Visar nuvarande värden; Uppdatera namn för en specifik category
        Console.WriteLine($"{category.CategoryName} ");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(name))
        {
            category.CategoryName = name;
        }
    
        // Uppdaterar description för en specifik category; TODO: FIX ME (NULL, not required)
        Console.WriteLine($"{category.CategoryDescription} ");
        var description = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(description))
        {
            category.CategoryDescription = description;
        }

        // Uppdaterar 
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Category edited");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
    
    
    public static async Task DeleteAsync(int id)
    {
        using var db = new ShopContext();
    
        var category = await db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }
        db.Categories.Remove(category);
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Category deleted");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    
}