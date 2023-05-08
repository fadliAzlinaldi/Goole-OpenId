using Goole_OpenId.Models;

namespace Goole_OpenId.Data
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly GooleDbContext _context;

        public UserService(IUserRepo userRepo, IRoleRepo roleRepo, GooleDbContext context)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _context = context;
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
    }
}
