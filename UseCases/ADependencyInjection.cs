using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class ADependencyInjection
{
    public static void AddUseCases(this IServiceCollection Services)
    {
        Services.AddLogging();

        //builder.Services.AddFluentValidation();
        Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        Services.AddFluentValidationAutoValidation();
        Services.AddFluentValidationClientsideAdapters();
    }

}
