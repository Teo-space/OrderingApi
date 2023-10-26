namespace Domain.Catalog;

/// <summary>
/// Тип товара
/// </summary>
public class ProductType
{
    /// <summary>
    /// Идентификатор типа товара
    /// </summary>
    public IdType ProductTypeId { get; private set; }
    /// <summary>
    /// наименование типа товара
    /// </summary>
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

