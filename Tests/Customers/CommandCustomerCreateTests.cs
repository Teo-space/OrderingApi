using Microsoft.Extensions.DependencyInjection;
using UseCases.Customers;
using UseCases.Customers.Service;

namespace Tests.Customers;

public class CommandCustomerCreateTests
{
    private IServiceProvider serviceProvider {  get; set; }

    [SetUp]
    public void Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase();
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();
    }
    //using var Scope = serviceProvider.CreateScope();

    [Test]
    public async Task CustomerCreateOk()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var command = new CommandCustomerCreate("+79871234567", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeTrue();
    }

    [Test]
    public async Task CustomerCreatePhoneNumberTooLong()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var command = new CommandCustomerCreate("+798712345670", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeFalse();
    }

    [Test]
    public async Task CustomerCreatePhoneNumberIncorrect()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var command = new CommandCustomerCreate("+7987123456A", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeFalse();
    }

    [Test]
    public async Task CustomerCreateUserNameTooLong()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var command = new CommandCustomerCreate("+79871234567", "Василий Иванович Никитин ТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекст");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeFalse();
    }

    /*
    [Test]
    public async Task CustomerCreateExists()
    {
        ICustomersService customersService = serviceProvider.GetRequiredService<ICustomersService>();
        var command = new CommandCustomerCreate("+79871234567", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        var CustomerExistsResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeFalse();
    }
    */


}


