using Domain.Catalog;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Catalog;
using UseCases.Catalog.Service;
using UseCases.Customers;

namespace Tests.Catalog;

public class QueryGetProductsTests
{
    private IServiceProvider serviceProvider { get; set; }
    private ICatalogService catalogService { get; set; }

    private ProductType ProductType { get; set; }

    [SetUp]
    public async Task Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase(Guid.NewGuid().ToString());
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();
        catalogService = serviceProvider.GetRequiredService<ICatalogService>();

        var command = new CommandProductTypeCreate("Трансмиссионные масла");
        var Result = await catalogService.ProductTypeCreate(command);
        ProductType = Result.Value;
    }


    [Test]
    public async Task GetProductsOk()
    {
        var request = new QueryGetProducts(ProductType.ProductTypeId);
        var Result = await catalogService.GetProducts(request);

        Result.Success.Should().BeTrue();
    }


}
