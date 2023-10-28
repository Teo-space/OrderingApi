using Infrastructure.EntityFrameworkCore;
using Mapster;
using Microsoft.Extensions.Logging;

namespace UseCases.Ordering.Service;

internal class OrderingService(AppDbContext dbContext, ILogger<OrderingService> logger) : IOrderingService
{

    /// <summary>
    /// Метод получения списка заказов по конкретному клиенту за выбранный временной период, отсортированный по дате создания.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<OrderDto>>> GetCustomerOrders(QueryGetCustomerOrders query)//a0e48a01-090d-3378-b939-93494fb4ab2a
    {
        var validator = new QueryGetCustomerOrders.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{query.GetType().Name}] Invalid  {query}");
            return Result.InputValidationErrors<IReadOnlyCollection<OrderDto>>(result);
        }
        logger.LogInformation(query.ToString());

        var Orders = await dbContext
            .Set<Order>()
            .AsNoTracking()
            .Where(order => order.CustomerId == query.CustomerId)
            .Where(order => order.CreatedAt.Date >= query.StartDate.Date)
            .Where(order => order.CreatedAt.Date <= query.EndDate.Date)
            .OrderBy(order => order.CreatedAt)
            .ToListAsync();

        return Orders.Adapt<IReadOnlyCollection<OrderDto>>().Ok();
    }




    /// <summary>
    /// Метод формирования заказа с проверкой наличия требуемого количества товара на складе, 
    /// а также уменьшение доступного количества товара на складе в БД в случае успешного создания заказа.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderDto>> OrderCheckOut(CommandOrderCheckOut command)
    {
        var validator = new CommandOrderCheckOut.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<OrderDto>(result);
        }

        using (var transaction = dbContext.Database.BeginTransaction())
        {
            try
            {
                var Customer = await dbContext
                    .Set<Customer>()
                    .Where(Customer => Customer.CustomerId == command.CustomerId)
                    .Include(c => c.OrderCartItems)
                    .ThenInclude(item => item.Product)
                    .FirstOrDefaultAsync();
                //Проверка существования пользователя
                if (Customer == null)
                {
                    string message = $"[{command.GetType().Name}] Customer({command.CustomerId}) Not Found!";
                    logger.LogWarning(message);
                    return Result.NotFound<OrderDto>(message);
                }
                if (!Customer.OrderCartItems.Any())
                {
                    string message = $"[{command.GetType().Name}] Customer({command.CustomerId}) OrderCart is Empty!";
                    logger.LogWarning(message);
                    return Result.InvalidOperation<OrderDto>(message);
                }

                var ProductNotFound = Customer.OrderCartItems
                    .Where(item => item.Product == null)
                    .Select(item => item.ProductId.ToString());
                if (ProductNotFound.Any())
                {
                    string message = $"[{command.GetType().Name}] Customer({command.CustomerId}) Include OrderCartItems.Product or Product not exists!";
                    logger.LogWarning(message);
                    return Result.InvalidOperation<OrderDto>(message);
                }
                var NotEnoughtProduct = Customer.OrderCartItems
                    .Where(item => item.Product.QuanityInStock < item.Quanity)
                    .Select(item => item.Product.Name);
                if (NotEnoughtProduct.Any())
                {
                    string message = $"[{command.GetType().Name}] Customer({command.CustomerId}) Not Enought Products in Stock {string.Join(", ", NotEnoughtProduct)}";
                    logger.LogWarning(message);
                    return Result.InvalidOperation<OrderDto>(message);
                }

                var order = Order.Create(Customer);
                dbContext.Set<Order>().Add(order);
                //logger.LogInformation($"Add Order");

                foreach (var item in Customer.OrderCartItems)
                {
                    //logger.LogInformation($"Add OrderLine");
                    var orderLine = OrderLine.Create(order, item.Product, item);
                    dbContext.Set<OrderLine>().Add(orderLine);
                }

                dbContext.Set<OrderCartItem>().RemoveRange(Customer.OrderCartItems);

                dbContext.SaveChanges();
                transaction.Commit();
                return order.Adapt<OrderDto>().Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }






}
