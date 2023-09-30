namespace Domain.Catalog;


/*
Таблица с товарами:
ИД товара
ИД типа товара (список всех типов выделить в отдельную таблицу, поле с foreign key)
Наименование товара
Цена товара
Доступное количество товара на складе
*/
public class Product
{
    public IdType ProductId { get; private set; }

    public IdType ProductTypeId { get; private set; }
    [JsonIgnore]
    public ProductType ProductType { get; private set; }
    
    public string Name { get; private set; }

    public double Price { get; private set; }

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
