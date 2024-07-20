using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;

public class TestController : ApiBaseController
{
    private readonly ILogger<TestController> logger;

    public TestController(ILogger<TestController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// �������� ��������� ����������
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
    /// �������� ����
    /// </summary>
    /// <returns></returns>
    [HttpGet("ResponseCache")]
    //[ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    //[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
    [OutputCache(Duration = 8)]
    public ContentResult GetTime() => Content(DateTime.Now.ToString());


}