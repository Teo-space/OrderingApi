using Domain.Ordering;
using NUlid;

namespace Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

public class EntityConfigurationOrderLine : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable("OrderLines");

        builder.HasKey(x => x.OrderLineId);
        builder.Property(x => x.OrderLineId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.OrderId);
        builder.Property(x => x.OrderId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.ProductId);
        builder.Property(x => x.ProductId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;


        builder.Property(x => x.Quanity).IsConcurrencyToken();


        builder.HasOne(OrderLine => OrderLine.Product)
            .WithOne()
            .HasForeignKey<OrderLine>(x => x.ProductId);

    }
}
