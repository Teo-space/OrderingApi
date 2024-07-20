using UseCases.Catalog;
using UseCases.Customers;

namespace Interfaces.Services.Catalog;

/// <summary>
/// Сервис для работы с товарами
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Получение списка товаров
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<Result<IReadOnlyCollection<ProductDto>>> GetProducts(QueryGetProducts query);

    /// <summary>
    /// Создание товара
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<ProductDto>> ProductCreate(CommandProductCreate command);

}
