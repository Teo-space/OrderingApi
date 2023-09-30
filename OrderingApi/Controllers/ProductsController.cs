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
public class ProductsController : ControllerBase
{
    readonly ICatalogService catalogService;
    public ProductsController(ICatalogService catalogService)
    {
        this.catalogService = catalogService;
    }

    /// <summary>
    /// Запрос получения списка товаров,
    /// с возможностью фильтрации
    /// по типу товара 
    /// и/или по наличию на складе 
    /// и сортировки по цене (возрастанию и убыванию).
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [OutputCache(Duration = 30)]
    public async Task<Result<IReadOnlyCollection<Product>>> GetProducts([FromQuery] QueryGetProducts query) 
        => await catalogService.GetProducts(query);

    /// <summary>
    /// Запрос на получение типов продуктов
    /// </summary>
    /// <returns></returns>
    [HttpGet("Types/")]
    [OutputCache(Duration = 30)]
    public async Task<Result<IReadOnlyCollection<ProductType>>> GetProductTypes()
        => await catalogService.GetProductTypes();

    /// <summary>
    /// Запрос на получение типа продукта по имени
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("TypeByName/")]
    [OutputCache(Duration = 30)]
    public async Task<Result<ProductType>> GetProductTypeByName([FromQuery] QueryGetProductTypeByName query)
        => await catalogService.GetProductTypeByName(query);





}
