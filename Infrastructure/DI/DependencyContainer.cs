using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Infrastructure.DbContext;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Infrastructure.DI
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Postgres");

            services.AddHttpClient<IVpnService, VpnService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddDbContext<BotDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IBotRepository, BotRepository>();
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IVpnRepository, VpnRepository>();
            services.AddScoped<IVpnService, VpnService>();

            services.AddHttpClient<ITelegramBotClient, TelegramBotClient>((httpClient, sp) =>
                new TelegramBotClient(configuration["BotSettings:BotToken"], httpClient)
            );

            return services;
        }
    }
}
