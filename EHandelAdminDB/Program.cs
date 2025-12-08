using EHandelAdminDB;
using EHandelAdminDB.Helpers;
using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;


Console.WriteLine("DB: " + Path.Combine(AppContext.BaseDirectory, "shop.db"));
using (var db = new ShopContext())
{
    await db.Database.MigrateAsync();
    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product {Pris = 10, ProductName = "Apple"},
            new Product {Pris = 20, ProductName = "Orange"},
            new Product {Pris = 30, ProductName = "Banana"},
            new Product {Pris = 40, ProductName = "Milk"},
            new Product {Pris = 50, ProductName = "Musli"},
            new Product {Pris = 60, ProductName = "Water"}
        );
        await db.SaveChangesAsync();
        Console.WriteLine("Seeded DB");
    }
}

while (true)
{
    Console.WriteLine("\nCommands: customers | +customer | EditCustomer <Id> | DeleteCustomer <Id> | customerordercounts");
    Console.WriteLine("Commands: Orders | OrderDetail <id> | AddOrder");
    Console.WriteLine("Commands: ListProducts | OrderCustomerSearch <id> | obs <status> | listproductsales");
    Console.WriteLine(">");
    var line = Console.ReadLine()?.Trim() ?? string.Empty;
    // Hoppa över tomma rader
    if (string.IsNullOrEmpty(line))
    {
        continue;
    }

    if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break; // Avsluta programmet, hoppa ur loopen
    }
    
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var cmd = parts[0].ToLowerInvariant();

    switch (cmd)
    {
        case "customers":
            await CustomerHelpers.ListCustomersAsync();
            break;

        case "+customer":
            await CustomerHelpers.AddCustomerAsync();
            break;

        case "orders":
            Console.WriteLine("Please write the page");
            var pageLoan = int.Parse((Console.ReadLine()));
            Console.WriteLine("Please write the page size");
            var pageSizeLoan = int.Parse((Console.ReadLine()));
            await OrderHelpers.ListOrdersAsync(pageLoan, pageSizeLoan);
            break;

        case "addorder":
            await OrderHelpers.AddOrder();
            break;
        case "listproducts":
            await ProductHelpers.ListProductsAsync();
            break;
        case "customerordercounts":
            await CustomerHelpers.ListCustomerOrderCountsAsync();
            break;
        case "listproductsales":
            await ProductHelpers.ListProductSalesAsync();
            break;
        case "exit":
            return;

        default:
            Console.WriteLine("Unknown command.");
            break;
    }
}