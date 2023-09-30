namespace Domain.Catalog;

public class ProductType
{
    public IdType ProductTypeId { get; private set; }

    public string Name { get; private set; }


    [JsonIgnore]
    public List<Product> Products { get; private set; } = new List<Product>();


    public static ProductType Create(string Name)
    {
        var productType = new ProductType();
        productType.ProductTypeId = Ulid.NewUlid().ToGuid();
        productType.Name = Name;
        return productType;
    }

}

