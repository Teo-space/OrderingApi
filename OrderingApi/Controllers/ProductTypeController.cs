using Interfaces.Services.Catalog;
using Microsoft.AspNetCore.OutputCaching;

namespace OrderingApi.Controllers;

/// <summary>
/// Контроллер для работы с каталогом Товаров
/// </summary>
public class ProductTypeController(IProductTypeService productService) : ApiBaseController
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
