namespace Domain.Customers;

/*
 
Таблица с клиентами:
ИД клиента
ФИО клиента
Телефон клиента
 
*/
public class Customer
{
    public IdType CustomerId { get; private set; }

    public string PhoneNumber { get; private set; }

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