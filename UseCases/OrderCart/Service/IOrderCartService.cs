namespace UseCases.OrderCart.Service;

public interface IOrderCartService
{
    /// <summary>
    /// Получение списка товаров в корзине
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<Result<IReadOnlyCollection<OrderCartItem>>> OrderCartItemsGet(QueryOrderCartItemsGet query);

    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItem>> AddItem(CommandOrderCartItemAdd command);

    /// <summary>
    /// Удаление из корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItem>> RemoveItem(CommandOrderCartItemRemove command);

    /// <summary>
    /// Очистка корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<IReadOnlyCollection<OrderCartItem>>> CartClear(CommandOrderCartItemsClear command);

    /// <summary>
    /// Инкремент количества товара в корзине
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItem>> IncrementItemQuanity(CommandOrderCartItemIncrement command);

    /// <summary>
    /// Декремент количества товара в корзине. Возможен до минимального количества (1)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItem>> DecrementItemQuanity(CommandOrderCartItemDecrement command);
}
