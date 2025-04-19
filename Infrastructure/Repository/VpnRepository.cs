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
using Telegram.Bot.Types;

namespace Infrastructure.Repository
{
    public class VpnRepository : IVpnRepository
    {
        private readonly BotDbContext dbContext;

        public VpnRepository(BotDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region Api Info
        public async Task<Result<ApiInfo>> AddApiInfo(ApiInfo apiInfo)
        {
            dbContext.ApiInfos.Add(apiInfo);
            await dbContext.SaveChangesAsync();
            return Result<ApiInfo>.Success("Api info successfully added.", apiInfo);
        }
        public async Task<Result<ApiInfo>> UpdateApiInfo(ApiInfo apiInfo)
        {
            dbContext.ApiInfos.Update(apiInfo);
            await dbContext.SaveChangesAsync();
            return Result<ApiInfo>.Success("Api info successfully updated.", apiInfo);
        }

        public async Task<Result<ApiInfo>> GetApiInfo(int apiInfoId)
        {
            var result = await dbContext.ApiInfos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == apiInfoId);
            if (result != null)
                return Result<ApiInfo>.Success("Api info founded!", result);
            return Result<ApiInfo>.Failure("No api info founded!");
        }

        public async Task<Result<ICollection<ApiInfo>>> GetAllApiInfos()
        {
            var apiInfos = await dbContext.ApiInfos.AsNoTracking().ToListAsync();
            return Result<ICollection<ApiInfo>>.Success(apiInfos);
        }

        public async Task<Result<ApiInfo>> DeleteApiInfo(int apiInfoId)
        {
            var result = await GetApiInfo(apiInfoId);
            if (result.IsSuccess)
            {
                dbContext.ApiInfos.Remove(result.Data!);
                await dbContext.SaveChangesAsync();
                return Result<ApiInfo>.Success("Api info Successfully removed!");
            }
            else
                return result;
        }
        #endregion

        #region Service
        public async Task<Result<Service>> AddService(Service service)
        {
            dbContext.Services.Add(service);
            await dbContext.SaveChangesAsync();
            return Result<Service>.Success("Successfully added.", service);
        }
        public async Task<Result<Service>> UpdateService(Service service)
        {
            dbContext.Services.Update(service);
            await dbContext.SaveChangesAsync();
            return Result<Service>.Success("Successfully updated.", service);
        }

        public async Task<Result<Service>> DeleteService(int serviceId)
        {
            var result = await GetService(serviceId);
            if (result.IsSuccess)
            {
                dbContext.Services.Remove(result.Data!);
                await dbContext.SaveChangesAsync();
                return Result<Service>.Success("Service Successfully removed!");
            }
            else
                return result;
        }

        public async Task<Result<Service>> GetService(int serviceId)
        {
            var result = await dbContext.Services.AsNoTracking().FirstOrDefaultAsync(x => x.Id == serviceId);
            if (result != null)
                return Result<Service>.Success("Service founded!", result);
            return Result<Service>.Failure("No service founded!");
        }

        public async Task<Result<ICollection<Service>>> GetAllServices()
        {
            var services = await dbContext.Services.AsNoTracking().ToListAsync();
            return Result<ICollection<Service>>.Success("Ok", services);
        }
        #endregion

        #region Subscription
        public async Task<Result<UserSubscription>> AddSubscription(UserSubscription subscription)
        {
            dbContext.UsersSubscriptions.Add(subscription);
            await dbContext.SaveChangesAsync();
            return Result<UserSubscription>.Success("Successfully added.", subscription);
        }

        public async Task<Result<UserSubscription>> UpdateSubscription(UserSubscription subscription)
        {
            dbContext.UsersSubscriptions.Update(subscription);
            await dbContext.SaveChangesAsync();
            return Result<UserSubscription>.Success("Successfully updated.", subscription);
        }

        public async Task<Result<UserSubscription>> DeleteSubscription(int subscriptionId)
        {
            var result = await GetSubscriptionById(subscriptionId);
            if (result.IsSuccess)
            {
                dbContext.UsersSubscriptions.Remove(result.Data!);
                await dbContext.SaveChangesAsync();
                return Result<UserSubscription>.Success("Subscription Successfully removed!");
            }
            else
                return result;
        }

        public async Task<Result<UserSubscription>> GetSubscriptionById(int subscriptionId)
        {
            var result = await dbContext.UsersSubscriptions
                .Include(x => x.Service)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == subscriptionId);
            if (result != null)
                return Result<UserSubscription>.Success("Service founded!", result);
            return Result<UserSubscription>.Failure("No service founded!");
        }

        public async Task<Result<ICollection<UserSubscription>>> GetSubscriptionsByUserId(int userId)
        {
            var result = await dbContext.UsersSubscriptions
                .Include(x => x.Service)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (result != null)
                return Result<ICollection<UserSubscription>>.Success("Service founded!", result);
            return Result<ICollection<UserSubscription>>.Failure("No service founded!");
        }

        public async Task<Result<ICollection<UserSubscription>>> GetAllSubscriptions(int offset)
        {
            var services = await dbContext.UsersSubscriptions
                .AsNoTracking()
                .Skip(offset)
                .Take(20)
                .ToListAsync();

            return Result<ICollection<UserSubscription>>.Success("Ok", services);
        }
        #endregion

        public async Task<Result<ICollection<MonthPlan>>> GetAllMonthPlans()
        {
            var result = await dbContext.MonthPlans
                .Include(x => x.TrafficPlans)
                .AsNoTracking()
                .ToListAsync();

            if (result != null)
                return Result<ICollection<MonthPlan>>.Success(result);
            return Result<ICollection<MonthPlan>>.Failure("No plan founded!");
        }
        public async Task<Result<MonthPlan>> GetMonthPlanById(int Id)
        {
            var plan = await dbContext.MonthPlans
                .Include(x => x.TrafficPlans)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (plan != null)
                return Result<MonthPlan>.Success(plan);

            return Result<MonthPlan>.Failure("No plan founded!");
        }
    }
}
