using Goole_OpenId.Data;
using Goole_OpenId.Dtos;
using Goole_OpenId.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace Goole_OpenId.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly IUserService _userService;

        public AccountsController(IUserRepo userRepo, IRoleRepo roleRepo, IUserService userService)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _userService = userService;
        }

        [HttpPost("/Register")]
        public async Task<string> Register(RegisterDto user)
        {
            try
            {
                // tambah user
                var u = new User
                {
                    Username = user.Username,
                    Password = BC.HashPassword(user.Password),
                    Fullname = user.Fullname,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Address = user.Address,
                    City = user.City
                };

                // tambah user dan role ke dalam database
                await _userService.RegisterUserAsync(u);
                return "succes";
            }
            catch (Exception ex)
            {
                return "failed register";
            }
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var userToken = await _userService.LoginUserAsync(login);
            if (userToken.Token != null)
            {
                return Ok(userToken);
            }
            else
            {
                return Unauthorized(userToken);
            }
        }

        //[HttpPut("update")]
        //public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateDto)
        //{
        //    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    bool result = await _userRepo.UpdateProfileAsync(userId, updateDto);

        //    if (result)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest("Gagal memperbarui profil pengguna.");
        //    }
        //}
    }
}
