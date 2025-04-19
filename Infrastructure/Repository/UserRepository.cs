using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            var result = await GetUserByUserId(user.UserId);
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

        public async Task<Result<User>> GetUserById(int userId)
        {
            var result = await dbContext.Users
                .Include(x => x.Admin)
                .Include(x => x.Wallet)
                .Include(x => x.Discount)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

            if (result != null)
                return Result<User>.Success(result);
            return Result<User>.Failure("No api info founded!");
        }

        public async Task<Result<User>> GetUserByUserId(long userId)
        {
            var result = await dbContext.Users
                .Include(x => x.Admin)
                .Include(x => x.Wallet)
                .Include(x => x.Discount)
                .AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

            if (result != null)
                return Result<User>.Success(result);
            return Result<User>.Failure("No user founded!");
        }

        public async Task<Result<User>> UpdateUser(User user)
        {
            var existingUser = await dbContext.Users
        .Include(u => u.Admin)
        .Include(u => u.Wallet)
        .Include(u => u.Discount)
        .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
                return Result<User>.Failure("User not found!");

            dbContext.Entry(existingUser).CurrentValues.SetValues(user);

            UpdateNavigation(existingUser, user, x => x.Admin);
            UpdateNavigation(existingUser, user, x => x.Wallet);
            UpdateNavigation(existingUser, user, x => x.Discount);

            await dbContext.SaveChangesAsync();
            return Result<User>.Success(existingUser);
        }
        private void UpdateNavigation<TEntity, TNavigation>(
            TEntity existingEntity,
            TEntity newEntity,
            Expression<Func<TEntity, TNavigation?>> navigationProperty)
            where TEntity : class
            where TNavigation : class
        {
            var member = (navigationProperty.Body as MemberExpression)?.Member;
            if (member == null) return;

            var propertyInfo = member as PropertyInfo;
            if (propertyInfo == null) return;

            var currentValue = propertyInfo.GetValue(existingEntity);
            var newValue = propertyInfo.GetValue(newEntity);

            if (newValue == null)
                return;

            if (currentValue == null)
            {
                // Attach new value if current is null
                dbContext.Entry(newValue).State = EntityState.Added;
                propertyInfo.SetValue(existingEntity, newValue);
            }
            else
            {
                dbContext.Entry(currentValue).CurrentValues.SetValues(newValue);
            }
        }
    }
}
