using Azure;
using LinkShortener.Constants;
using LinkShortener.Data;
using LinkShortener.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Repositories
{
    public class RepositoryLinks
    {
        AppDbContext _dbContext;
        public RepositoryLinks(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UnAuthorizedLinkDTO>> GetLinkData()
        {
            var list = await _dbContext.Links
                .Select (l => new UnAuthorizedLinkDTO
                {
                    ShortLink = l.ShortLink,
                    LongLink = l.LongLink
                })
                .ToListAsync();

            return list;
        }

        public async Task<List<LinkUserDTO>> GetLinkData(int userId, string role)
        {
            if (role == UserRole.Admin.ToString())
            {
                return await _dbContext.Links.Include(u => u.User)
                    .Select (u => new LinkUserDTO
                    {
                        LinkId = u.LinkId,
                        LongLink = u.LongLink,
                        ShortLink = u.ShortLink,
                        CreationDate = u.CreationDate,
                        UserId = u.UserId,
                        UserLogin = u.User.Login,
                    })
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.Links.Include(u => u.User)
                    .Select (u => new LinkUserDTO
                    {
                        LinkId = u.LinkId,
                        LongLink = u.LongLink,
                        ShortLink = u.ShortLink,
                        CreationDate = u.CreationDate,
                        UserId = u.UserId,
                        UserLogin = u.User.Login,
                    })
                    .Where(u => u.UserId == userId)
                    .ToListAsync();
            }
        }

        public async Task<bool> TryAddLinkData( int userId,
                                                string longLink,
                                                string shortLink)
        {
            if (_dbContext.Links.Any(l => l.ShortLink == shortLink))
                return false;

            Link newLink = new Link()
            {
                LongLink = longLink,
                ShortLink = shortLink,
                CreationDate = DateTime.UtcNow,
                UserId = userId
            };

            _dbContext.Add(newLink);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task DeleteLinkData(   int linkId,
                                            int userId,
                                            string role)
        {
            var linkUserId = await _dbContext.Links.SingleOrDefaultAsync(l => l.LinkId == linkId);

            if (linkUserId == null)
                throw new InvalidOperationException();

            if (linkUserId.UserId == userId || role == UserRole.Admin.ToString())
            {
                _dbContext.Remove(linkUserId);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetLinkData(string shortLink)
        {
            var link = await _dbContext.Links.FirstOrDefaultAsync(l => l.ShortLink == shortLink);

            return link?.LongLink;
        }
    }
}