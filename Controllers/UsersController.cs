using Goole_OpenId.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goole_OpenId.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        {
            _userService = userService;
        }
        [Authorize(Roles = "member")]
        [HttpPut("ChangePassword")]
        public async Task<string> ChangePassowordUser(string newPassword)
        {
            try
            {
                await _userService.UpdatePassword(newPassword);
                return "Password updated successfully";
            }
            catch (ArgumentException ex)
            {
                return $"Error: {ex.Message}";
            }
            catch (Exception)
            {
                return "Error updating password";
            }
        }
        

    }
}
