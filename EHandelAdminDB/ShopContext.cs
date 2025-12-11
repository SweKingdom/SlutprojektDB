using EHandelAdminDB.Models;
using Microsoft.EntityFrameworkCore;
namespace EHandelAdminDB;

public class ShopContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderRow> OrderRows => Set<OrderRow>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OrderSummary> OrderSummery => Set<OrderSummary>();
    public DbSet<CustomerOrderCountView> CustomerOrderCountViews => Set<CustomerOrderCountView>();
    public DbSet<ProductSalesView> ProductSalesView { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "shop.db");

        optionsBuilder.UseSqlite($"Filename={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            // PK
            e.HasKey(x => x.CategoryId);
            // Properties
            e.Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(100);
            e.Property(x => x.CategoryDescription)
                .HasMaxLength(250);
            e.HasIndex(x => x.CategoryName)
                .IsUnique();
            
        });

        modelBuilder.Entity<Customer>(e =>
        {
            // PK
            e.HasKey(x => x.CustomerId);
            // Properties
            e.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250);
            e.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(250);
            e.HasIndex(x => x.Email)
                .IsUnique();

        });

        modelBuilder.Entity<CustomerOrderCountView>(o =>
        {
            o.HasNoKey();
            o.ToView("CustomerOrderCountView");
        });


        modelBuilder.Entity<Order>(e =>
        {
            // PK
            e.HasKey(x => x.OrderId);
            // Properties
            e.Property(x => x.OrderDate);
            e.Property(x => x.TotalAmount);
            e.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderRow>(e =>
        {
            // PK
            e.HasKey(x => x.OrderRowId);
            // Prop
            e.Property(x => x.Quantity)
                .IsRequired();
            e.Property(x => x.UnitPrice)
                .IsRequired();
            
            // FK
            e.HasOne(x => x.Order)
                .WithMany(x => x.OrderRows)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<OrderSummary>(o =>
            {
                o.HasNoKey(); // Saknar PK alltså har ingen primärnyckel
                o.ToView("OrderSummaryView"); // Koppla tabellen mot SQlite
            }
        );

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(x => x.ProductId);
            e.Property(x => x.Pris)
                .IsRequired();
            e.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(150);
            e.HasIndex(x => x.ProductName)
                .IsUnique();
            e.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<ProductSalesView>(o =>
        {
            o.HasNoKey();
            o.ToView("ProductSalesView");
        });
    }
}