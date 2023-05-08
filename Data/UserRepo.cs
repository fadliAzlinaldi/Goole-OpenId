using Goole_OpenId.Dtos;
using Goole_OpenId.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Goole_OpenId.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly GooleDbContext _context;
        public UserRepo(GooleDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IDbContextTransaction> startTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<bool> UpdateProfileAsync(int id, UpdateProfileDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return false;
            }

            user.Fullname = updateDto.Fullname;
            user.PhoneNumber = updateDto.PhoneNumber;
            user.Email = updateDto.Email;
            user.Address = updateDto.Address;
            user.City = updateDto.City;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
