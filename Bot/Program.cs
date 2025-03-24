using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace Bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var host = Host.CreateDefaultBuilder(args)
            //.ConfigureServices((context, services) =>
            //{
            //    var connectionString = "Server=78.47.214.54;Database=unknown_bot;User=unknown_bot_user;Password=DFdsfddf#R$#22!1#$!!;Port=3306;";

            //    //services.AddDbContext<BotDbContext>(options =>
            //    //{
            //    //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
            //    //        sqlOptions =>
            //    //        {
            //    //            sqlOptions.EnableRetryOnFailure(20);
            //    //            sqlOptions.MigrationsAssembly("Data");
            //    //        });

            //    //});

            //    services.AddHttpClient<ITelegramBotClient, TelegramBotClient>((httpClient, sp) =>
            //        new TelegramBotClient("7497401140:AAG0Cl20z602H0V9Uk8gmTnxrUOGtBl_BUU", httpClient)
            //    );

            //    services.AddScoped<BotService>();
            //})
            //.Build();

            //var botService = host.Services.GetRequiredService<BotService>();
            //await botService.Start();

            //Console.WriteLine("Bot is running...");

            //await Task.Delay(Timeout.Infinite);

            //botService.Stop();
        }
    }
}
