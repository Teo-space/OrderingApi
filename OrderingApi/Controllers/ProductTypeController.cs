using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using UseCases.Catalog;
using UseCases.Catalog.Service;

namespace OrderingApi.Controllers;

/// <summary>
/// Контроллер для работы с каталогом Товаров
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProductTypeController(IProductTypeService productService) : ControllerBase
{

    /// <summary>
    /// Запрос на получение типов продуктов
    /// </summary>
    /// <returns></returns>
    [HttpGet("Types/")]
    [OutputCache(Duration = 30)]
    public async Task<Result<IReadOnlyCollection<ProductTypeDto>>> GetProductTypes()
        => await productService.GetProductTypes();

    /// <summary>
    /// Запрос на получение типа продукта по имени
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("TypeByName/")]
    [OutputCache(Duration = 30)]
    public async Task<Result<ProductTypeDto>> GetProductTypeByName([FromQuery] QueryGetProductTypeByName query)
        => await productService.GetProductTypeByName(query);



}
