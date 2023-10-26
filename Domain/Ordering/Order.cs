namespace Domain.Ordering;


/// <summary>
/// Заказ
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public IdType OrderId { get; private set; }
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public IdType CustomerId { get; private set; }

    [JsonIgnore]
    public Customer Customer { get; private set; }

    /// <summary>
    /// дата создания заказа
    /// </summary>
    public DateTime CreatedAt { get; private set; }


    [JsonIgnore]
    public List<OrderLine> OrderLines { get; private set; } = new List<OrderLine>();


    public static Order Create(Customer Customer)
    {
        var order = new Order();
        order.OrderId = Ulid.NewUlid().ToGuid();
        order.Customer = Customer;
        order.CustomerId = Customer.CustomerId;
        order.CreatedAt = DateTime.Now;
        return order;
    }


}
