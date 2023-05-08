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
    public class GooleController : ControllerBase
    {
        private readonly GooleDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly IUserService _userService;

        public GooleController(GooleDbContext context, IConfiguration configuration, IUserRepo userRepo, IRoleRepo roleRepo, IUserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _userService = userService;
        }
        [HttpPost]
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

                return "sukses";
            }
            catch (Exception ex)
            {
                return "gagal";
            }

            //// transaction
            //using (var trans = _userRepo.startTransactionAsync()) // startTransactionAsync
            //{
            //    try
            //    {
            //        // tambah user
            //        var u = new User
            //        {
            //            Username = user.Username,
            //            Password = BC.HashPassword(user.Password),
            //            Fullname = user.Fullname,
            //            PhoneNumber = user.PhoneNumber,
            //            Email = user.Email,
            //            Address = user.Address,
            //            City = user.City
            //        };

            //        // ambil role member
            //        var role = _roleRepo.GetRoleMemberAsync().GetAwaiter().GetResult(); // GetRoleMemberAsync
            //        // assign role ke user
            //        if (role != null)
            //        {
            //            var ur = new UserRole();
            //            ur.User = u;
            //            ur.Role = role;

            //            _context.UserRoles.Add(ur);
            //            // simpan dan commit
            //            _context.SaveChanges();
            //            trans.Commit(); // commit
            //            return "sukses";
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //    }
            //}
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            bool result = await _userRepo.UpdateProfileAsync(userId, updateDto);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Gagal memperbarui profil pengguna.");
            }
        }
    }
}
