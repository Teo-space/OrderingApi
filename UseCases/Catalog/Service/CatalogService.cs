using FluentValidation;
using Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UseCases.Catalog.Service;


internal class CatalogService : ICatalogService
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<CatalogService> logger;
    public CatalogService(AppDbContext dbContext, ILogger<CatalogService> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }


    /// <summary>
    /// Получение списка товаров
    /// (Ulid ProductTypeId, bool Instock = true, bool OrberByDescending = false)
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<Product>>> GetProducts(QueryGetProducts query)
    {
        var validator = new QueryGetProducts.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{query.GetType().Name}] Invalid  {query}");
            return Result.InputValidationErrors<IReadOnlyCollection<Product>>(result);
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
        var Products = await QResults.ToListAsync();

        return Result.Ok(Products as IReadOnlyCollection<Product>);
    }


    /// <summary>
    /// Создание товара
    /// (Ulid ProductTypeId, string Name, double Price, double QuanityInStock)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<Product>> ProductCreate(CommandProductCreate command)
    {
        var validator = new CommandProductCreate.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<Product>(result);
        }
        var ProductType = await dbContext
            .Set<ProductType>()
            .Where(pt => pt.ProductTypeId == command.ProductTypeId)
            .FirstOrDefaultAsync();
        if (ProductType is null)
        {
            logger.LogWarning($"[{command.GetType().Name}] ParentNotFound (ProductType) : {command.ProductTypeId}");
            return Result.ParentNotFound<Product>($"ProductType : {command.ProductTypeId}");
        }
        var product = Product.Create(ProductType, command.Name, command.Price, command.QuanityInStock);
        dbContext.Add(product);
        await dbContext.SaveChangesAsync();
        return Result.Ok(product);
    }








    /// <summary>
    /// Получить всех типов товаров
    /// </summary>
    /// <returns></returns>
    public async Task<Result<IReadOnlyCollection<ProductType>>> GetProductTypes()
    {
        var ProductTypes = await dbContext.Set<ProductType>().AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        return Result.Ok(ProductTypes as IReadOnlyCollection<ProductType>);
    }


    /// <summary>
    /// Получение товара по имени
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public async Task<Result<ProductType>> GetProductTypeByName(QueryGetProductTypeByName query)
    {
        var validator = new QueryGetProductTypeByName.Validator();
        var result = validator.Validate(query);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{query.GetType().Name}] Invalid  {query}");
            return Result.InputValidationErrors<ProductType>(result);
        }
        var ProductType = await dbContext
            .Set<ProductType>()
            .AsNoTracking()
            .Where(x => x.Name == query.Name)
            .FirstOrDefaultAsync();
        if (ProductType is null)
        {
            logger.LogWarning($"[{query.GetType().Name}] ProductType NotFound {query.Name}");
            return Result.NotFound<ProductType>(query.Name);
        }
        return Result.Ok(ProductType);
    }


    /// <summary>
    /// Создание нового типа товаров
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Result<ProductType>> ProductTypeCreate(CommandProductTypeCreate command)
    {
        var validator = new CommandProductTypeCreate.Validator();
        var result = validator.Validate(command);
        if (!result.IsValid)
        {
            logger.LogWarning($"[{command.GetType().Name}] Invalid  {command}");
            return Result.InputValidationErrors<ProductType>(result);
        }
        if(await dbContext.Set<ProductType>().AsNoTracking().AnyAsync(x => x.Name == command.Name))
        {
            string message = $"[{command.GetType().Name}] ProductType with {nameof(command.Name)}: {command.Name} already exists!";
            logger.LogWarning(message);
            return Result.Conflict<ProductType>(message);
        }

        var productType = ProductType.Create(command.Name);
        dbContext.Add(productType);
        await dbContext.SaveChangesAsync();

        return Result.Ok(productType);
    }




}

