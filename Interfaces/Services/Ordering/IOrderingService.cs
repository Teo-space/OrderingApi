using UseCases.Ordering;

namespace Interfaces.Services.Ordering;

/// <summary>
/// Сервис заказов
/// </summary>
public interface IOrderingService
{

    /// <summary>
    /// Метод получения списка заказов по конкретному клиенту за выбранный временной период, отсортированный по дате создания.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<Result<IReadOnlyCollection<OrderDto>>> GetCustomerOrders(QueryGetCustomerOrders query);


    /// <summary>
    /// Метод формирования заказа с проверкой наличия требуемого количества товара на складе, 
    /// а также уменьшение доступного количества товара на складе в БД в случае успешного создания заказа.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<OrderDto>> OrderCheckOut(CommandOrderCheckOut command);


}
