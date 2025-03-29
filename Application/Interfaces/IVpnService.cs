using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.DTOs.Marzban.Requests;
using Domain.DTOs.Marzban.Responses;
using Domain.Entities.Vpn;

namespace Application.Interfaces
{
    public interface IVpnService
    {
        Task<Result<LoginResponseDTO>> SignInAdmin(LoginRequestDTO loginRequestDTO);
        Task<Result<ApiInfo>> AddApiInfo(ApiInfo apiInfo);
        Task<Result<ApiInfo>> GetApiInfo();
    }
}
