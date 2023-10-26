namespace Domain.Ordering;

/// <summary>
/// Спецификация (позиция) заказа
/// </summary>
public class OrderLine
{
    /// <summary>
    /// Идентификатор спецификации к заказу
    /// </summary>
    public IdType OrderLineId { get; private set; }
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public IdType OrderId { get; private set; }
    [JsonIgnore]
    public Order Order { get; private set; }
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public IdType ProductId { get; private set; }
    [JsonIgnore]
    public Product Product { get; private set; }
    /// <summary>
    /// количество товара
    /// </summary>
    public double Quanity { get; private set; }


    public static OrderLine Create(Order Order, Product Product, OrderCartItem orderCartItem)
    {
        var orderLine = new OrderLine();
        orderLine.OrderLineId = Ulid.NewUlid().ToGuid();
        orderLine.Order = Order;
        orderLine.OrderId = Order.OrderId;

        orderLine.Product = Product;
        orderLine.ProductId = Product.ProductId;

        if(orderCartItem.ProductId != Product.ProductId)
        {
            throw new InvalidOperationException($"Product({Product.ProductId}) does not match OrderCartItem({orderCartItem.ProductId})");
        }

        orderLine.Quanity = orderCartItem.Quanity;
        Product.WritingOff(orderLine.Quanity);

        //Order.OrderLines.Add(orderLine);
        return orderLine;
    }

}
