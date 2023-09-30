using Domain.OrderingCart;
using NUlid;

namespace Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

public class EntityConfigurationOrderCartItem : IEntityTypeConfiguration<OrderCartItem>
{
    public void Configure(EntityTypeBuilder<OrderCartItem> builder)
    {
        builder.ToTable("OrderCartItems");

        builder.HasKey(x => x.OrderCartItemId);
        builder.Property(x => x.OrderCartItemId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.CustomerId);
        builder.Property(x => x.CustomerId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.ProductId);
        builder.Property(x => x.ProductId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;


        builder.Property(x => x.Quanity).IsConcurrencyToken();


        builder.HasOne(OrderCartItem => OrderCartItem.Product)
            .WithMany(Product => Product.OrderCartItems)
            .HasPrincipalKey(Product => Product.ProductId)
            .HasForeignKey(OrderCartItem => OrderCartItem.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

    }



}

