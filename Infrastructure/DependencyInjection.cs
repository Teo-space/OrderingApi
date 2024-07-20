using Infrastructure.EntityFrameworkCore;
using Interfaces.DbContexts;

public static class DependencyInjection__ForumsInfrastructure
{
    public static void AddInfrastructureUseSqlServer(this IServiceCollection Services, string ConnectionString)
    {
        Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(ConnectionString);
        });
        Services.AddScoped<IAppDbContext, AppDbContext>();
    }

    public static void AddInfrastructureUseSqlite(this IServiceCollection Services
        , string ConnectionString= $"FileName=AppDbContext.db")
	{
        Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlite(ConnectionString);
		});

        Services.AddScoped<IAppDbContext, AppDbContext>();
    }

	public static void AddInfrastructureUseInMemoryDatabase(this IServiceCollection Services, string DataBaseName)
	{
        Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseInMemoryDatabase(DataBaseName);
		});

        Services.AddScoped<IAppDbContext, AppDbContext>();
    }


    public static void AddInfrastructureUseMySql(this IServiceCollection Services, 
        string ConnectionString = "Server=localhost;Uid=mysql;Pwd=mysql;Database=OrderingApi;")
	{
        Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
		});

        Services.AddScoped<IAppDbContext, AppDbContext>();
    }



}