using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
<<<<<<< HEAD
using Microsoft.Extensions.DependencyInjection;
=======
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
>>>>>>> Initial Project
using Telegram.Bot;

namespace Infrastructure.DI
{
    public static class DependencyContainer
    {
<<<<<<< HEAD
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            //services.AddDbContext<BotDbContext>(options =>
            //{
            //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
            //        sqlOptions =>
            //        {
            //            sqlOptions.EnableRetryOnFailure(20);
            //            sqlOptions.MigrationsAssembly("Data");
            //        });

            //});
=======
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Postgres");

            services.AddHttpClient();

            services.AddDbContext<BotDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
>>>>>>> Initial Project
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IVpnService, VpnService>();

            services.AddHttpClient<ITelegramBotClient, TelegramBotClient>((httpClient, sp) =>
<<<<<<< HEAD
                new TelegramBotClient("YOUR_BOT_TOKEN", httpClient)
=======
                new TelegramBotClient(configuration["BotSettings:BotToken"], httpClient)
>>>>>>> Initial Project
            );

            return services;
        }
    }
}
