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
        private readonly GooleDbContext _context;
        public ProfileRepo(GooleDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> UpdateProfileAsync(UpdateProfileDto updateDto)
        {
            
        }

    }


}
