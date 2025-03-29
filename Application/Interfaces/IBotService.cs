using Application.Common;
using Domain.Entities.Bot;

namespace Application.Interfaces
{
    public interface IBotService
    {
        Task Run();

        Task<Result<BotSetting>> GetAllSettings();
        Task<Result<BotSetting>> GetSetting(string key);
        Task<Result<BotSetting>> UpdateSetting(BotSetting setting);
    }
}
