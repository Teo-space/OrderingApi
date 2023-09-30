using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderingService orderingService;
    public OrdersController(IOrderingService orderingService)
    {
        this.orderingService = orderingService;

    }

    /// <summary>
    /// Запрос получения списка заказов по конкретному клиенту за выбранный временной период, отсортированный по дате создания.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [OutputCache(Duration = 30)]
    public async Task<Result<IReadOnlyCollection<Order>>> GetCustomerOrders([FromQuery] QueryGetCustomerOrders query)
        => await orderingService.GetCustomerOrders(query);

    //CommandOrderCheckOut

    /// <summary>
    ///  Команда формирования заказа
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Result<Order>> OrderCheckOut([FromBody] CommandOrderCheckOut command)
        => await orderingService.OrderCheckOut(command);


}
