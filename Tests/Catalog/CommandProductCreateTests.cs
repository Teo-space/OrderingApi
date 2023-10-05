using Domain.Catalog;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Catalog.Service;
using UseCases.Customers;

namespace Tests.Catalog;

public class CommandProductCreateTests
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

    //CommandProductCreate(IdType ProductTypeId, string Name, double Price, double QuanityInStock)

    [Test]
    public async Task ProductCreatOk()
    {
        var request = new CommandProductCreate(ProductType.ProductTypeId, "MOBIL Super 2000 X1 10W-40", 2673, 14d);
        var Result = await catalogService.ProductCreate(request);

        Result.Success.Should().BeTrue();
    }

    [Test]
    public async Task ProductCreatProductTypeIdEmpty()
    {
        var request = new CommandProductCreate(Guid.Empty, "MOBIL Super 2000 X1 10W-40", 2673, 14d);
        var Result = await catalogService.ProductCreate(request);

        Result.Success.Should().BeFalse();
    }

    [Test]
    public async Task ProductCreatNameTooLong()
    {
        var request = new CommandProductCreate(ProductType.ProductTypeId, "MOBIL Super 2000 X1 10W-40 TextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextText", 2673, 14d);
        var Result = await catalogService.ProductCreate(request);

        Result.Success.Should().BeFalse();
    }

    [Test]
    public async Task ProductCreatNamePriceGreaterZero()
    {
        var request = new CommandProductCreate(ProductType.ProductTypeId, "MOBIL Super 2000 X1 10W-40", -1, 14d);
        var Result = await catalogService.ProductCreate(request);

        Result.Success.Should().BeFalse();
    }

    [Test]
    public async Task ProductCreatQuanityInStockGreaterZero()
    {
        var request = new CommandProductCreate(ProductType.ProductTypeId, "MOBIL Super 2000 X1 10W-40", 2673, -1);
        var Result = await catalogService.ProductCreate(request);

        Result.Success.Should().BeFalse();
    }




}
