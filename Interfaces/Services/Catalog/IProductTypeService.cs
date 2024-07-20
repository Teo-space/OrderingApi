using UseCases.Customers;

namespace Interfaces.Services.Catalog;

public interface IProductTypeService
{

    /// <summary>
    /// Получить всех типов товаров
    /// </summary>
    /// <returns></returns>
    public Task<Result<IReadOnlyCollection<ProductTypeDto>>> GetProductTypes();


    /// <summary>
    /// Получение товара по имени
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public Task<Result<ProductTypeDto>> GetProductTypeByName(QueryGetProductTypeByName query);


    /// <summary>
    /// Создание нового типа товаров
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<Result<ProductTypeDto>> ProductTypeCreate(CommandProductTypeCreate command);





}
