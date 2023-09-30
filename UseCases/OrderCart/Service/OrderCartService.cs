using Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UseCases.OrderCart.Service;


internal class OrderCartService : IOrderCartService
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<OrderCartService> logger;

    public OrderCartService(AppDbContext dbContext, ILogger<OrderCartService> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }



    public async Task<Result<IReadOnlyCollection<OrderCartItem>>> OrderCartItemsGet(QueryOrderCartItemsGet query)
    {
        var validator = new QueryOrderCartItemsGet.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"{query.GetType().Name} Invalid  {query}");
            return Result.InputValidationErrors<IReadOnlyCollection<OrderCartItem>>(query.GetType().Name, result);
        }
        var Customer = await dbContext
            .Set<Customer>()
            .AsNoTracking()
            .Where(Customer => Customer.CustomerId == query.CustomerId)
            .Include(x => x.OrderCartItems)
            .FirstOrDefaultAsync();
        //Проверка существования пользователя
        if (Customer is null)
        {
            logger.LogWarning($"[{query.GetType().Name}] Customer({query.CustomerId}) Not Found!");
            return Result.NotFound<IReadOnlyCollection<OrderCartItem>>($"[{query.GetType().Name}] Customer({query.CustomerId}) Not Found!");
        }
        return Result.Ok(Customer.OrderCartItems as IReadOnlyCollection<OrderCartItem>);
    }


    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItem>> AddItem(CommandOrderCartItemAdd command)
    {
        var validator = new CommandOrderCartItemAdd.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"{command.GetType().Name} Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItem>(command.GetType().Name, result);
        }

        var Customer = await dbContext
            .Set<Customer>()
            .Where(Customer => Customer.CustomerId == command.CustomerId)
            .FirstOrDefaultAsync();
        //Проверка существования пользователя
        if (Customer is null)
        {
            logger.LogWarning($"[{command.GetType().Name}] Customer({command.CustomerId}) Not Found!");
            return Result.NotFound<OrderCartItem>($"[{command.GetType().Name}] Customer({command.CustomerId}) Not Found!");
        }

        var Product = await dbContext
            .Set<Product>()
            .Where(Product => Product.ProductId == command.ProductId)
            .FirstOrDefaultAsync();
        //Проверка существования товара
        if (Product is null)
        {
            logger.LogWarning($"[{command.GetType().Name}] Product({command.CustomerId}) Not Found!");
            return Result.NotFound<OrderCartItem>($"[{command.GetType().Name}] Product({command.CustomerId}) Not Found!");
        }
        var orderCartItemExists = await dbContext
            .Set<OrderCartItem>()
            .Where(x => x.ProductId == Product.ProductId)
            .FirstOrDefaultAsync();
        //Проверка что товар уже в корзине
        if (orderCartItemExists is not null)
        {
            return Result.Ok(orderCartItemExists);
        }

        var orderCartItem = OrderCartItem.Create(Customer, Product);
        dbContext.Add(orderCartItem);
        await dbContext.SaveChangesAsync();
        return Result.Ok(orderCartItem);

    }


    /// <summary>
    /// Удаление из корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItem>> RemoveItem(CommandOrderCartItemRemove command)
    {
        var validator = new CommandOrderCartItemRemove.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"{command.GetType().Name} Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItem>(command.GetType().Name, result);
        }

        var items = await dbContext.Set<OrderCartItem>()
            .Where(item => item.CustomerId == command.CustomerId)
            .Where(item => item.ProductId == command.ProductId)
            .ToListAsync();
        //Нет в корзине
        if (!items.Any())
        {
            logger.LogWarning($"[{command.GetType().Name}] OrderCartItem({command.ProductId}) Not Found!");
            return Result.NotFound<OrderCartItem>($"[{command.GetType().Name}] OrderCartItem({command.ProductId}) Not Found!");
        }

        dbContext.Set<OrderCartItem>().RemoveRange(items);
        await dbContext.SaveChangesAsync();
        return Result.Ok(items.First());
    }

    /// <summary>
    /// Очистка корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<OrderCartItem>>> CartClear(CommandOrderCartItemsClear command)
    {
        var validator = new CommandOrderCartItemsClear.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"{command.GetType().Name} Invalid  {command}");
            return Result.InputValidationErrors<IReadOnlyCollection<OrderCartItem>>(command.GetType().Name, result);
        }

        var items = await dbContext
            .Set<OrderCartItem>()
            .AsNoTracking()
            .Where(item => item.CustomerId == command.CustomerId)
            .ToListAsync();
        //Пусто
        if (!items.Any())
        {
            return Result.Ok(items as IReadOnlyCollection<OrderCartItem>);
        }

        dbContext.Set<OrderCartItem>().RemoveRange(items);
        await dbContext.SaveChangesAsync();
        return Result.Ok(items as IReadOnlyCollection<OrderCartItem>);
    }


    /// <summary>
    /// Инкремент количества товара в корзине
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItem>> IncrementItemQuanity(CommandOrderCartItemIncrement command)
    {
        var validator = new CommandOrderCartItemIncrement.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"{command.GetType().Name} Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItem>(command.GetType().Name, result);
        }

        var item = await dbContext.Set<OrderCartItem>()
            .Where(item => item.CustomerId == command.CustomerId)
            .Where(item => item.ProductId == command.ProductId)
            .FirstOrDefaultAsync();
        //Нет в корзине
        if (item is null)
        {
            logger.LogWarning($"[{command.GetType().Name}] OrderCartItem({command.ProductId}) Not Found!");
            return Result.NotFound<OrderCartItem>($"[{command.GetType().Name}] OrderCartItem({command.ProductId}) Not Found!");
        }
        //Можно хранить величину инкремента в товаре, подгружать его
        item.Increment(/*Product*/);

        await dbContext.SaveChangesAsync();
        return Result.Ok(item);
    }

    /// <summary>
    /// Декремент количества товара в корзине. Возможен до минимального количества (1)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<OrderCartItem>> DecrementItemQuanity(CommandOrderCartItemDecrement command)
    {
        var validator = new CommandOrderCartItemDecrement.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"{command.GetType().Name} Invalid  {command}");
            return Result.InputValidationErrors<OrderCartItem>(command.GetType().Name, result);
        }

        var item = await dbContext.Set<OrderCartItem>()
            .Where(item => item.CustomerId == command.CustomerId)
            .Where(item => item.ProductId == command.ProductId)
            .FirstOrDefaultAsync();
        //Нет в корзине
        if (item is null)
        {
            logger.LogWarning($"[{command.GetType().Name}] OrderCartItem({command.ProductId}) Not Found!");
            return Result.NotFound<OrderCartItem>($"[{command.GetType().Name}] OrderCartItem({command.ProductId}) Not Found!");
        }
        //Можно хранить величину инкремента в товаре, подгружать его
        item.Decrement(/*Product*/);

        await dbContext.SaveChangesAsync();
        return Result.Ok(item);
    }




}
