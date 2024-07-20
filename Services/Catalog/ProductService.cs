using Interfaces.DbContexts;
using Interfaces.Services.Catalog;
using Mapster;
using Microsoft.Extensions.Logging;

namespace UseCases.Catalog.Service;


internal class ProductService(IAppDbContext dbContext, ILogger<ProductService> logger) : IProductService
{

    /// <summary>
    /// Получение списка товаров
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<ProductDto>>> GetProducts(QueryGetProducts query)
    {
        var validator = new QueryGetProducts.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{query.GetType().Name}] Invalid  {query}");
            return Result.InputValidationErrors<IReadOnlyCollection<ProductDto>>(result);
        }

        var QResults = dbContext.Set<Product>().AsNoTracking();
        if(query.ProductTypeId != IdType.Empty)
        {
            QResults = QResults.Where(x => x.ProductTypeId == query.ProductTypeId);
        }
        if(query.Instock)
        {
            QResults = QResults.Where(x => x.QuanityInStock > 0);
        }
        if(query.OrderByDescending)
        {
            QResults = QResults.OrderByDescending(x => x.Price);
        }
        else
        {
            QResults = QResults.OrderBy(x => x.Price);
        }
        var Products = await QResults.ProjectToType<ProductDto>().ToListAsync();

        return Products.Ok();
    }


    /// <summary>
    /// Создание товара
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<ProductDto>> ProductCreate(CommandProductCreate command)
    {
        var validator = new CommandProductCreate.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<ProductDto>(result);
        }
        var ProductType = await dbContext.ProductTypes
            .Where(pt => pt.ProductTypeId == command.ProductTypeId)
            .FirstOrDefaultAsync();
        if (ProductType is null)
        {
            logger.LogWarning($"[{command.GetType().Name}] ParentNotFound (ProductType) : {command.ProductTypeId}");
            return Result.ParentNotFound<ProductDto>($"ProductType : {command.ProductTypeId}");
        }
        var product = Product.Create(ProductType, command.Name, command.Price, command.QuanityInStock);
        dbContext.Add(product);
        await dbContext.SaveChangesAsync();

        return product.Adapt<ProductDto>().Ok();

    }

}

