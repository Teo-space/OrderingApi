using Domain.Catalog;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Catalog;
using UseCases.Catalog.Service;
using UseCases.Customers;

namespace Tests.Catalog;

public class QueryGetProductsTests
{
    private IServiceProvider serviceProvider { get; set; }
    private IProductService ProductService { get; set; }
    private IProductTypeService ProductTypeService { get; set; }

    private ProductTypeDto ProductType { get; set; }

    [SetUp]
    public async Task Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase(Guid.NewGuid().ToString());
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();
        ProductService = serviceProvider.GetRequiredService<IProductService>();
        ProductTypeService = serviceProvider.GetRequiredService<IProductTypeService>();

        var command = new CommandProductTypeCreate("Трансмиссионные масла");
        var Result = await ProductTypeService.ProductTypeCreate(command);
        ProductType = Result.Value;
    }


    [Test]
    public async Task GetProductsOk()
    {
        var request = new QueryGetProducts(ProductType.ProductTypeId);
        var Result = await ProductService.GetProducts(request);

        Result.Success.Should().BeTrue();
    }


}
