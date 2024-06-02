using CSForums.Data;
using CSForums.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CSForums.Service
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;

        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Forum> GetAll() 
        {
            return _context.Forums
                .Include(forum => forum.Posts);
        }
        public Forum GetById(int id)
        {
            Forum? forum = _context.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Replies)
                        .ThenInclude(r => r.User)
                .FirstOrDefault();

            return forum;
        }

        public IEnumerable<ApplicationUser> GetAllActivities()
        {
            throw new NotImplementedException();
        }

        public async Task Create(Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

    }
}
