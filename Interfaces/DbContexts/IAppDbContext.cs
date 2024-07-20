using Domain.Catalog;
using Domain.Customers;
using Domain.Ordering;
using Domain.OrderingCart;
using Microsoft.EntityFrameworkCore;

namespace Interfaces.DbContexts;

public interface IAppDbContext : IBaseDbContext
{
    public DbSet<Customer> Customers { get; }
    public DbSet<OrderCartItem> OrderCartItems { get; }

    public DbSet<Order> Orders { get; }
    public DbSet<OrderLine> OrderLines { get; }

    public DbSet<Product> Products { get; }
    public DbSet<ProductType> ProductTypes { get; }

}
