using Infrastructure;
using Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderingApi.AppFilters;



var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddLogging();
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    });

    //����� ������������ ResponseCaching, OutputCache, �������� ���� MiddleWare
    //��� DistributedMemoryCache �������� � Redis ��� ������ Key-Value
    builder.Services.AddResponseCaching();
    builder.Services.AddOutputCache(options =>
    {
        //����� ��������� �������� ��� ����
        options.AddBasePolicy(builder => builder.Cache());           
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.AddAndConfigureSwagger();

    {
        //builder.AddInfrastructureUseSqlite();
        //builder.AddInfrastructureUseMySql();
        builder.Services.AddInfrastructureUseSqlServer(builder.Configuration.GetConnectionString("MSSqlLocal"));
        //User Secrets:
        //"ConnectionStrings:MSSqlLocal": "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=aspnet-OrderingApi-53bc9b9d-9d6a-45d4-8429-2a2761773502;Integrated Security=True;Multiple Active Result Sets=True"

        builder.Services.AddUseCases();
    }

}
var app = builder.Build();
{
    {   
        //���������� �� ��������� �������
        //���������� �� ��������� �������
        //���������� �� ��������� �������
        //���������� �� ��������� �������
        TestDataInitializer.Init(app.Services);
    }


    if (app.Environment.IsDevelopment())
    {
        app.UseAndConfigureSwagger();
    }


    app.UseHttpsRedirection();

    app.UseResponseCaching();

    app.UseAuthorization();

    app.UseOutputCache();

    app.MapControllers();

    app.Run();


}
