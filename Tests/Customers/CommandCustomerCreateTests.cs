using Microsoft.Extensions.DependencyInjection;
using UseCases.Customers;
using UseCases.Customers.Service;

namespace Tests.Customers;

public class CommandCustomerCreateTests
{
    private IServiceProvider serviceProvider {  get; set; }
    private ICustomersService customersService {  get; set; }

    [SetUp]
    public void Setup()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddInfrastructureUseInMemoryDatabase();
        services.AddUseCases();
        serviceProvider = services.BuildServiceProvider();
        customersService = serviceProvider.GetRequiredService<ICustomersService>();
    }
    //using var Scope = serviceProvider.CreateScope();

    [Test]
    public async Task CustomerCreateOk()
    {
        var command = new CommandCustomerCreate("+79871234567", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeTrue();
    }

    [Test]
    public async Task CustomerCreatePhoneNumberTooLong()
    {
        var command = new CommandCustomerCreate("+798712345670", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeFalse();
    }

    [Test]
    public async Task CustomerCreatePhoneNumberIncorrect()
    {
        var command = new CommandCustomerCreate("+7987123456A", "Василий Иванович Никитин");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeFalse();
    }

    [Test]
    public async Task CustomerCreateUserNameTooLong()
    {
        var command = new CommandCustomerCreate("+79871234567", "Василий Иванович Никитин ТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекстТекст");
        var CustomerCreatedResult = await customersService.CustomerCreate(command);

        CustomerCreatedResult.Success.Should().BeFalse();
    }


    [Test]
    public async Task CustomerCreateExists()
    {
        var command = new CommandCustomerCreate("+79871234561", "Александр Лейб");
        //Создали пользователя
        await customersService.CustomerCreate(command);
        //Пробуем создать еще раз с тем же номером телефона
        var CustomerExistsResult = await customersService.CustomerCreate(command);

        CustomerExistsResult.Success.Should().BeFalse();
    }



}


