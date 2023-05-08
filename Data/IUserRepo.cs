﻿using Goole_OpenId.Dtos;
using Goole_OpenId.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace Goole_OpenId.Data
{
    public interface IUserRepo
    {
        Task AddUserAsync(User user);
        Task AddUserRoleAsync(UserRole userRole);
        Task<bool> UpdateProfileAsync(int id, UpdateProfileDto updateProfile);
    }
}
