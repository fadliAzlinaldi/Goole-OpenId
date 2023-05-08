using Goole_OpenId.Dtos;
using Goole_OpenId.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace Goole_OpenId.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GooleController : ControllerBase
    {
        private readonly GooleDbContext _context;
        private readonly IConfiguration _configuration;
        public GooleController(GooleDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public string Register(RegisterDto user)
        {
            // transaction
            using (var trans = _context.Database.BeginTransaction()) // startTransactionAsync
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

                    // ambil role member
                    var role = _context.Roles.Where(o => o.NameRole == "member").FirstOrDefault(); // GetRoleMemberAsync
                    // assign role ke user
                    if (role != null)
                    {
                        var ur = new UserRole();
                        ur.User = u;
                        ur.Role = role;

                        _context.UserRoles.Add(ur);
                        // simpan dan commit
                        _context.SaveChanges();
                        trans.Commit(); // commit
                        return "sukses";
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
            return "gagal";
        }
    }
}
