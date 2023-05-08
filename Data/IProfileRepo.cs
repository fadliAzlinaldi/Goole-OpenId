using Goole_OpenId.Dtos;
using Goole_OpenId.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goole_OpenId.Data
{
    public interface IProfileRepo
    {
        Task<bool> UpdateProfileAsync(int id, UpdateProfileDto updateProfile);
    }
}
