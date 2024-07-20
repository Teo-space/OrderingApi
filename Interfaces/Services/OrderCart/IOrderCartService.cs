using UseCases.OrderCart;

namespace Interfaces.Services.OrderCart;

public interface IOrderCartService
{
    /// <summary>
    /// Получение списка товаров в корзине
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<Result<IReadOnlyCollection<OrderCartItemDto>>> OrderCartItemsGet(QueryOrderCartItemsGet query);

    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItemDto>> AddItem(CommandOrderCartItemAdd command);

    /// <summary>
    /// Удаление из корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItemDto>> RemoveItem(CommandOrderCartItemRemove command);

    /// <summary>
    /// Очистка корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<IReadOnlyCollection<OrderCartItemDto>>> CartClear(CommandOrderCartItemsClear command);

    /// <summary>
    /// Инкремент количества товара в корзине
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItemDto>> IncrementItemQuanity(CommandOrderCartItemIncrement command);

    /// <summary>
    /// Декремент количества товара в корзине. Возможен до минимального количества (1)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderCartItemDto>> DecrementItemQuanity(CommandOrderCartItemDecrement command);

}
