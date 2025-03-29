using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain.Entities.Bot;

namespace Application.Services
{
    public class BotService : IBotService
    {
        public Task<Result<BotSetting>> GetAllSettings()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<BotSetting>> GetSetting(string key)
        {
            return Result<BotSetting>.Success("Ok", new BotSetting()
            {
                Key = "DOMAIN",
                Value = "https://library98.ir/"
            });
        }

        public Task Run()
        {
            throw new NotImplementedException();
        }

        public Task<Result<BotSetting>> UpdateSetting(BotSetting setting)
        {
            throw new NotImplementedException();
        }
    }
}
