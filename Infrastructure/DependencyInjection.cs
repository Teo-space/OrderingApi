using FluentValidation.AspNetCore;
using Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

public static class DependencyInjection__ForumsInfrastructure
{
    static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddLogging();

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }


    public static void AddInfrastructureUseSqlServer(this WebApplicationBuilder builder, string ConnectionString)
    {
        builder.Configure();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(ConnectionString);
        });

    }



    public static void AddInfrastructureUseSqlite(this WebApplicationBuilder builder
		, string ConnectionString= $"FileName=AppDbContext.db")
	{
		builder.Configure();

		builder.Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlite(ConnectionString);
		});
	}

	public static void AddInfrastructureUseInMemoryDatabase(this WebApplicationBuilder builder)
	{
        builder.Configure();


        builder.Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseInMemoryDatabase("ForumTest");
		});

	}


    public static void AddInfrastructureUseMySql(this WebApplicationBuilder builder
		, string ConnectionString = "Server=localhost;Uid=mysql;Pwd=mysql;Database=OrderingApi;")
	{
        builder.Configure();

        builder.Services.AddDbContext<AppDbContext>(options =>
		{
			options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
		});

	}



}