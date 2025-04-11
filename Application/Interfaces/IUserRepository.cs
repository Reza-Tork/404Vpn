using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities.Bot;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<User>> AddUser(User user);
        Task<Result<User>> UpdateUser(User user);
        Task<Result<User>> DeleteUser(User user);
        Task<Result<User>> GetUserById(int userId);
        Task<Result<User>> GetUserByUserId(long userId);
        Task<Result<ICollection<User>>> GetAllUsers();
    }
}
