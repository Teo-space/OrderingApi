namespace Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

public class EntityConfigurationProduct : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.ProductId);
        builder.Property(x => x.ProductId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.ProductTypeId);
        builder.Property(x => x.ProductTypeId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.ProductId);
        builder.Property(x => x.ProductId)
            // .HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;


        builder.Property(x => x.Name).HasMaxLength(100);//IsUnique???

        builder.Property(x => x.Price).IsConcurrencyToken();
        builder.Property(x => x.QuanityInStock).IsConcurrencyToken();


        builder.HasOne(Product => Product.ProductType)
            .WithMany(ProductType => ProductType.Products)
            .HasPrincipalKey(ProductType => ProductType.ProductTypeId)
            .HasForeignKey(Product => Product.ProductTypeId);

    }
}
