using FluentValidation.AspNetCore;
using Interfaces.Services.Catalog;
using Interfaces.Services.Customers;
using Interfaces.Services.OrderCart;
using Interfaces.Services.Ordering;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class ADependencyInjection
{
    public static void AddServices(this IServiceCollection Services)
    {
        Services.AddLogging();

        //builder.Services.AddFluentValidation();
        Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        Services.AddFluentValidationAutoValidation();
        Services.AddFluentValidationClientsideAdapters();


        Services.AddScoped<ICustomersService, CustomersService>();
        Services.AddScoped<IOrderCartService, OrderCartService>();
        Services.AddScoped<IOrderingService, OrderingService>();
        Services.AddScoped<IProductTypeService, ProductTypeService>();
        Services.AddScoped<IProductService, ProductService>();

    }

}
