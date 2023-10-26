namespace Domain.Catalog;

/// <summary>
/// Товар
/// </summary>
public class Product
{
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public IdType ProductId { get; private set; }
    /// <summary>
    /// Идентификатор типа товара
    /// </summary>
    public IdType ProductTypeId { get; private set; }
    [JsonIgnore]
    public ProductType ProductType { get; private set; }
    /// <summary>
    /// Наименование товара
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Цена за ед. товара
    /// </summary>
    public double Price { get; private set; }
    /// <summary>
    /// Количество на складе
    /// </summary>
    public double QuanityInStock { get; private set; }

    [JsonIgnore]
    public List<OrderCartItem> OrderCartItems { get; set; } = new List<OrderCartItem>();



    public static Product Create(ProductType ProductType, string Name, double Price, double QuanityInStock)
    {
        Product product = new Product();
        product.ProductId = Ulid.NewUlid().ToGuid();
        product.ProductType = ProductType;
        product.ProductTypeId = ProductType.ProductTypeId;

        product.Name = Name;
        product.Price = Price;
        product.QuanityInStock = QuanityInStock;

        return product;
    }


    public void Replenishment(double value) => QuanityInStock += value;

    public bool CanWriteOff(double value) => (QuanityInStock - value) > 0;
    public void WritingOff(double value)
    {
        if (CanWriteOff(value))
        {
            QuanityInStock -= value;
        }
        else
        {
            throw new InvalidOperationException($"[Product[Name({this.Name}), Id({this.ProductId})]] Not enough goods to write off! ({QuanityInStock} - {value})");
        }
    }




}
