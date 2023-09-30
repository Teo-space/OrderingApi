namespace UseCases.Ordering.Service;


public interface IOrderingService
{
    public Task<Result<IReadOnlyCollection<Order>>> GetCustomerOrders(QueryGetCustomerOrders query);

    public Task<Result<Order>> OrderCheckOut(CommandOrderCheckOut command);

    
}
