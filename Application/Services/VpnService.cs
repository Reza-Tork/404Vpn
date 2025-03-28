using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Domain.Entities.Vpn;

namespace Application.Services
{
    public class VpnService : IVpnService
    {
        public async Task<Result<LoginResponseDTO>> SignInAdmin(LoginRequestDTO loginRequestDTO)
        {
            //Impliment login


            return Result<LoginResponseDTO>.Success("Ok", new LoginResponseDTO() { Token = "" });
        }

        public Task<Result<ApiInfo>> GetApiInfo()
        {

        }
    }
}
