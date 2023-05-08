using Goole_OpenId.Models;
using Goole_OpenId.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Goole_OpenId.Data
{
    public class ProfileRepo : IProfileRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly GooleDbContext _context;
        public ProfileRepo(GooleDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
