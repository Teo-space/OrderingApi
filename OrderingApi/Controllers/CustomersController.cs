using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomersService customersService;
    public CustomersController(ICustomersService customersService) 
    {
        this.customersService = customersService;
    }


    /// <summary>
    /// Запрос получения клиента по номеру телефона.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [OutputCache(Duration = 15)]
    [HttpGet]
    public async Task<Result<Customer>> CustomerGet([FromQuery] QueryCustomerGet query)
        => await customersService.CustomerGet(query);


    /// <summary>
    /// Команда для создания клиента
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Result<Customer>> CustomerCreate([FromBody] CommandCustomerCreate command)
        => await customersService.CustomerCreate(command);






}
