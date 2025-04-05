using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain.Entities.Bot;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<Result<Domain.Entities.Bot.User>> CheckUserExists(Telegram.Bot.Types.User user)
        {

            var dbUser = await userRepository.GetUserById(user.Id);
            if (dbUser.IsSuccess)
                return dbUser;

            var addResult = await userRepository.AddUser(new Domain.Entities.Bot.User()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Step = Step.None,
                Username = user.Username,
                Wallet = new Wallet()
                {
                    Balance = 0
                }
            });
            return addResult;
        }

        public async Task<Result<Domain.Entities.Bot.User>> UpdateUser(Domain.Entities.Bot.User user)
        {
            return await userRepository.UpdateUser(user);
        }

        public async Task<Result<Domain.Entities.Bot.User>> DeleteUser(long userId)
        {
            return await userRepository.DeleteUser((await userRepository.GetUserById(userId)).Data);
        }
    }
}
