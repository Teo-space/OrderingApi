namespace Domain.Ordering;


/*
Таблица с заказами:
ИД заказа
ИД клиента
Дата создания заказа
*/
public class Order
{
    public IdType OrderId { get; private set; }

    public IdType CustomerId { get; private set; }
    [JsonIgnore]
    public Customer Customer { get; private set; }


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
