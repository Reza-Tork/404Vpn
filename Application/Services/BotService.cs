using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain.Entities.Bot;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Services
{
    public class BotService : IBotService
    {
        private readonly IBotRepository botRepository;
        public BotService(
            IBotRepository botRepository)
        {
            this.botRepository = botRepository;
        }

        public async Task<Result<ICollection<BotMessage>>> GetAllMessages()
        {
            return await botRepository.GetAllBotMessages();
        }

        public async Task<Result<ICollection<BotSetting>>> GetAllSettings()
        {
            return await botRepository.GetAllSettings();
        }

        public async Task<Result<BotSetting>> GetSetting(string key)
        {
            return await botRepository.GetSetting(key);
        }

        public Task<Result<BotMessage>> UpdateMessage(BotMessage message)
        {
            throw new NotImplementedException();
        }

        public Task<Result<BotSetting>> UpdateSetting(BotSetting setting)
        {
            throw new NotImplementedException();
        }
    }
}
