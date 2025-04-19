using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain.Entities.Bot;
using Domain.Entities.Enums;
using Domain.Entities.Vpn;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repository
{
    public class BotRepository : IBotRepository
    {
        private readonly IMemoryCache cache;
        private readonly BotDbContext dbContext;
        private const string MessagesCacheKey = "BotMessages";
        private const string SettingsCacheKey = "BotSettings";
        private const string FactorsCacheKey = "Factor";
        public BotRepository(BotDbContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        #region Settings
        public async Task<Result<BotSetting>> AddSetting(BotSetting setting)
        {
            dbContext.BotSettings.Add(setting);
            await dbContext.SaveChangesAsync();

            cache.Remove(SettingsCacheKey);

            return Result<BotSetting>.Success("Ok", setting);
        }

        public async Task<Result<BotSetting>> DeleteSetting(int botSettingId)
        {
            var result = await GetSetting(botSettingId);
            if (result.IsSuccess)
            {
                dbContext.BotSettings.Remove(result.Data!);
                await dbContext.SaveChangesAsync();


                cache.Remove(SettingsCacheKey);
                cache.Remove($"{SettingsCacheKey}_{result.Data!.Id}");
                cache.Remove($"{SettingsCacheKey}_{result.Data!.Key}");

                return Result<BotSetting>.Success("Bot setting Successfully removed!");
            }
            else
                return result;
        }

        public async Task<Result<ICollection<BotSetting>>> GetAllSettings()
        {
            if (cache.TryGetValue(SettingsCacheKey, out ICollection<BotSetting> cachedSettings))
                return Result<ICollection<BotSetting>>.Success(cachedSettings);

            var botSettings = await dbContext.BotSettings.AsNoTracking().ToListAsync();

            cache.Set(SettingsCacheKey, botSettings, TimeSpan.FromHours(1));

            return Result<ICollection<BotSetting>>.Success(botSettings);
        }

        public async Task<Result<BotSetting>> GetSetting(int botSettingId)
        {
            if (cache.TryGetValue($"{SettingsCacheKey}_{botSettingId}", out BotSetting? cachedSettings))
                return Result<BotSetting>.Success(cachedSettings);


            var result = await dbContext.BotSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == botSettingId);
            if (result != null)
            {
                cache.Set($"{SettingsCacheKey}_{botSettingId}", result, TimeSpan.FromHours(1));
                return Result<BotSetting>.Success("Bot setting founded!", result);
            }
            return Result<BotSetting>.Failure("No bot setting not founded!");
        }

        public async Task<Result<BotSetting>> GetSetting(string settingKey)
        {
            if (cache.TryGetValue($"{SettingsCacheKey}_{settingKey}", out BotSetting? cachedSettings))
                return Result<BotSetting>.Success(cachedSettings);

            var result = await dbContext.BotSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Key == settingKey);
            if (result != null)
            {
                cache.Set($"{SettingsCacheKey}_{settingKey}", result, TimeSpan.FromHours(1));
                return Result<BotSetting>.Success("Bot setting founded!", result);
            }
            return Result<BotSetting>.Failure("No bot setting not founded!");
        }

        public async Task<Result<BotSetting>> UpdateSetting(BotSetting setting)
        {
            dbContext.BotSettings.Update(setting);
            await dbContext.SaveChangesAsync();

            cache.Remove(SettingsCacheKey);
            cache.Remove($"{SettingsCacheKey}_{setting.Id}");
            cache.Remove($"{SettingsCacheKey}_{setting.Key}");

            return Result<BotSetting>.Success("Api info successfully updated.", setting);
        }
        #endregion

        #region Messages
        public async Task<Result<BotMessage>> UpdateBotMessage(int botMessageId, string value)
        {
            var botMessage = await dbContext.BotMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == botMessageId);
            if (botMessage != null)
            {
                botMessage.Message = value;
                dbContext.BotMessages.Update(botMessage);
                await dbContext.SaveChangesAsync();

                cache.Remove(MessagesCacheKey);

                return Result<BotMessage>.Success(botMessage);
            }
            return Result<BotMessage>.Failure("Bot message doesn't exist!");
        }

        public async Task<Result<BotMessage>> UpdateBotMessage(BotCommand command, string value)
        {
            var botMessage = await dbContext.BotMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Command == command);
            if (botMessage != null)
            {
                botMessage.Message = value;
                dbContext.BotMessages.Update(botMessage);
                await dbContext.SaveChangesAsync();

                cache.Remove(MessagesCacheKey);

                return Result<BotMessage>.Success(botMessage);
            }
            return Result<BotMessage>.Failure("Bot message doesn't exist!");
        }
        public async Task<Result<ICollection<BotMessage>>> GetAllBotMessages()
        {
            if (cache.TryGetValue(MessagesCacheKey, out ICollection<BotMessage>? cachedMessages))
                return Result<ICollection<BotMessage>>.Success(cachedMessages);

            var messages = await dbContext.BotMessages.AsNoTracking().ToListAsync();
            cache.Set(MessagesCacheKey, messages, TimeSpan.FromHours(1));

            return Result<ICollection<BotMessage>>.Success(messages);
        }
        #endregion

        #region Factor
        public async Task<Result<Factor>> CreateFactor(Factor factor)
        {
            dbContext.Factors.Add(factor);
            await dbContext.SaveChangesAsync();

            cache.Set($"{FactorsCacheKey}_{factor.Id}", factor);
            cache.Set($"{FactorsCacheKey}_{factor.UniqueKey}", factor);

            return Result<Factor>.Success(factor);
        }

        public async Task<Result<Factor>> UpdateFactor(Factor factor)
        {
            dbContext.Factors.Update(factor);
            await dbContext.SaveChangesAsync();

            cache.Set($"{FactorsCacheKey}_{factor.Id}", factor);
            cache.Set($"{FactorsCacheKey}_{factor.UniqueKey}", factor);

            return Result<Factor>.Success(factor);
        }

        public async Task<Result<Factor>> DeleteFactor(int factorId)
        {
            var factor = await GetFactor(factorId);
            if (factor.Data == null || !factor.IsSuccess)
                return Result<Factor>.Failure("Factor not found!");

            dbContext.Factors.Remove(factor.Data);
            await dbContext.SaveChangesAsync();

            cache.Remove($"{FactorsCacheKey}_{factorId}");
            cache.Remove($"{FactorsCacheKey}_{factor.Data.UniqueKey}");

            return Result<Factor>.Success("Factor removed", factor.Data);
        }

        public async Task<Result<Factor>> GetFactor(int factorId)
        {
            if (cache.TryGetValue($"{FactorsCacheKey}_{factorId}", out Factor? cachedFactor))
                return Result<Factor>.Success(cachedFactor);

            var factor = await dbContext.Factors
                .Include(x => x.UserSubscription)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == factorId);

            if (factor == null)
                return Result<Factor>.Failure("Factor not found!");
            return Result<Factor>.Success(factor);
        }
        public async Task<Result<Factor>> GetFactorByUniqueKey(string factorUniqueKey)
        {
            if (cache.TryGetValue($"{FactorsCacheKey}_{factorUniqueKey}", out Factor? cachedFactor))
                return Result<Factor>.Success(cachedFactor);

            var factor = await dbContext.Factors
                .Include(x => x.UserSubscription)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.UniqueKey == factorUniqueKey);

            if (factor == null)
                return Result<Factor>.Failure("Factor not found!");
            return Result<Factor>.Success(factor);
        }
        #endregion
    }
}
