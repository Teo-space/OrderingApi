using UseCases.Customers;

namespace Interfaces.Services.Customers;

/// <summary>
/// Сервис для работы клиентами(покупателями)
/// </summary>
public interface ICustomersService
{
    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<CustomerDto>> CustomerCreate(CommandCustomerCreate command);


    /// <summary>
    /// Получение клиента по номеру телефона
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<Result<CustomerDto>> CustomerGet(QueryCustomerGet query);


}
