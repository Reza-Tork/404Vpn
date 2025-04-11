using Application.Common;
using Domain.Entities.Bot;
using Telegram.Bot.Types;

namespace Application.Interfaces
{
    public interface IBotService
    {
        Task<Result<ICollection<BotSetting>>> GetAllSettings();
        Task<Result<BotSetting>> GetSetting(string key);
        Task<Result<BotSetting>> UpdateSetting(BotSetting setting);
        Task<Result<BotMessage>> UpdateMessage(BotMessage message);
        Task<Result<ICollection<BotMessage>>> GetAllMessages();

        Task<Result<Factor>> CreateFactor(Factor factor);
        Task<Result<Factor>> UpdateFactor(Factor factor);
        Task<Result<Factor>> DeleteFactor(Guid factorId);
        Task<Result<Factor>> GetFactor(Guid factorId);
    }
}
