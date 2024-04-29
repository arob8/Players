using Players.Infrastructure.Services;
using Players.Infrastructure.Interfaces;
using Players.Infrastructure.CbsSports.Client;
using Players.Domain.Interfaces;
using Players.Infrastructure.Database.Repositories;
using Players.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Players.Infrastructure.Factory.Basketball;
using Players.Infrastructure.Factory.Football;
using Players.Infrastructure.Factory.Baseball;
using Players.Infrastructure.DataProcessor.Football;
using Players.Infrastructure.DataProcessor.Basketball;
using Players.Infrastructure.DataProcessor.Baseball;
using Players.Infrastructure.Calculations;
using Players.Domain.Utilities.NameFormatter.Baseball;
using Players.Domain.Utilities.NameFormatter.Basketball;
using Players.Domain.Utilities.NameFormatter.Football;

namespace Players.API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _enviroment;

        public Startup(IConfiguration config, IWebHostEnvironment environment)
        {
            _config = config;
            _enviroment = environment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // cookie auth and token auth
            //services.AddAuthentication().AddCookie().AddJwtBearer(
            //  cfg =>
            //  cfg.TokenValidationParameters = new TokenValidationParameters()
            //  {
            //      ValidIssuer = _config["Tokens:Issuer"],
            //      ValidAudience = _config["Tokens:Audience"],
            //      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
            //  }
            //);

            // services.AddScoped - lives for the length of request
            // services.AddSingleton - lives for as long as the program is alive

            var connectionString = _config.GetConnectionString("SQLServer");
            if (connectionString == null)
            {
                throw new InvalidOperationException("Connection string 'SQLServer' is not configured.");
            }

            // Configuration for DbContext
            services.AddDbContext<PlayersContext>(options => options.UseSqlServer(connectionString));

            // Seeder
            //services.AddTransient<PlayerSeeder>();

            // Registering the external client
            services.AddSingleton<HttpClient>();
            services.AddScoped<IClient, CbsSportsClient>();

            // Registering Domain services
            services.AddScoped<FootballPlayerNameFormatter>();
            services.AddScoped<BaseballPlayerNameFormatter>();
            services.AddScoped<BasketballPlayerNameFormatter>();

            // Registering infrastructure services
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IPlayerDataCalculator, PlayerDataCalculator>();
            services.AddScoped<FootballPlayerFactory>();
            services.AddScoped<BaseballPlayerFactory>();
            services.AddScoped<BasketballPlayerFactory>();
            services.AddScoped<FootballPlayerDataProcessor>();
            services.AddScoped<BaseballPlayerDataProcessor>();
            services.AddScoped<BasketballPlayerDataProcessor>();

            // Registering application services
            //services.AddScoped<IPlayerDtoDataMapper, PlayerDTODataMapper>();
            //services.AddScoped<IPlayerService, Application.Services.PlayerService>();

            // Registering background services
            services.AddHostedService<PlayerDataBackgroundService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<PlayersContext>();
            //    db.Database.Migrate();
            //}
        }
    }
}