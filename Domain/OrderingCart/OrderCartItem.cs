using System.Text.Json.Serialization;

namespace Domain.OrderingCart;

public class OrderCartItem
{
    public IdType OrderCartItemId { get; private set; }

    public IdType CustomerId { get; private set; }
    [JsonIgnore]
    public Customer Customer { get; private set; }

    public IdType ProductId { get; private set; }
    [JsonIgnore]
    public Product Product { get; private set; }


    public double Quanity { get; private set; }



    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    /// <param name="Customer"></param>
    /// <param name="Product"></param>
    /// <returns></returns>
    public static OrderCartItem Create(Customer Customer, Product Product)//Strong Typing
    {
        var OrderCartItem = new OrderCartItem();
        OrderCartItem.OrderCartItemId = Ulid.NewUlid().ToGuid();
        OrderCartItem.Customer = Customer;
        OrderCartItem.Product = Product;
        //Потом можно будет добавить в Product свойство Default Quanity и брать оттуда
        OrderCartItem.Quanity = 1;

        return OrderCartItem;
    }

    public void Increment()
    {
        Quanity += 1;
    }
    public void Decrement()
    {
        if (Quanity - 1 > 1)
        {
            Quanity -= 1;
        }
    }





}
