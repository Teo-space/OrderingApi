namespace Domain.Ordering;

public class OrderLine
{
    public IdType OrderLineId { get; private set; }

    public IdType OrderId { get; private set; }
    [JsonIgnore]
    public Order Order { get; private set; }

    public IdType ProductId { get; private set; }
    [JsonIgnore]
    public Product Product { get; private set; }

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
