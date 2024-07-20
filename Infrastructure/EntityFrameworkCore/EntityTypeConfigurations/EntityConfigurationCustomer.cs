namespace Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

public class EntityConfigurationCustomer : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.CustomerId);
        builder.Property(x => x.CustomerId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.PhoneNumber);

        builder.Property(x => x.PhoneNumber).HasMaxLength(20);//Unique???
        builder.Property(x => x.UserName).HasMaxLength(100);

        builder.HasMany(Customer => Customer.OrderCartItems)
            .WithOne(OrderCartItem => OrderCartItem.Customer)
            .HasPrincipalKey(Customer => Customer.CustomerId)
            .HasForeignKey(OrderCartItem => OrderCartItem.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(Customer => Customer.Orders)
            .WithOne(Order => Order.Customer)
            .HasPrincipalKey(Customer => Customer.CustomerId)
            .HasForeignKey(Order => Order.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

    }

}
