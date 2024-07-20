namespace Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

public class EntityConfigurationOrder : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.OrderId);
        builder.Property(x => x.OrderId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.CustomerId);
        builder.Property(x => x.CustomerId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasMany(Order => Order.OrderLines)
            .WithOne(OrderLine => OrderLine.Order)
            .HasPrincipalKey(Order => Order.OrderId)
            .HasForeignKey(OrderLine => OrderLine.OrderLineId)
            .OnDelete(DeleteBehavior.NoAction);

    }

}
