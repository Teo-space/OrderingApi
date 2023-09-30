using Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UseCases.Customers.Service;

internal class CustomersService : ICustomersService
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<CustomersService> logger;
    public CustomersService(AppDbContext dbContext, ILogger<CustomersService> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<Customer>> CustomerCreate(CommandCustomerCreate command)
    {
        var validator = new CommandCustomerCreate.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"{command.GetType().Name} Invalid  {command}");
            return Result.InputValidationErrors<Customer>(command.GetType().Name, result);
        }
        var CustomerExists = await dbContext
            .Set<Customer>()
            .Where(x => x.PhoneNumber == command.PhoneNumber)
            .FirstOrDefaultAsync();
        if (CustomerExists is not null)
        {
            logger.LogWarning($"Customer with phoneNumber: {command.PhoneNumber} already exists!");
            return Result.Conflict<Customer>($"Customer with phoneNumber: {command.PhoneNumber} already exists!");
        }

        var customer = Customer.Create(command.PhoneNumber, command.UserName);
        await dbContext.AddAsync(customer);
        await dbContext.SaveChangesAsync();
        return Result.Ok(customer);
    }


    /// <summary>
    /// Получение пользователя по номеру телефона
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<Customer>> CustomerGet(QueryCustomerGet query)
    {
        var validator = new QueryCustomerGet.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"{query.GetType().Name} Invalid  {query}");
            return Result.InputValidationErrors<Customer>(query.GetType().Name, result);
        }

        var Customer = await dbContext
            .Set<Customer>().AsNoTracking()
            .Where(x => x.PhoneNumber == query.PhoneNumber)
            .FirstOrDefaultAsync();

        if(Customer == null)
        {
            logger.LogWarning($"Customer with phoneNumber: {query.PhoneNumber} not found!");
            return Result.NotFound<Customer>($"Customer with phoneNumber: {query.PhoneNumber} not found!");
        }
        return Result.Ok(Customer);
    }







}
