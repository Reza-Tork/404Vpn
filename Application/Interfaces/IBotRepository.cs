using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities.Bot;

namespace Application.Interfaces
{
    public interface IBotRepository
    {
        Task<Result<BotSetting>> AddSetting(BotSetting setting);
        Task<Result<BotSetting>> UpdateSetting(BotSetting setting);
        Task<Result<BotSetting>> DeleteSetting(int botSettingId);
        Task<Result<BotSetting>> GetSetting(int botSettingId);
        Task<Result<ICollection<BotSetting>>> GetAllSettings();
    }
}
