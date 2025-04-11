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

        #region Setting
        public async Task<Result<BotSetting>> GetSetting(string key)
        {
            return await botRepository.GetSetting(key);
        }

        public Task<Result<BotSetting>> UpdateSetting(BotSetting setting)
        {
            throw new NotImplementedException();
        }
        #endregion Setting

        #region Factor
        public async Task<Result<Factor>> CreateFactor(Factor factor)
        {
            return await botRepository.CreateFactor(factor);
        }

        public async Task<Result<Factor>> DeleteFactor(Guid factorId)
        {
            return await botRepository.DeleteFactor(factorId);
        }

        public async Task<Result<Factor>> GetFactor(Guid factorId)
        {
            return await botRepository.GetFactor(factorId);
        }

        public async Task<Result<Factor>> UpdateFactor(Factor factor)
        {
            return await botRepository.UpdateFactor(factor);
        }
        #endregion Factor

        #region Message
        public async Task<Result<ICollection<BotMessage>>> GetAllMessages()
        {
            return await botRepository.GetAllBotMessages();
        }

        public async Task<Result<ICollection<BotSetting>>> GetAllSettings()
        {
            return await botRepository.GetAllSettings();
        }
        public Task<Result<BotMessage>> UpdateMessage(BotMessage message)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
