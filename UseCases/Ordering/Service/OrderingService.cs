﻿using Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UseCases.Ordering.Service;

internal class OrderingService : IOrderingService
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<OrderingService> logger;
    public OrderingService(AppDbContext dbContext, ILogger<OrderingService> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }



    /// <summary>
    /// Метод получения списка заказов по конкретному клиенту за выбранный временной период, отсортированный по дате создания.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<Order>>> GetCustomerOrders(QueryGetCustomerOrders query)//a0e48a01-090d-3378-b939-93494fb4ab2a
    {
        var validator = new QueryGetCustomerOrders.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"{query.GetType().Name} Invalid  {query}");
            return Result.InputValidationErrors<IReadOnlyCollection<Order>>(query.GetType().Name, result);
        }
        logger.LogInformation(query.ToString());

        var Orders = (await dbContext
            .Set<Order>()
            .AsNoTracking()
            .Where(order => order.CustomerId == query.CustomerId)
            .Where(order => order.CreatedAt.Date >= query.StartDate.Date)
            .Where(order => order.CreatedAt.Date <= query.EndDate.Date)
            //.OrderBy(order => order.CreatedAt)
            .ToListAsync())
            .OrderBy(order => order.CreatedAt).ToList();//Сортировка результатов в памяти

        return Result.Ok(Orders as IReadOnlyCollection<Order>);
    }




    /// <summary>
    /// Метод формирования заказа с проверкой наличия требуемого количества товара на складе, 
    //а также уменьшение доступного количества товара на складе в БД в случае успешного создания заказа.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<Order>> OrderCheckOut(CommandOrderCheckOut command)
    {
        var validator = new CommandOrderCheckOut.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"{command.GetType().Name} Invalid  {command}");
            return Result.InputValidationErrors<Order>(command.GetType().Name, result);
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
                    logger.LogWarning($"[{command.GetType().Name}] Customer({command.CustomerId}) Not Found!");
                    return Result.NotFound<Order>($"[{command.GetType().Name}] Customer({command.CustomerId}) Not Found!");
                }
                if (!Customer.OrderCartItems.Any())
                {
                    logger.LogWarning($"OrderCart is Empty!");
                    return Result.InvalidOperation<Order>($"OrderCart is Empty!");
                }

                var ProductNotFound = Customer.OrderCartItems
                    .Where(item => item.Product == null)
                    .Select(item => item.ProductId.ToString());
                if (ProductNotFound.Any())
                {
                    logger.LogWarning($"Include OrderCartItems.Product or Product not exists!");
                    return Result.InvalidOperation<Order>($"Include OrderCartItems.Product or Product not exists!");
                }
                var NotEnoughtProduct = Customer.OrderCartItems
                    .Where(item => item.Product.QuanityInStock < item.Quanity)
                    .Select(item => item.Product.Name);
                if (NotEnoughtProduct.Any())
                {
                    logger.LogWarning($"Not Enought Products in Stock {string.Join(", ", NotEnoughtProduct)}");
                    return Result.InvalidOperation<Order>($"Not Enought Products in Stock {string.Join(", ", NotEnoughtProduct)}");
                }

                var order = Order.Create(Customer);
                dbContext.Set<Order>().Add(order);
                logger.LogInformation($"Add Order");

                foreach (var item in Customer.OrderCartItems)
                {
                    logger.LogInformation($"Add OrderLine");
                    var orderLine = OrderLine.Create(order, item.Product, item);
                    dbContext.Set<OrderLine>().Add(orderLine);
                    
                }

                dbContext.Set<OrderCartItem>().RemoveRange(Customer.OrderCartItems);

                dbContext.SaveChanges();
                transaction.Commit();
                return Result.Ok(order);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }






}