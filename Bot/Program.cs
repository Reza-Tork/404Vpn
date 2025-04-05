using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Helpers;
using Application.Interfaces;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Infrastructure.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
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

            var app = builder.Build();

            app.MapPost("/webhook", async (HttpContext context, [FromKeyedServices("UpdateHandler")] IUpdateHandler updateHandler) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();

                var update = System.Text.Json.JsonSerializer.Deserialize<Update>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

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
