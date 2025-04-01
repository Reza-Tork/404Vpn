using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain.Entities.Bot;
using Domain.Entities.Vpn;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class BotRepository : IBotRepository
    {
        private readonly BotDbContext dbContext;

        public BotRepository(BotDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Result<BotSetting>> AddSetting(BotSetting setting)
        {
            dbContext.BotSettings.Add(setting);
            await dbContext.SaveChangesAsync();
            return Result<BotSetting>.Success("Ok", setting);
        }

        public async Task<Result<BotSetting>> DeleteSetting(int botSettingId)
        {
            var result = await GetSetting(botSettingId);
            if (result.IsSuccess)
            {
                dbContext.BotSettings.Remove(result.Data!);
                await dbContext.SaveChangesAsync();
                return Result<BotSetting>.Success("Bot setting Successfully removed!");
            }
            else
                return result;
        }

        public async Task<Result<ICollection<BotSetting>>> GetAllSettings()
        {
            var botSettings = await dbContext.BotSettings.ToListAsync();
            return Result<ICollection<BotSetting>>.Success("Ok", botSettings);
        }

        public async Task<Result<BotSetting>> GetSetting(int botSettingId)
        {
            var result = await dbContext.BotSettings.FirstOrDefaultAsync(x => x.Id == botSettingId);
            if (result != null)
                return Result<BotSetting>.Success("Bot setting founded!", result);
            return Result<BotSetting>.Failure("No bot setting not founded!");
        }

        public async Task<Result<BotSetting>> UpdateSetting(BotSetting setting)
        {
            dbContext.BotSettings.Update(setting);
            await dbContext.SaveChangesAsync();
            return Result<BotSetting>.Success("Api info successfully updated.", setting);
        }
    }
}
