using EHandelAdminDB;
using EHandelAdminDB.Helpers;
using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;


Console.WriteLine("DB: " + Path.Combine(AppContext.BaseDirectory, "shop.db"));
using (var db = new ShopContext())
{
    await db.Database.MigrateAsync();
    
    // Seed products if database is empty
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
    
    // if(await db.Orders.CountAsync() < 100)
    // {
    //    var moreOrder = new List<Order>();
    //    for (int i = 1; i <= 100; i++)
    //    {
    //        moreOrder.Add(new Order
    //        {
    //            CustomerId = 1,
    //            OrderDate = DateTime.Now,
    //            Status = OrderStatus.Pending
    //        });
    //    }
    //    db.Orders.AddRange(moreOrder);
    //    await db.SaveChangesAsync();
    //    Console.WriteLine("Seeded db with more orders!");

    //}}
}

// Main menu LOOP
while (true)
{
    Console.WriteLine("\nCommands: Customer Menu - 1");
    Console.WriteLine("Commands: Order Menu - 2");
    Console.WriteLine("Commands: Category Menu - 3");
    Console.WriteLine("Commands: Product Menu - 4");
    Console.WriteLine("Commands: Exit");
    Console.WriteLine(">");

    var choice = Console.ReadLine()?.ToLower();
    if (choice == "exit")
        break;
    if (choice == "1")
        await CustomerMenu();
    else if (choice == "2")
        await OrderMenu();
    else if (choice == "3")
        await CategoryMenu();
    else if (choice == "4")
        await ProductMenu();
    else
        Console.WriteLine("Invalid option. Try again.");
    
    // Customer menu loop
    static async Task CustomerMenu()
    {
        while (true)
        {
            Console.WriteLine("\nCommands: Add | List | ListCount | Exit");
            Console.WriteLine(">");
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();
            switch (cmd)
            {
                case "add":
                    await CustomerHelpers.AddCustomerAsync();
                    break;
                case "list":
                    await CustomerHelpers.ListCustomersAsync();
                    break;
                case "listcount":
                    await CustomerHelpers.ListCustomerOrderCountsAsync();
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
    
    // Order menu loop
    static async Task OrderMenu()
    {
        while (true)
        {
            Console.WriteLine("\nCommands: Add | List | ListCustomer <id> | Status <id> | Product <id> | Exit");
            Console.WriteLine(">");
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();
            switch (cmd)
            {
                case "add":
                    await OrderHelpers.AddOrder();
                    break;
                case "list":
                    int pageLoan;
                    int pageSizeLoan;
                    Console.WriteLine("Please write the page");
                    while (!int.TryParse(Console.ReadLine(), out pageLoan))
                    {
                        Console.WriteLine("Invalid number. Please enter digits only:");
                    }
                    Console.WriteLine("Please write the page size");
                    while (!int.TryParse(Console.ReadLine(), out pageSizeLoan))
                    {
                        Console.WriteLine("Invalid number. Please enter digits only:");
                    }
                    await OrderHelpers.ListOrdersAsync(pageLoan, pageSizeLoan);
                    break;
                case "listcustomer":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idLOrder))
                    {
                        Console.WriteLine("Usage: OrderCustomerSearch <id>");
                        break;
                    }
                    await OrderHelpers.OrderByCustomerAsync(idLOrder);
                    break;
                case "status":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idSOrder))
                    {
                        Console.WriteLine("Usage: Status <id>");
                        break;
                    }
                    await OrderHelpers.ChangeOrderStatusAsync(idSOrder);
                    break;
                case "product":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idPOrder))
                    {
                        Console.WriteLine("Usage: Product <id>");
                        break;
                    }
                    await OrderHelpers.OrderByProductAsync(idPOrder);
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
    
    
    // Category Menu loop
    static async Task CategoryMenu()
    {
        while (true)
        {
            Console.WriteLine("\nCommands: Add | List | Edit <id> | Delete <id> | Search | Exit");
            Console.WriteLine(">");
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();
            switch (cmd)
            {
                case "add":
                    await CategoryHelpers.AddAsync();
                    break;
                case "list":
                    await CategoryHelpers.ListAsync();
                    break;
                case "edit":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idCategory))
                    {
                        Console.WriteLine("Usage: Edit <id>");
                        break;
                    }
                    await CategoryHelpers.EditAsync(idCategory);
                    break;
                case "delete":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idDCategory))
                    {
                        Console.WriteLine("Usage: Delete <id>");
                        break;
                    }
                    await CategoryHelpers.DeleteAsync(idDCategory);
                    break;
                case "search":
                    await CategoryHelpers.SearchCategoryAsync();
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
    
    // Product menu loop
    static async Task ProductMenu()
    {
        while (true)
        {
            Console.WriteLine("\nCommands: Add | List | ListSold | Edit <id> | Delete <id> | Search | Exit");
            Console.WriteLine(">");
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();
            switch (cmd)
            {
                case "add":
                    await ProductHelpers.AddProductAsync();
                    break;
                case "list":
                    await ProductHelpers.ListProductsAsync();
                    break;
                case "listsold":
                    await ProductHelpers.ListProductSalesAsync();
                    break;
                case "edit":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idProduct))
                    {
                        Console.WriteLine("Usage: Edit <id>");
                        break;
                    }
                    await ProductHelpers.EditProductAsync(idProduct);
                    break;
                case "delete":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idDProduct))
                    {
                        Console.WriteLine("Usage: Delete <id>");
                        break;
                    }
                    await ProductHelpers.DeleteProductAsync(idDProduct);
                    break;
                case "search":
                    await ProductHelpers.SearchProductAsync();
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    } 
}