using Infrastructure;
using OrderingApi.AppFilters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    });

    //Можно использовать ResponseCaching, OutputCache, написать свой MiddleWare
    //Или DistributedMemoryCache например в Redis или другом Key-Value
    builder.Services.AddResponseCaching();
    builder.Services.AddOutputCache(options =>
    {
        //Можно настроить политику под себя
        options.AddBasePolicy(builder => builder.Cache());           
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Domain.xml"));
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"UseCases.xml"));
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"ApiContracts.xml"));
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Results.xml"));

    });
    {
        builder.Services.AddInfrastructureUseSqlite();
        //builder.Services.AddInfrastructureUseMySql();
        //builder.Services.AddInfrastructureUseSqlServer(builder.Configuration.GetConnectionString("MSSqlLocal"));
        //User Secrets:
        //"ConnectionStrings:MSSqlLocal": "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=aspnet-OrderingApi-53bc9b9d-9d6a-45d4-8429-2a2761773502;Integrated Security=True;Multiple Active Result Sets=True"

        builder.Services.AddUseCases();
    }

}
var app = builder.Build();
{
    {   
        //Наполнение БД тестовыми данными
        TestDataInitializer.Init(app.Services);
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });
    }


    app.UseHttpsRedirection();

    app.UseResponseCaching();

    app.UseAuthorization();

    app.UseOutputCache();

    app.MapControllers();

    app.Run();


}
