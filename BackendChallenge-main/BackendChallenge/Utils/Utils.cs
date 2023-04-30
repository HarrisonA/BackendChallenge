using System.Threading;
using System.Threading.Tasks;
using BackendChallenge.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendChallenge.Utils
{
    public static class TokenUtils
    {
        public static async Task<int?> GetUserIdFromToken(string userToken, CancellationToken token, AppDbContext db)
        {
            return await db.UserTokens
            .Where(ut => ut.Token == userToken)
            .Select(ut => ut.UserId)
            .SingleOrDefaultAsync(token);
        }
    }

    public static class UserUtils
    {
        public static async Task<int?> GetCompanyIdFromUserId(int userId, CancellationToken token, AppDbContext db)
        {
            return await db.Users
            .Where(u => u.UserId == userId)
            .Select(u => u.CompanyId)
            .FirstOrDefaultAsync(token);
        }
    }

}



