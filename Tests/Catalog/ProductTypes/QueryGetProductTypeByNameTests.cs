using Interfaces.Services.Catalog;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Customers;

namespace Tests.Catalog.ProductTypes;

public class QueryGetProductTypeByNameTests
{
    private IServiceProvider serviceProvider { get; set; }
    private IProductTypeService ProductTypeService { get; set; }

    [SetUp]
    public async Task Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase(Guid.NewGuid().ToString());
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();
        ProductTypeService = serviceProvider.GetRequiredService<IProductTypeService>();

        var command = new CommandProductTypeCreate("Трансмиссионные масла");
        var Result = await ProductTypeService.ProductTypeCreate(command);
    }

    [Test]
    public async Task GetProductTypeByNameOk()
    {
        var request = new QueryGetProductTypeByName("Трансмиссионные масла");
        var Result = await ProductTypeService.GetProductTypeByName(request);

        Result.Success.Should().BeTrue();
    }

    [Test]
    public async Task GetProductTypeByNameNotFound()//"Моторные масла"
    {
        var request = new QueryGetProductTypeByName("Тест");
        var Result = await ProductTypeService.GetProductTypeByName(request);


        Result.Success.Should().BeFalse();
    }


    [Test]
    public async Task GetProductTypeByNameNameTooLong()
    {
        var request = new QueryGetProductTypeByName("Моторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные масла");
        var ResultExists = await ProductTypeService.GetProductTypeByName(request);

        ResultExists.Success.Should().BeFalse();
    }

}
