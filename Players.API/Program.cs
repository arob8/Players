using Players.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        //using (var scope = host.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;
        //    try
        //    {
        //        var seeder = services.GetRequiredService<PlayerSeeder>();
        //        await seeder.Seed();
        //    }
        //    catch (Exception ex)
        //    {
        //        var logger = services.GetRequiredService<ILogger<Program>>();
        //        logger.LogError(ex, "An error occurred while seeding the data.");
        //    }
        //}

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(AddConfiguration)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

    private static void AddConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
    {
        builder.Sources.Clear();

        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();
    }
}