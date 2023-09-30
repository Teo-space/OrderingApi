using Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class TestDataInitializer
{
    public static void Init(IServiceProvider serviceProvider)
    {
        using(var scope = serviceProvider.CreateScope())
        {
            ScopedContext(scope);
        }
    }

    static void ScopedContext(IServiceScope serviceScope)
    {
        ICatalogService     catalogService = serviceScope.ServiceProvider.GetRequiredService<ICatalogService>();
        ICustomersService   customersService = serviceScope.ServiceProvider.GetRequiredService<ICustomersService>();
        IOrderCartService   orderCartService = serviceScope.ServiceProvider.GetRequiredService<IOrderCartService>();
        IOrderingService    orderingService = serviceScope.ServiceProvider.GetRequiredService<IOrderingService>();
        AppDbContext            dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        //Проверка что тестовые данные уже есть
        var types = catalogService.GetProductTypes()
            .GetAwaiter().GetResult();
        if(types.Success && types.Value.Any())
        {
            return;
        }

        //Создаём пользователя
        //CommandCustomerCreate(string PhoneNumber, string UserName)
        var CustomerResult = customersService.CustomerCreate(new("+79871234567", "Василий Петрович Пупкин"))
            .GetAwaiter().GetResult();
        var Customer = CustomerResult.Value;


        //Создаем несколько типов товаров
        //CommandProductTypeCreate(string Name)
        var catMotorOilsResult = catalogService.ProductTypeCreate(new("Моторные масла"))
            .GetAwaiter().GetResult();
        var catMotorOils = catMotorOilsResult.Value;
        var catTransmissionOilsResult = catalogService.ProductTypeCreate(new("Трансмиссионные масла"))
            .GetAwaiter().GetResult();
        var catPowerSteeringOilsResult = catalogService.ProductTypeCreate(new("Масла для ГУР"))
            .GetAwaiter().GetResult();
        var catOtherOilsResult = catalogService.ProductTypeCreate(new("Масла прочие"))
            .GetAwaiter().GetResult();


        //Создаем несколько товаров
        //CommandProductCreate(IdType ProductTypeId, string Name, double Price, double QuanityInStock
        var OilMobil1Result = catalogService.ProductCreate(new CommandProductCreate(
            catMotorOilsResult.Value.ProductTypeId, 
            "MOBIL 1 Synthetic Motor oil 0W-40",
            1150, 32d))
            .GetAwaiter().GetResult();
        var OilMobil1 = OilMobil1Result.Value;

        var OilMobilSuperResult = catalogService.ProductCreate(new CommandProductCreate(
            catMotorOilsResult.Value.ProductTypeId,
            "MOBIL Super 2000 X1 10W-40",
            2673, 14d))
            .GetAwaiter().GetResult();

        var OilMobilDelvacResult = catalogService.ProductCreate(new CommandProductCreate(
            catMotorOilsResult.Value.ProductTypeId,
            "MOBIL Delvac MX 15W-40",
            15777, 15d))
            .GetAwaiter().GetResult();


        //Помещаем товар в корзину
        //CommandOrderCartItemAdd(IdType CustomerId, IdType ProductId, double Quanity)
        var cartItemResult = orderCartService
            .AddItem(new(Customer.CustomerId, OilMobil1Result.Value.ProductId, 1d))
            .GetAwaiter().GetResult();

        //Пробуем завершить заказ по товарам в корзине пользователя
        //CommandOrderCheckOut(IdType CustomerId)
        orderingService
            .OrderCheckOut(new(Customer.CustomerId))
            .GetAwaiter().GetResult();


    }




}
 