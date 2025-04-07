using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities.Vpn;

namespace Application.Interfaces
{
    public interface IVpnRepository
    {
        Task<Result<ApiInfo>> AddApiInfo(ApiInfo apiInfo);
        Task<Result<ApiInfo>> DeleteApiInfo(int apiInfoId);
        Task<Result<ApiInfo>> UpdateApiInfo(ApiInfo apiInfo);
        Task<Result<ApiInfo>> GetApiInfo(int apiInfoId);
        Task<Result<ICollection<ApiInfo>>> GetAllApiInfos();

        #region Service
        Task<Result<Service>> AddService(Service service);
        Task<Result<Service>> DeleteService(int serviceId);
        Task<Result<Service>> UpdateService(Service service);
        Task<Result<Service>> GetService(int serviceId);
        Task<Result<ICollection<Service>>> GetAllServices();
        #endregion

        Task<Result<UserSubscription>> AddSubscription(UserSubscription subscription);
        Task<Result<UserSubscription>> DeleteSubscription(int subscriptionId);
        Task<Result<UserSubscription>> UpdateSubscription(UserSubscription subscription);
        Task<Result<UserSubscription>> GetSubscriptionById(int subscriptionId);
        Task<Result<ICollection<UserSubscription>>> GetSubscriptionsByUserId(int userId);
        Task<Result<ICollection<UserSubscription>>> GetAllSubscriptions(int offset);
    }
}
