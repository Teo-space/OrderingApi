using Interfaces.DbContexts;
using Interfaces.Services.Catalog;
using Mapster;
using Microsoft.Extensions.Logging;

namespace UseCases.Catalog.Service;

internal class ProductTypeService(IAppDbContext dbContext, ILogger<ProductTypeService> logger) : IProductTypeService
{

    /// <summary>
    /// Получить всех типов товаров
    /// </summary>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<ProductTypeDto>>> GetProductTypes()
    {
        var ProductTypes = await dbContext.ProductTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectToType<ProductTypeDto>()
            .ToListAsync();

        return ProductTypes.Ok();
    }

    /// <summary>
    /// Получение товара по имени
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public async Task<Result<ProductTypeDto>> GetProductTypeByName(QueryGetProductTypeByName query)
    {
        var validator = new QueryGetProductTypeByName.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{query.GetType().Name}] Invalid  {query}");
            return Result.InputValidationErrors<ProductTypeDto>(result);
        }
        var ProductType = await dbContext.ProductTypes.AsNoTracking()
            .Where(x => x.Name == query.Name)
            .ProjectToType<ProductTypeDto>()
            .FirstOrDefaultAsync();
        if (ProductType is null)
        {
            logger.LogWarning($"[{query.GetType().Name}] ProductType NotFound {query.Name}");
            return Result.NotFound<ProductTypeDto>(query.Name);
        }

        return Result.Ok(ProductType);
    }

    /// <summary>
    /// Создание нового типа товаров
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<ProductTypeDto>> ProductTypeCreate(CommandProductTypeCreate command)
    {
        var validator = new CommandProductTypeCreate.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<ProductTypeDto>(result);
        }
        var ProductTypeExists = await dbContext.Set<ProductType>().AsNoTracking().AnyAsync(x => x.Name == command.Name);
        if (ProductTypeExists)
        {
            string message = $"[{command.GetType().Name}] ProductType with {nameof(command.Name)}: {command.Name} already exists!";
            logger.LogWarning(message);
            return Result.Conflict<ProductTypeDto>(message);
        }

        var productType = ProductType.Create(command.Name);
        dbContext.Add(productType);
        await dbContext.SaveChangesAsync();

        return productType.Adapt<ProductTypeDto>().Ok();
    }

}
