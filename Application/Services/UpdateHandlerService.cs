using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers.Handlers;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Services
{
    public class UpdateHandlerService : IUpdateHandler
    {
        private readonly IUserService userService;
        private readonly IUpdateHandler messageHandler;
        private readonly IUpdateHandler callbackHandler;
        public UpdateHandlerService(
            IUserService userService,
            [FromKeyedServices("MessageHandler")] IUpdateHandler messageHandler,
            [FromKeyedServices("CallbackQueryHandler")] IUpdateHandler callbackHandler)
        {
            this.userService = userService;
            this.messageHandler = messageHandler;
            this.callbackHandler = callbackHandler;
        }
        public async Task HandleUpdate(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var user = await userService.CheckUserExists(update.Message.From);
                        await messageHandler.HandleUpdate(update);
                    }
                    break;
                case UpdateType.CallbackQuery:
                    {
                        var user = await userService.CheckUserExists(update.CallbackQuery.From);
                        await callbackHandler.HandleUpdate(update);
                    }
                    break;
            }
        }
    }
}
