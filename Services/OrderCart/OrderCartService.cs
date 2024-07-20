using Interfaces.DbContexts;
using Interfaces.Services.OrderCart;
using Mapster;
using Microsoft.Extensions.Logging;

namespace UseCases.OrderCart.Service;


internal class OrderCartService(IAppDbContext dbContext, ILogger<OrderCartService> logger) : IOrderCartService
{

    /// <summary>
    /// Получение списка товаров в корзине пользователя
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<OrderCartItemDto>>> OrderCartItemsGet(QueryOrderCartItemsGet query)
    {
        var validator = new QueryOrderCartItemsGet.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{query.GetType().Name}] Invalid  {query}");
            return Result.InputValidationErrors<IReadOnlyCollection<OrderCartItemDto>>(result);
        }
        var Customer = await dbContext.Customers.AsNoTracking()
            .Where(Customer => Customer.CustomerId == query.CustomerId)
            .Include(x => x.OrderCartItems)
            .FirstOrDefaultAsync();
        //Проверка существования пользователя
        if (Customer is null)
        {
            logger.LogWarning($"[{query.GetType().Name}] Customer({query.CustomerId}) Not Found!");
            return Result.NotFound<IReadOnlyCollection<OrderCartItemDto>>($"[{query.GetType().Name}] Customer({query.CustomerId}) Not Found!");
        }

        return Customer.OrderCartItems.Adapt<IReadOnlyCollection<OrderCartItemDto>>().Ok();
    }


    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItemDto>> AddItem(CommandOrderCartItemAdd command)
    {
        var validator = new CommandOrderCartItemAdd.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItemDto>(result);
        }

        var Customer = await dbContext
            .Customers
            .Where(Customer => Customer.CustomerId == command.CustomerId)
            .FirstOrDefaultAsync();
        //Проверка существования пользователя
        if (Customer is null)
        {
            string message = $"[{command.GetType().Name}] Customer({command.CustomerId}) Not Found!";
            logger.LogWarning(message);
            return Result.NotFound<OrderCartItemDto>(message);
        }

        var Product = await dbContext
            .Products
            .Where(Product => Product.ProductId == command.ProductId)
            .FirstOrDefaultAsync();
        //Проверка существования товара
        if (Product is null)
        {
            string message = $"[{command.GetType().Name}] Product({command.ProductId}) Not Found!";
            logger.LogWarning(message);
            return Result.NotFound<OrderCartItemDto>(message);
        }
        var orderCartItemExists = await dbContext
            .OrderCartItems
            .Where(x => x.ProductId == Product.ProductId)
            .FirstOrDefaultAsync();
        //Проверка что товар уже в корзине
        if (orderCartItemExists is not null)
        {
            return orderCartItemExists.Adapt<OrderCartItemDto>().Ok();
        }

        var orderCartItemCreated = OrderCartItem.Create(Customer, Product);
        dbContext.Add(orderCartItemCreated);
        await dbContext.SaveChangesAsync();

        return orderCartItemCreated.Adapt<OrderCartItemDto>().Ok();
    }


    /// <summary>
    /// Удаление из корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItemDto>> RemoveItem(CommandOrderCartItemRemove command)
    {
        var validator = new CommandOrderCartItemRemove.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItemDto>(result);
        }

        var items = await dbContext.OrderCartItems
            .Where(item => item.CustomerId == command.CustomerId)
            .Where(item => item.ProductId == command.ProductId)
            .ToListAsync();
        //Нет в корзине
        if (!items.Any())
        {
            string message = $"[{command.GetType().Name}] OrderCartItem  (ProductId:{command.ProductId}) Not Found!";
            logger.LogWarning(message);
            return Result.NotFound<OrderCartItemDto>(message);
        }

        dbContext.Set<OrderCartItem>().RemoveRange(items);
        await dbContext.SaveChangesAsync();

        return items.First().Adapt<OrderCartItemDto>().Ok();
    }


    /// <summary>
    /// Очистка корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<OrderCartItemDto>>> CartClear(CommandOrderCartItemsClear command)
    {
        var validator = new CommandOrderCartItemsClear.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<IReadOnlyCollection<OrderCartItemDto>>(result);
        }

        var items = await dbContext.OrderCartItems
            .Where(item => item.CustomerId == command.CustomerId)
            .ToListAsync();

        if (items.Any())
        {
            dbContext.OrderCartItems.RemoveRange(items);
            await dbContext.SaveChangesAsync();
        }

        return items.Adapt<IReadOnlyCollection<OrderCartItemDto>>().Ok();
    }


    /// <summary>
    /// Инкремент количества товара в корзине
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItemDto>> IncrementItemQuanity(CommandOrderCartItemIncrement command)
    {
        var validator = new CommandOrderCartItemIncrement.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItemDto>(result);
        }

        var item = await dbContext.Set<OrderCartItem>()
            .Where(item => item.CustomerId == command.CustomerId)
            .Where(item => item.ProductId == command.ProductId)
            .FirstOrDefaultAsync();
        //Нет в корзине
        if (item is null)
        {
            string message = $"[{command.GetType().Name}] OrderCartItem(ProductId:{command.ProductId}) Not Found!";
            logger.LogWarning(message);
            return Result.NotFound<OrderCartItemDto>(message);
        }
        item.Increment();
        await dbContext.SaveChangesAsync();

        return item.Adapt<OrderCartItemDto>().Ok();
    }

    /// <summary>
    /// Декремент количества товара в корзине. Возможен до минимального количества (1)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItemDto>> DecrementItemQuanity(CommandOrderCartItemDecrement command)
    {
        var validator = new CommandOrderCartItemDecrement.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItemDto>(result);
        }

        var item = await dbContext.Set<OrderCartItem>()
            .Where(item => item.CustomerId == command.CustomerId)
            .Where(item => item.ProductId == command.ProductId)
            .FirstOrDefaultAsync();
        //Нет в корзине
        if (item is null)
        {
            string message = $"[{command.GetType().Name}] OrderCartItem(:ProductId{command.ProductId}) Not Found!";
            logger.LogWarning(message);
            return Result.NotFound<OrderCartItemDto>(message);
        }
        //Можно хранить величину инкремента в товаре, подгружать его
        item.Decrement(/*Product*/);
        await dbContext.SaveChangesAsync();

        return item.Adapt<OrderCartItemDto>().Ok();
    }




}
