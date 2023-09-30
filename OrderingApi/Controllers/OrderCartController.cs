using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;

/// <summary>
/// Работа с корзиной покупок
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrderCartController : ControllerBase
{
    private readonly IOrderCartService orderCartService;
    public OrderCartController(IOrderCartService orderCartService)
    {
        this.orderCartService = orderCartService;
    }


    /// <summary>
    /// Получение списка товаров в корзине
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [OutputCache(Duration = 15)]
    [HttpGet]
    public async Task<Result<IReadOnlyCollection<OrderCartItem>>> OrderCartItemsGet([FromQuery] QueryOrderCartItemsGet query)
        => await orderCartService.OrderCartItemsGet(query);

    
    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Result<OrderCartItem>> OrderCartItemAdd([FromBody] CommandOrderCartItemAdd command)
        => await orderCartService.AddItem(command);
    
    /// <summary>
    /// Удаление товара из корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<Result<OrderCartItem>> OrderCartItemRemove([FromBody] CommandOrderCartItemRemove command)
        => await orderCartService.RemoveItem(command);


    /// <summary>
    /// Очистка корзины
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpDelete("Clear/")]
    public async Task<Result<IReadOnlyCollection<OrderCartItem>>> OrderCartItemsClear([FromBody] CommandOrderCartItemsClear command)
        => await orderCartService.CartClear(command);


    /// <summary>
    /// Инкремент количества товара в корзине
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("Increment/")]
    public async Task<Result<OrderCartItem>> OrderCartItemIncrement([FromBody] CommandOrderCartItemIncrement command)
        => await orderCartService.IncrementItemQuanity(command);


    /// <summary>
    /// Декремент количества товара в корзине. Возможен до минимального количества (1)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("Decrement")]
    public async Task<Result<OrderCartItem>> OrderCartItemDecrement([FromBody] CommandOrderCartItemDecrement command)
        => await orderCartService.DecrementItemQuanity(command);


}
