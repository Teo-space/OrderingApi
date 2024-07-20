using Interfaces.Services.Catalog;
using Microsoft.AspNetCore.OutputCaching;
using UseCases.Catalog;

namespace OrderingApi.Controllers;

/// <summary>
/// Контроллер для работы с каталогом Товаров
/// </summary>
public class ProductsController(IProductService productService) : ApiBaseController
{

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
    public async Task<Result<IReadOnlyCollection<ProductDto>>> GetProducts([FromQuery] QueryGetProducts query) 
        => await productService.GetProducts(query);



}
