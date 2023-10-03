using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UseCases.Customers.Service;

public static class ADependencyInjection
{
    public static void AddUseCases(this IServiceCollection Services)
    {
        Services.AddLogging();

        //builder.Services.AddFluentValidation();
        Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        Services.AddFluentValidationAutoValidation();
        Services.AddFluentValidationClientsideAdapters();


        Services.AddScoped<ICustomersService, CustomersService>();
        Services.AddScoped<IOrderCartService, OrderCartService>();
        Services.AddScoped<IOrderingService, OrderingService>();
        Services.AddScoped<ICatalogService, CatalogService>();






    }







}
