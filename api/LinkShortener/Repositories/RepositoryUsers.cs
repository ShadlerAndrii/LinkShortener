using LinkShortener.Constants;
using LinkShortener.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Repositories
{
    public class RepositoryUsers
    {
        AppDbContext _dbContext;
        public RepositoryUsers(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUserData()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> TryAddUserData(  string newLogin,
                                        string newPassword,
                                        UserRole newRole)
        {
            if(_dbContext.Users.FirstOrDefaultAsync(u => u.Login == newLogin) == null)
            {
                return false;
            }

            User newUser = new User()
            {
                Login = newLogin,
                Password = newPassword,
                Role = newRole
            };

            _dbContext.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> LoginUser(  string login,
                                            string password)
        {
            var user = await _dbContext.Users
                .Where(u => (u.Login == login)
                    && (u.Password == password))
                .SingleOrDefaultAsync();

            return user;
        }
    }
}