using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Infrastructure.DI
{
    public static class DependencyContainer
    {
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
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IVpnService, VpnService>();

            services.AddHttpClient<ITelegramBotClient, TelegramBotClient>((httpClient, sp) =>
                new TelegramBotClient("YOUR_BOT_TOKEN", httpClient)
            );

            return services;
        }
    }
}
