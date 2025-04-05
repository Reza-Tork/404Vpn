using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domain.Entities.Bot;
using Domain.Entities.Vpn;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BotDbContext dbContext;

        public UserRepository(BotDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Result<User>> AddUser(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return Result<User>.Success(user);
        }

        public async Task<Result<User>> DeleteUser(User user)
        {
            var result = await GetUserById(user.UserId);
            if (result.IsSuccess)
            {
                dbContext.Users.Remove(result.Data!);
                await dbContext.SaveChangesAsync();
                return Result<User>.Success("Api info Successfully removed!");
            }
            else
                return result;
        }

        public async Task<Result<ICollection<User>>> GetAllUsers()
        {
            var users = await dbContext.Users.AsNoTracking().ToListAsync();
            return Result<ICollection<User>>.Success(users);
        }

        public async Task<Result<User>> GetUserById(long userId)
        {
            var result = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            if (result != null)
                return Result<User>.Success(result);
            return Result<User>.Failure("No api info founded!");
        }

        public async Task<Result<User>> UpdateUser(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return Result<User>.Success(user);
        }
    }
}
