using Goole_OpenId.Dtos;
using Microsoft.EntityFrameworkCore.Storage;

namespace Goole_OpenId.Data
{
    public interface IUserRepo
    {
        Task<IDbContextTransaction> startTransactionAsync();
        Task<bool> UpdateProfileAsync(int id, UpdateProfileDto updateProfile);
    }
}
