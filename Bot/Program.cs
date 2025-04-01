using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Application.Helpers;
using Application.Interfaces;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Error);
                    logging.AddFilter("System.Net.Http.HttpClient", LogLevel.None);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructureServices(context.Configuration);
                })
                .Build();

            var vpnService = host.Services.GetRequiredService<IVpnService>();
            var result = await vpnService.SignInAdmin(new LoginRequestDTO()
            {
                username = "arman",
                password = "arman2122"
            });
            if (result.IsSuccess)
            {
                Console.WriteLine($"[+] Token: {result.Data!.Token}");
            }


            
            //await botService.Start();

            //Console.WriteLine("Bot is running...");

            //await Task.Delay(Timeout.Infinite);

            //botService.Stop();
        }
    }
}
