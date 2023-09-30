namespace Infrastructure.EntityFrameworkCore;
using Domain.Catalog;
using Domain.Customers;
using Domain.Ordering;
using Domain.OrderingCart;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<OrderCartItem> OrderCartItems { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }





}


