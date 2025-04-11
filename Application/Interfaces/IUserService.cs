using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities.Bot;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> CheckUserExists(Telegram.Bot.Types.User user);
        Task<Result<User>> GetUserById(int userId);
        Task<Result<User>> UpdateUser(User user);
        Task<Result<User>> DeleteUser(long userId);
        
    }
}
