using Microsoft.Extensions.DependencyInjection;
using UseCases.Catalog.Service;
using UseCases.Customers.Service;
using UseCases.Customers;

namespace Tests.Catalog.ProductTypes;

public class CommandProductTypeCreateTests
{
    private IServiceProvider serviceProvider { get; set; }
    private ICatalogService catalogService { get; set; }

    [SetUp]
    public void Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase(Guid.NewGuid().ToString());
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();
        catalogService = serviceProvider.GetRequiredService<ICatalogService>();
    }


    [Test]
    public async Task ProductTypeCreateOk()
    {
        var command = new CommandProductTypeCreate("Трансмиссионные масла");
        var Result = await catalogService.ProductTypeCreate(command);

        Result.Success.Should().BeTrue();
        Console.WriteLine(Result.message);
    }

    [Test]
    public async Task ProductTypeCreateAlreadyExists()
    {
        var command = new CommandProductTypeCreate("Моторные масла");
        await catalogService.ProductTypeCreate(command);
        var ResultExists = await catalogService.ProductTypeCreate(command);

        ResultExists.Success.Should().BeFalse();
    }

    [Test]
    public async Task ProductTypeCreateNameTooLong()
    {
        var command = new CommandProductTypeCreate("Моторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные маслаМоторные масла");
        var ResultExists = await catalogService.ProductTypeCreate(command);

        ResultExists.Success.Should().BeFalse();
    }


}
