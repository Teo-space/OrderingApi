namespace UseCases.Customers.Service;

public interface ICustomersService
{
    public Task<Result<Customer>> CustomerCreate(CommandCustomerCreate command);

    public Task<Result<Customer>> CustomerGet(QueryCustomerGet query);


}
