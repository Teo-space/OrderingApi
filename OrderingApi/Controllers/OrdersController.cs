using Interfaces.Services.Ordering;
using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;

public class OrdersController(IOrderingService orderingService) : ApiBaseController
{

    /// <summary>
    /// Запрос получения списка заказов по конкретному клиенту за выбранный временной период, отсортированный по дате создания.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [OutputCache(Duration = 30)]
    public async Task<Result<IReadOnlyCollection<OrderDto>>> GetCustomerOrders([FromQuery] QueryGetCustomerOrders query)
        => await orderingService.GetCustomerOrders(query);

    //CommandOrderCheckOut

    /// <summary>
    ///  Команда формирования заказа
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Result<OrderDto>> OrderCheckOut([FromBody] CommandOrderCheckOut command)
        => await orderingService.OrderCheckOut(command);


}
