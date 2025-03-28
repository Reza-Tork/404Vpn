using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructureServices();
            })
            .Build();

            //var botService = host.Services.GetRequiredService<BotService>();
            //await botService.Start();

            //Console.WriteLine("Bot is running...");

            //await Task.Delay(Timeout.Infinite);

            //botService.Stop();
        }
    }
}
