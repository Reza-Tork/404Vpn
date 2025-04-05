using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities.Bot;
using Domain.Entities.Enums;

namespace Application.Interfaces
{
    public interface IBotRepository
    {
        Task<Result<BotSetting>> AddSetting(BotSetting setting);
        Task<Result<BotSetting>> UpdateSetting(BotSetting setting);
        Task<Result<BotSetting>> DeleteSetting(int botSettingId);
        Task<Result<BotSetting>> GetSetting(int botSettingId);
        Task<Result<BotSetting>> GetSetting(string settingKey);
        Task<Result<ICollection<BotSetting>>> GetAllSettings();

        Task<Result<BotMessage>> UpdateBotMessage(int botMessageId, string value);
        Task<Result<BotMessage>> UpdateBotMessage(BotCommand command, string value);
        Task<Result<ICollection<BotMessage>>> GetAllBotMessages();
    }
}
