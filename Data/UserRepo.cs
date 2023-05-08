using Goole_OpenId.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Goole_OpenId.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly GooleDbContext _context;
        public UserRepo(GooleDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> startTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
