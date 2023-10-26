namespace Domain.Customers;

/// <summary>
/// Клиент
/// </summary>
public class Customer
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public IdType CustomerId { get; private set; }
    /// <summary>
    /// Телефонный номер
    /// </summary>
    public string PhoneNumber { get; private set; }
    /// <summary>
    /// Ф.и.о
    /// </summary>
    public string UserName { get; private set; }


    [JsonIgnore]
    public List<OrderCartItem> OrderCartItems { get; private set; } = new List<OrderCartItem>();
    [JsonIgnore]
    public List<Order> Orders { get; private set; } = new List<Order>();



    public static Customer Create(string PhoneNumber, string UserName)
    {
        var Customer = new Customer();
        Customer.CustomerId = Ulid.NewUlid().ToGuid();
        Customer.PhoneNumber = PhoneNumber;
        Customer.UserName = UserName;
        return Customer;
    }


}