using Interfaces.DbContexts;
using Interfaces.Services.Customers;
using Mapster;
using Microsoft.Extensions.Logging;

namespace UseCases.Customers.Service;

/// <summary>
/// Сервис для работы клиентами(покупателями)
/// </summary>
internal class CustomersService(IAppDbContext dbContext, ILogger<CustomersService> logger) : ICustomersService
{

    /// <summary>
    /// Создание клиента
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<CustomerDto>> CustomerCreate(CommandCustomerCreate command)
    {
        var validator = new CommandCustomerCreate.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<CustomerDto>(result);
        }
        var CustomerExists = await dbContext.Customers.AnyAsync(x => x.PhoneNumber == command.PhoneNumber);
        if (CustomerExists)
        {
            string message = $"[{command.GetType().Name}] Customer with phoneNumber: {command.PhoneNumber} already exists!";
            logger.LogWarning(message);
            return Result.Conflict<CustomerDto>(message);
        }

        var customer = Customer.Create(command.PhoneNumber, command.UserName);

        dbContext.Add(customer);
        await dbContext.SaveChangesAsync();

        return customer.Adapt<CustomerDto>().Ok();
    }

    /// <summary>
    /// Получение клиента по номеру телефона
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<CustomerDto>> CustomerGet(QueryCustomerGet query)
    {
        var validator = new QueryCustomerGet.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{query.GetType().Name}] Invalid  {query}");
            return Result.InputValidationErrors<CustomerDto>(result);
        }

        var Customer = await dbContext
            .Set<Customer>()
            .AsNoTracking()
            .Where(x => x.PhoneNumber == query.PhoneNumber)
            .FirstOrDefaultAsync();

        if(Customer == null)
        {
            string message = $"[{query.GetType().Name}] Customer with phoneNumber: {query.PhoneNumber} not found!";
            logger.LogWarning(message);
            return Result.NotFound<CustomerDto>(message);
        }
        return Customer.Adapt<CustomerDto>().Ok(); 
    }
}
