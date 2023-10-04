using Microsoft.Extensions.DependencyInjection;
using UseCases.Customers.Service;
using UseCases.Customers;

namespace Tests.Customers;

public class QueryCustomerGetTests
{

    private IServiceProvider serviceProvider { get; set; }

    [SetUp]
    public async Task Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase();
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();

        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var command = new CommandCustomerCreate("+79871234567", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);
    }

    [Test]
    public async Task CustomerGetOk()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var query = new QueryCustomerGet("+79871234567");
        var Result = await customersService.CustomerGet(query);

        Result.Success.Should().BeTrue();
    }


    [Test]
    public async Task CustomerGetPhoneNumberTooLong()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var query = new QueryCustomerGet("+798712345670");
        var Result = await customersService.CustomerGet(query);

        Result.Success.Should().BeFalse();
    }


    [Test]
    public async Task CustomerCreatePhoneNumberIncorrect()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var query = new QueryCustomerGet("+7987123456A");
        var Result = await customersService.CustomerGet(query);

        Result.Success.Should().BeFalse();

    }


    [Test]
    public async Task CustomerGetUserNotExists()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var query = new QueryCustomerGet("+79870000000");
        var Result = await customersService.CustomerGet(query);

        Result.Success.Should().BeFalse();
    }
}
