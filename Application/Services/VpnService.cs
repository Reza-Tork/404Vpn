using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Text;
using System.Threading.Tasks;
using Application.Common;
=======
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Helpers;
>>>>>>> Initial Project
using Application.Interfaces;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Domain.Entities.Vpn;

namespace Application.Services
{
    public class VpnService : IVpnService
    {
<<<<<<< HEAD
        public async Task<Result<LoginResponseDTO>> SignInAdmin(LoginRequestDTO loginRequestDTO)
        {
            //Impliment login


            return Result<LoginResponseDTO>.Success("Ok", new LoginResponseDTO() { Token = "" });
        }

        public Task<Result<ApiInfo>> GetApiInfo()
        {

=======
        private readonly HttpClient _httpClient;
        private readonly IBotService _botService;

        public VpnService(HttpClient httpClient, IBotService botService)
        {
            _httpClient = httpClient;
            _botService = botService;
        }
        public async Task<Result<LoginResponseDTO>> SignInAdmin(LoginRequestDTO loginRequestDTO)
        {
            var domainResult = await _botService.GetSetting("DOMAIN");
            if(!domainResult.IsSuccess || domainResult.Data == null)
                return Result<LoginResponseDTO>.Failure(domainResult.Message ?? "Failed to get default domain!");

            _httpClient.BaseAddress = new Uri(domainResult.Data.Value);
            var loginResult = await _httpClient.PostAsync("/api/admin/token", new StringContent(loginRequestDTO.ConvertToFormUrlEncoded(), Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (loginResult.IsSuccessStatusCode)
            {
                var loginResponseDTO = await loginResult.Content.ReadFromJsonAsync<LoginResponseDTO>();
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
            throw new NotImplementedException();
        }

        public async Task<Result<ApiInfo>> AddApiInfo(ApiInfo apiInfo)
        {
            throw new NotImplementedException();
>>>>>>> Initial Project
        }
    }
}
