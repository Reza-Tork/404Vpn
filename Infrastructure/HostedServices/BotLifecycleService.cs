using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities.Bot;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.HostedServices
{
    public class BotLifecycleService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfiguration botConfiguration;

        public BotLifecycleService(
            ITelegramBotClient botClient,
            BotConfiguration botConfiguration)
        {
            _botClient = botClient;
            this.botConfiguration = botConfiguration;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webhookInfo = await _botClient.GetWebhookInfo(cancellationToken);
            if (webhookInfo.Url != string.Empty)
                await _botClient.DeleteWebhook(false, cancellationToken);

            await _botClient.SetMyCommands(new List<BotCommand>()
            {
                new BotCommand("/start", "شروع مجدد ربات")
            });
            await _botClient.SetWebhook($"{botConfiguration.Domain}/webhook",allowedUpdates: [UpdateType.Message, UpdateType.CallbackQuery], cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var webhookInfo = await _botClient.GetWebhookInfo(cancellationToken); 
            await _botClient.DeleteMyCommands(cancellationToken: cancellationToken);
            if (webhookInfo.Url != string.Empty)
                await _botClient.DeleteWebhook(false, cancellationToken);
        }
    }
}
