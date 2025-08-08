using LinkShortener.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Repositories
{
    public class RepositoryAbouts
    {
        AppDbContext _dbContext;
        public RepositoryAbouts(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetAboutData()
        {
            return await _dbContext.Abouts.Select(b => b.AboutText).SingleAsync();
        }

        public async Task ChangeAboutData(string aboutText)
        {
            About changedAbout = new About()
            {
                AboutId = 1,
                AboutText = aboutText
            };

            _dbContext.Update(changedAbout);
            await _dbContext.SaveChangesAsync();
        }
    }
}
