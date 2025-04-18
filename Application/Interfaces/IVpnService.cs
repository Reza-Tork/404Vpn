﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Domain.Entities.Bot;
using Domain.Entities.Vpn;

namespace Application.Interfaces
{
    public interface IVpnService
    {
        Task<Result<LoginResponseDTO>> SignInAdmin(LoginRequestDTO loginRequestDTO);
        Task<Result<ApiInfo>> AddApiInfo(ApiInfo apiInfo);
        Task<Result<ApiInfo>> GetApiInfo();

        Task<Result<AddUserResponseDTO>> AddSubscription(int userId, int serviceId, string username, int days, int bandwidth, string[] tags);
        Task<Result<UserSubscription>> GetSubscription(int subId);
        Task<Result<GetUserDetailsResponse>> GetSubscription(string username);
        Task<Result<UserSubscription>> UpdateSubscription(UserSubscription userSubscription);
        Task<Result<ICollection<UserSubscription>>> GetUserSubscriptions(int userId);
        Task<Result<ICollection<UserSubscription>>> GetAllSubscriptions(int offset);

        Task<Result<Service>> AddService(Service service);
        Task<Result<Service>> DeleteService(int serviceId);
        Task<Result<Service>> UpdateService(Service service);
        Task<Result<Service>> GetService(int serviceId);
        Task<Result<ICollection<Service>>> GetAllServices();

        Task<Result<ICollection<MonthPlan>>> GetAllMonthPlans();
        Task<Result<MonthPlan>> GetMonthPlanById(int Id);
    }
}
