﻿using Goole_OpenId.Dtos;
using Goole_OpenId.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace Goole_OpenId.Data
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly GooleDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepo userRepo, IRoleRepo roleRepo, GooleDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserToken> LoginUserAsync(LoginDto login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == login.Username && u.IsBanned == false);
            if (user != null && BC.Verify(login.Password, user.Password))
            {
                var roleNames = await _context.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.NameRole)
                    .ToListAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };

                foreach (var roleName in roleNames)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                }

                var secret = _configuration.GetValue<string>("AppSettings:Secret");
                var secretBytes = Encoding.ASCII.GetBytes(secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretBytes),
                SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new UserToken
                {
                    Token = tokenHandler.WriteToken(token),
                    ExpiredAt = tokenDescriptor.Expires?.ToString(),
                    Message = "success login",
                    Username = user.Username
                };
            }
            return new UserToken { Message = "Invalid username or password" };
        }

        public async Task RegisterUserAsync(User user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // ambil role member
                    var role = await _roleRepo.GetRoleMemberAsync();
                    if (role != null)
                    {
                        // assign role ke user
                        var ur = new UserRole();
                        ur.User = user;
                        ur.Role = role;

                        // tambah user dan role ke dalam database
                        await _userRepo.AddUserAsync(user);
                        await _userRepo.AddUserRoleAsync(ur);

                        // simpan dan commit
                        await _context.SaveChangesAsync();
                        transaction.Commit(); // commit
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Gagal menambahkan user", ex);
                }
            }
        }

        public async Task UpdatePassword(string password)
        {
            // get username
            var user = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (user == null) { throw new ArgumentException("user not found"); }

            // ambil user
            var u = await _userRepo.GetUser(user);
            u.Password = BC.HashPassword(password);
            await _userRepo.UpdateUser(u);
        }

        public async Task Banned(string username)
        {
            // get username
            var user = await _userRepo.GetUser(username);
            if (user == null) { throw new ArgumentException("user not found"); }
            // ambil user
            /*var u = await _userRepo.GetUser(username);
            u.IsBanned = true;
            await _userRepo.UpdateUser(u);*/
            user.IsBanned = true;
            await _userRepo.UpdateUser(user);

        }
    }
}