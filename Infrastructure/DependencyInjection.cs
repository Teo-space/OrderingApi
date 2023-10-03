using FluentValidation.AspNetCore;
using Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

public static class DependencyInjection__ForumsInfrastructure
{
    static void Configure(this IServiceCollection Services)
    {
        Services.AddLogging();

        Services.AddFluentValidationAutoValidation();
        Services.AddFluentValidationClientsideAdapters();
        Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }


    public static void AddInfrastructureUseSqlServer(this IServiceCollection Services, string ConnectionString)
    {
        Services.Configure();

        Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(ConnectionString);
        });

    }



    public static void AddInfrastructureUseSqlite(this IServiceCollection Services
        , string ConnectionString= $"FileName=AppDbContext.db")
	{
        Services.Configure();

        Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlite(ConnectionString);
		});
	}

	public static void AddInfrastructureUseInMemoryDatabase(this IServiceCollection Services)
	{
        Services.Configure();


        Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseInMemoryDatabase("AppDbContext");
		});

	}


    public static void AddInfrastructureUseMySql(this IServiceCollection Services
        , string ConnectionString = "Server=localhost;Uid=mysql;Pwd=mysql;Database=OrderingApi;")
	{
        Services.Configure();

        Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
		});

	}



}