using Microsoft.Extensions.DependencyInjection;
using UseCases.Catalog.Service;
using UseCases.Customers;

namespace Tests.Catalog;

public class CommandProductCreateTests
{
    private IServiceProvider serviceProvider { get; set; }
    private ICatalogService catalogService { get; set; }

    [SetUp]
    public async Task Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase();
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();
        catalogService = serviceProvider.GetRequiredService<ICatalogService>();
        var command = new CommandProductTypeCreate("Трансмиссионные масла");
        var Result = await catalogService.ProductTypeCreate(command);
    }

    //CommandProductCreate(IdType ProductTypeId, string Name, double Price, double QuanityInStock)








}
