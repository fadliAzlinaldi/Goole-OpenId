using Microsoft.EntityFrameworkCore.Storage;

namespace Goole_OpenId.Data
{
    public interface IUserRepo
    {
        Task<IDbContextTransaction> startTransactionAsync();
    }
}
