using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Helpers;
using Application.Interfaces;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Domain.Entities.Vpn;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.Net.Http;
using Domain.Entities.Bot;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Services
{
    public class VpnService : IVpnService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IBotService _botService;
        private readonly IVpnRepository _vpnRepository;
        private readonly ILogger<VpnService> logger;

        public VpnService(IHttpClientFactory httpClientFactory, IBotService botService, IVpnRepository vpnRepository, ILogger<VpnService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _botService = botService;
            _vpnRepository = vpnRepository;
            this.logger = logger;
        }
        public async Task<Result<LoginResponseDTO>> SignInAdmin(LoginRequestDTO loginRequestDTO)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            var domainResult = await _botService.GetSetting("DOMAIN");
            if (!domainResult.IsSuccess || domainResult.Data == null)
                return Result<LoginResponseDTO>.Failure(domainResult.Message ?? "Failed to get default domain!");

            _httpClient.BaseAddress = new Uri(domainResult.Data.Value);
            var loginResult = await _httpClient.PostAsync("/api/admin/token", new StringContent(loginRequestDTO.ConvertToFormUrlEncoded(), Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (loginResult.IsSuccessStatusCode)
            {
                var loginResponseDTO = await loginResult.Content.ReadFromJsonAsync<LoginResponseDTO>();
                await AddApiInfo(new ApiInfo()
                {
                    Username = loginRequestDTO.username,
                    Password = loginRequestDTO.password,
                    Token = loginResponseDTO!.Token
                });
                return Result<LoginResponseDTO>.Success("Success", loginResponseDTO);
            }
            else
            {
                var loginResponseDTO = await loginResult.Content.ReadFromJsonAsync<LoginResponseErrorDTO>();
                return Result<LoginResponseDTO>.Failure(loginResponseDTO != null ? loginResponseDTO.Message : "An error has occured!");
            }
        }

        public async Task<Result<ApiInfo>> GetApiInfo()
        {
            var allApiInfos = await _vpnRepository.GetAllApiInfos();
            var apiInfoList = allApiInfos.Data;

            var needsLogin = !allApiInfos.IsSuccess || apiInfoList is null || apiInfoList.Count < 1
                             || (DateTime.UtcNow - apiInfoList.OrderBy(x => x.CreateDate).Last().CreateDate).TotalHours > 12;

            if (needsLogin)
            {
                var loginResult = await TryLoginFromSetting();
                return loginResult.IsSuccess
                    ? Result<ApiInfo>.Success(loginResult.Data!)
                    : Result<ApiInfo>.Failure(loginResult.Message!);
            }

            var lastApiInfo = apiInfoList.OrderBy(x => x.CreateDate).Last();
            return Result<ApiInfo>.Success("Ok", lastApiInfo);
        }

        private async Task<Result<ApiInfo>> TryLoginFromSetting()
        {
            var loginDataResult = await _botService.GetSetting("LOGIN_INFO");
            if (loginDataResult.Data is null)
                return Result<ApiInfo>.Failure("No login info found!");

            var credentials = loginDataResult.Data.Value.Split(':');
            if (credentials.Length < 2)
                return Result<ApiInfo>.Failure("Invalid login info format!");

            var loginRequest = new LoginRequestDTO
            {
                username = credentials[0],
                password = credentials[1]
            };

            var loginResponse = await SignInAdmin(loginRequest);
            if (!loginResponse.IsSuccess || loginResponse.Data is null)
                return Result<ApiInfo>.Failure("Can't login with this credential settings!");

            return Result<ApiInfo>.Success(new ApiInfo
            {
                Username = credentials[0],
                Password = credentials[1],
                Token = loginResponse.Data.Token
            });
        }

        public async Task<Result<ApiInfo>> AddApiInfo(ApiInfo apiInfo)
        {
            var allApiInfos = await _vpnRepository.GetAllApiInfos();
            if (allApiInfos.IsSuccess && allApiInfos.Data != null && allApiInfos.Data.Any(x => x.Username == apiInfo.Username))
            {
                var result = await _vpnRepository.UpdateApiInfo(apiInfo);
                return result;
            }
            return await _vpnRepository.AddApiInfo(apiInfo);
        }

        public async Task<Result<AddUserResponseDTO>> AddSubscription(int userId, int serviceId, int days, int bandwidth, string[] tags)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            var domainResult = await _botService.GetSetting("DOMAIN");
            if (!domainResult.IsSuccess || domainResult.Data == null)
                return Result<AddUserResponseDTO>.Failure(domainResult.Message ?? "Failed to get default domain!");

            var apiInfoResult = await GetApiInfo();
            if (!apiInfoResult.IsSuccess || apiInfoResult.Data == null)
                return Result<AddUserResponseDTO>.Failure(apiInfoResult.Message ?? "Failed to get default api info!");

            _httpClient.BaseAddress = new Uri(domainResult.Data.Value);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiInfoResult.Data.Token);

            long bytes = VpnHelpers.GbToByte(bandwidth);

            var requestDto = new AddUserRequestDTO()
            {
                data_limit = bytes,
                username = StringHelpers.GenerateUsername(),
                expire = DateTime.UtcNow.AddDays(days).ToTimestamp(),
                inbounds = new AddUserRequestDTO.Inbounds()
                {
                    vless = tags
                }
            };

            var addSubResult = await _httpClient.PostAsJsonAsync($"/api/user", requestDto);
            if (addSubResult.IsSuccessStatusCode)
            {
                var addSubResponse = await addSubResult.Content.ReadFromJsonAsync<AddUserResponseDTO>();

                logger.LogInformation("Subscription successfully created!");

                return Result<AddUserResponseDTO>.Success("Ok", addSubResponse);
            }
            else if (addSubResult.StatusCode == System.Net.HttpStatusCode.UnprocessableContent)
            {
                logger.LogInformation(await addSubResult.Content.ReadAsStringAsync());
                return Result<AddUserResponseDTO>.Failure("Unprocessable Content!");
            }
            else
            {
                logger.LogInformation(await addSubResult.Content.ReadAsStringAsync());
                return Result<AddUserResponseDTO>.Failure("Authentication Error!");
            }
        }
        public async Task<Result<UserSubscription>> UpdateSubscription(UserSubscription userSubscription)
        {
            return await _vpnRepository.UpdateSubscription(userSubscription);
        }
        public async Task<Result<UserSubscription>> GetSubscription(int subId)
        {
            return await _vpnRepository.GetSubscriptionById(subId);
        }

        public async Task<Result<ICollection<UserSubscription>>> GetUserSubscriptions(int userId)
        {
            return await _vpnRepository.GetSubscriptionsByUserId(userId);
        }
        public async Task<Result<GetUserDetailsResponse>> GetSubscription(string username)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            var domainResult = await _botService.GetSetting("DOMAIN");
            if (!domainResult.IsSuccess || domainResult.Data == null)
                return Result<GetUserDetailsResponse>.Failure(domainResult.Message ?? "Failed to get default domain!");

            var apiInfoResult = await GetApiInfo();
            if (!apiInfoResult.IsSuccess || apiInfoResult.Data == null)
                return Result<GetUserDetailsResponse>.Failure(apiInfoResult.Message ?? "Failed to get default api info!");

            _httpClient.BaseAddress = new Uri(domainResult.Data.Value);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiInfoResult.Data.Token);

            var addSubResult = await _httpClient.GetAsync($"/api/user/{username}");
            if (addSubResult.IsSuccessStatusCode)
            {
                var addSubResponse = await addSubResult.Content.ReadFromJsonAsync<GetUserDetailsResponse>();

                return Result<GetUserDetailsResponse>.Success(addSubResponse);
            }
            else
            {
                return Result<GetUserDetailsResponse>.Failure("An error has occured");
            }
        }
        public async Task<Result<ICollection<UserSubscription>>> GetAllSubscriptions(int offset)
        {
            return await _vpnRepository.GetAllSubscriptions(offset);
        }



        public async Task<Result<Service>> AddService(Service service)
        {
            return await _vpnRepository.AddService(service);
        }

        public async Task<Result<Service>> DeleteService(int serviceId)
        {
            return await _vpnRepository.DeleteService(serviceId);
        }

        public async Task<Result<Service>> UpdateService(Service service)
        {
            return await _vpnRepository.UpdateService(service);
        }

        public async Task<Result<Service>> GetService(int serviceId)
        {
            return await _vpnRepository.GetService(serviceId);
        }

        public async Task<Result<ICollection<Service>>> GetAllServices()
        {
            return await _vpnRepository.GetAllServices();
        }

        public async Task<Result<ICollection<MonthPlan>>> GetAllMonthPlans()
        {
            return await _vpnRepository.GetAllMonthPlans();
        }
        public async Task<Result<MonthPlan>> GetMonthPlanById(int Id)
        {
            return await _vpnRepository.GetMonthPlanById(Id);
        }
    }
}
