using Microsoft.EntityFrameworkCore;

namespace UseCases.Catalog.Service;

public  interface ICatalogService
{
    public Task<Result<IReadOnlyCollection<Product>>> GetProducts(QueryGetProducts query);
    public Task<Result<Product>> ProductCreate(CommandProductCreate command);


    public Task<Result<IReadOnlyCollection<ProductType>>> GetProductTypes();
    public Task<Result<ProductType>> GetProductTypeByName(QueryGetProductTypeByName query);
    public Task<Result<ProductType>> ProductTypeCreate(CommandProductTypeCreate command);


}
