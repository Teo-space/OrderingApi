using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using UseCases.Catalog.Service;
using UseCases.Customers;

namespace Tests.Catalog.ProductTypes;

public class QueryGetProductTypeByNameTests
{
    private IServiceProvider serviceProvider { get; set; }
    private ICatalogService catalogService { get; set; }

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
    }

    [Test]
    public async Task GetProductTypeByNameOk()
    {
        var request = new QueryGetProductTypeByName("Трансмиссионные масла");
        var Result = await catalogService.GetProductTypeByName(request);

        Result.Success.Should().BeTrue();
    }

    [Test]
    public async Task GetProductTypeByNameNotFound()//"Моторные масла"
    {
        var request = new QueryGetProductTypeByName("Тест");
        var Result = await catalogService.GetProductTypeByName(request);


        Result.Success.Should().BeFalse();
    }


    [Test]
    public async Task GetProductTypeByNameNameTooLong()
    {
        var request = new QueryGetProductTypeByName("Моторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные масла");
        var ResultExists = await catalogService.GetProductTypeByName(request);

        ResultExists.Success.Should().BeFalse();
    }

}
