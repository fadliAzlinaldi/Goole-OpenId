using Goole_OpenId.Models;

namespace Goole_OpenId.Data
{
    public interface IUserService
    {
        Task RegisterUserAsync(User user);
    }
}
