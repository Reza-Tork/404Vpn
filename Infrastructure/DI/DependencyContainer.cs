using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Helpers.Handlers;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.Bot;
using Infrastructure.DbContext;
using Infrastructure.HostedServices;
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
            var botSettings = configuration.GetSection("BotSettings").Get<BotConfiguration>()!;
            services.AddSingleton(botSettings);
            string connectionString = configuration.GetConnectionString("Postgres")!;

            services.AddHttpClient();
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            });

            services.AddDbContextPool<BotDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddMemoryCache();

            services.AddScoped<IBotRepository, BotRepository>();
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IVpnRepository, VpnRepository>();
            services.AddScoped<IVpnService, VpnService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddKeyedScoped<IUpdateHandler, UpdateHandlerService>("UpdateHandler");
            services.AddKeyedScoped<IUpdateHandler, MessageHandler>("MessageHandler");
            services.AddKeyedScoped<IUpdateHandler, CallbackQueryHandler>("CallbackQueryHandler");

            services.AddHttpClient<ITelegramBotClient, TelegramBotClient>((httpClient, sp) =>
                new TelegramBotClient(botSettings.BotToken, httpClient)
            );

            return services;
        }
    }
}
