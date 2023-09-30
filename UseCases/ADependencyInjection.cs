using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UseCases.Customers.Service;

public static class ADependencyInjection
{
    public static WebApplicationBuilder AddUseCases(this WebApplicationBuilder builder)
    {
        builder.Services.AddLogging();

        //builder.Services.AddFluentValidation();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();


        builder.Services.AddScoped<ICustomersService, CustomersService>();
        builder.Services.AddScoped<IOrderCartService, OrderCartService>();
        builder.Services.AddScoped<IOrderingService, OrderingService>();
        builder.Services.AddScoped<ICatalogService, CatalogService>();





        return builder;
    }







}
