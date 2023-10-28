using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CustomersController(ICustomersService customersService) : ControllerBase
{

    /// <summary>
    /// Запрос получения клиента по номеру телефона.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [OutputCache(Duration = 15)]
    [HttpGet]
    public async Task<Result<CustomerDto>> CustomerGet([FromQuery] QueryCustomerGet query)
        => await customersService.CustomerGet(query);


    /// <summary>
    /// Команда для создания клиента
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Result<CustomerDto>> CustomerCreate([FromBody] CommandCustomerCreate command)
        => await customersService.CustomerCreate(command);


}
