﻿using Goole_OpenId.Dtos;
using Goole_OpenId.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace Goole_OpenId.Data
{
    public class UserRepo : IUserRepo
    {
        //private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;
        private readonly GooleDbContext _context;
        public UserRepo(GooleDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }
        public async Task AddUserRoleAsync(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
        }
        public async Task<IEnumerable<User>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
        public async Task<User> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(o => o.Username == username && o.IsBanned == false);

            if (user == null)
            {
                // jika user tidak ditemukan
                throw new ArgumentException($"User with username {username} not found or is banned.");
            }

            return user;
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        //public async Task<bool> UpdateProfileAsync(int id, UpdateProfileDto updateDto)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());

        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    user.Fullname = updateDto.Fullname;
        //    user.PhoneNumber = updateDto.PhoneNumber;
        //    user.Email = updateDto.Email;
        //    user.Address = updateDto.Address;
        //    user.City = updateDto.City;

        //    var result = await _userManager.UpdateAsync(user);

        //    return result.Succeeded;
        //}
    }
}
