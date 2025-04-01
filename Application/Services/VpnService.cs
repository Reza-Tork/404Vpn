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

namespace Application.Services
{
    public class VpnService : IVpnService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IBotService _botService;
        private readonly IVpnRepository _vpnRepository;

        public VpnService(IHttpClientFactory httpClientFactory, IBotService botService, IVpnRepository vpnRepository)
        {
            _httpClientFactory = httpClientFactory;
            _botService = botService;
            _vpnRepository = vpnRepository;
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

        public async Task<Result<AddUserResponseDTO>> AddSubscription(int userId, int serviceId, int days, int bandwidth)
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
            var addSubResult = await _httpClient.PostAsJsonAsync($"/api/user", new AddUserRequestDTO()
            {
                Bandwidth = VpnHelpers.GbToByte(bandwidth),
                username = StringHelpers.GenerateUsername(),
                ExpireTimestamp = DateTime.UtcNow.AddDays(days).ToTimestamp(),
            });
            if (addSubResult.IsSuccessStatusCode)
            {
                var addSubResponse = await addSubResult.Content.ReadFromJsonAsync<AddUserResponseDTO>();
                return Result<AddUserResponseDTO>.Success("Ok", addSubResponse);
            }
            else if (addSubResult.StatusCode == System.Net.HttpStatusCode.UnprocessableContent)
            {
                return Result<AddUserResponseDTO>.Failure("Unprocessable Content!");
            }
            else
            {
                return Result<AddUserResponseDTO>.Failure("Authentication Error!");
            }
        }
        public async Task<Result<ApiInfo>> GetApiInfo()
        {
            return Result<ApiInfo>.Success("Ok", (await _vpnRepository.GetAllApiInfos()).Data!.OrderBy(x => x.CreateDate).Last());
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

        public Task<Result<UserSubscription>> GetSubscription(int subId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<UserSubscription>>> GetUserSubscriptions(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<UserSubscription>>> GetAllSubscriptions(int offset)
        {
            throw new NotImplementedException();
        }
    }
}
