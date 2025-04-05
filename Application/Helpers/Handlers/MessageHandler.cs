using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Application.Helpers.Handlers
{
    public class MessageHandler : IUpdateHandler
    {
        private readonly IUserService userService;
        private readonly IBotService botService;
        private readonly ITelegramBotClient botClient;

        public MessageHandler(IUserService userService,IBotService botService, ITelegramBotClient botClient)
        {
            this.userService = userService;
            this.botService = botService;
            this.botClient = botClient;
        }
        public async Task HandleUpdate(Update update)
        {
            var allBotMessages = (await botService.GetAllMessages()).Data.ToList();
            var user = await userService.CheckUserExists(update.Message!.From!);
            var message = update.Message;
            var chatId = update.Message.Chat.Id;
            if (message.Text!.IsCommand(out Domain.Entities.Enums.BotCommand command))
            {
                var responseMessage = allBotMessages[(int)command - 1];
                await botClient.SendMessage(chatId, responseMessage.Message, replyMarkup: BotKeyboards.MainKeyboard());
            }
        }
    }
}
