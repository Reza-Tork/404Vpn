using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers.Handlers;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Services
{
    public class UpdateHandlerService : IUpdateHandler
    {
        private readonly IUserService userService;
        private readonly ILogger<UpdateHandlerService> logger;
        private readonly IUpdateHandler messageHandler;
        private readonly IUpdateHandler callbackHandler;
        public UpdateHandlerService(
            IUserService userService,
            ILogger<UpdateHandlerService> logger,
            [FromKeyedServices("MessageHandler")] IUpdateHandler messageHandler,
            [FromKeyedServices("CallbackQueryHandler")] IUpdateHandler callbackHandler)
        {
            this.userService = userService;
            this.logger = logger;
            this.messageHandler = messageHandler;
            this.callbackHandler = callbackHandler;
        }
        public async Task HandleUpdate(Update update)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            await messageHandler.HandleUpdate(update);
                        }
                        break;
                    case UpdateType.CallbackQuery:
                        {
                            await callbackHandler.HandleUpdate(update);
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                logger.LogError("[Update Handler] {0}", ex.Message);
            }
        }
    }
}
