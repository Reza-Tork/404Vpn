using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD

<<<<<<< HEAD:Application/Interfaces/IBotService.cs
=======
using Application.Common;
using Domain.Entities.Bot;

>>>>>>> Initial Project
namespace Application.Interfaces
{
    public interface IBotService
    {
        Task Run();
<<<<<<< HEAD
=======
namespace Domain.DTOs.Marzban.Responses
{
    internal class AddUserResponseDTO
    {
>>>>>>> Initial Project:Domain/DTOs/Marzban/Responses/AddUserResponseDTO.cs
=======
        Task<Result<BotSetting>> GetAllSettings();
        Task<Result<BotSetting>> GetSetting(string key);
        Task<Result<BotSetting>> UpdateSetting(BotSetting setting);
>>>>>>> Initial Project
    }
}
