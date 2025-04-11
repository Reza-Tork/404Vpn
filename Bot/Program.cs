using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Helpers;
using Application.Interfaces;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Domain.Entities.Vpn;
using Infrastructure.DI;
using Infrastructure.HostedServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.SetMinimumLevel(LogLevel.Error);
            builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.None);

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddHostedService<BotLifecycleService>();
            builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);

            var app = builder.Build();

            app.MapPost("/webhook", async (HttpContext context, [FromKeyedServices("UpdateHandler")] IUpdateHandler updateHandler, ILogger<Program> logger, Update update) =>
            {
                logger.LogInformation($"Update received: {update.Type}");
                if (update != null)
                {
                    await updateHandler.HandleUpdate(update);
                }

                return Results.Ok();
            });

            app.Run();
        }
    }
}
