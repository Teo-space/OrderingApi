using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;


[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> logger;

    public TestController(ILogger<TestController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Проверка перехвата исключений
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet("ExceptionCatch")]
    public string ExceptionCatch()
    {
        logger.LogWarning("ExceptionCatch");
        throw new Exception("Catch Me!");
    }


    /// <summary>
    /// Проверка кеша
    /// </summary>
    /// <returns></returns>
    [HttpGet("ResponseCache")]
    //[ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    //[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
    [OutputCache(Duration = 8)]
    public ContentResult GetTime() => Content(DateTime.Now.ToString());


}