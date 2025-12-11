using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;

namespace EHandelAdminDB.Helpers;

public class CategoryHelpers
{

    /// <summary>
    /// Lists all categories
    /// </summary>
    public static async Task ListAsync()
    {
        var db = new ShopContext();
        var rows = await db.Categories
            .AsNoTracking()
            .OrderBy(category => category.CategoryName)
            .ToListAsync();
        Console.WriteLine("Id | Name | Description ");
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.CategoryId} | {row.CategoryName} | {row.CategoryDescription} ");
        }
    }

    /// <summary>
    /// Creates a category
    /// </summary>
    public static async Task AddAsync()
    {
        Console.WriteLine("Name: ");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(name) || name.Length > 100)
        {
            Console.WriteLine("Name is required (max 100).");
            return;
        }
        Console.WriteLine("Description (optional): ");
        var desc = Console.ReadLine() ?? string.Empty;
        using var db = new ShopContext();
        await db.Categories.AddAsync(new Category { CategoryName = name, CategoryDescription = desc });
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Category added");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine("Db Error (Maby duplicate?)! "+ exception.GetBaseException().Message);
        }
    }
    
    /// <summary>
    /// Edits a cagegorys atributes
    /// </summary>
    /// <param name="id">A specified category</param>
    public static async Task EditAsync(int id)
    {
        using var db = new ShopContext();
        var category = await db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        if (category == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }
        Console.WriteLine($"{category.CategoryName} ");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(name))
        {
            category.CategoryName = name;
        }
        Console.WriteLine($"{category.CategoryDescription} ");
        var description = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(description))
        {
            category.CategoryDescription = description;
        }
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
    
    /// <summary>
    /// Deletes a specific category
    /// </summary>
    /// <param name="id">A specified category</param>
    public static async Task DeleteAsync(int id)
    {
        using var db = new ShopContext();
        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);
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
    
    /// <summary>
    /// Creates a list of categories that contains the search term
    /// </summary>
    public static async Task SearchCategoryAsync()
    {
        Console.WriteLine($"Searching for categories");
        var category = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(category) || category.Length > 100)
        {
            Console.WriteLine("Invalid Category. Category is required (max 100).");
            return;
        }
        using var db = new ShopContext();
        var categories = await db.Categories
            .Where(c => c.CategoryName
                .ToLower()
                .Contains(category.ToLower()))
            .OrderBy(c => c.CategoryId)
            .ToListAsync();
        if (!categories.Any())
        {
            Console.WriteLine("No categories found.");
        }
        foreach (var c in categories)
        {
            Console.WriteLine($"{c.CategoryId} | {c.CategoryName} | {c.CategoryDescription} ");
        }
    }
}