namespace Infrastructure.EntityFrameworkCore.EntityTypeConfigurations;

public class EntityConfigurationProductType : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.ToTable("ProductTypes");

        builder.HasKey(x => x.ProductTypeId);
        builder.Property(x => x.ProductTypeId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.ProductTypeId);
        builder.Property(x => x.ProductTypeId)
            //.HasConversion(x => x.ToGuid(), x => new Ulid(x))
            ;

        builder.HasIndex(x => x.Name).IsUnique();//IsUnique???
        builder.Property(x => x.Name).HasMaxLength(100);


    }
}
