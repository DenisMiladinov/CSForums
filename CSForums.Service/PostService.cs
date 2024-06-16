using CSForums.Data;
using CSForums.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CSForums.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;
        private readonly IApplicationUser _user;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostService(ApplicationDbContext context, IApplicationUser user, UserManager<ApplicationUser> userManager)
        { 
            _context = context;
            _user = user;
            _userManager = userManager;
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task AddReply(PostReply reply)
        {
            _context.PostReplies.Add(reply);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id, string userId)
        {
            if (!await _user.ExistsAsync(userId))
                throw new ArgumentOutOfRangeException();

            bool isAdmin = await _user.IsAdminAsync(userId);
            Post? post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id && (x.User.Id == userId || isAdmin));

            if(post == null)
                throw new ArgumentOutOfRangeException();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReply(int replyId, string userId)
        {
            if (!await _user.ExistsAsync(userId))
                throw new ArgumentOutOfRangeException();

            bool isAdmin = await _user.IsAdminAsync(userId);
            PostReply? postReply = await _context.PostReplies.FirstOrDefaultAsync(x => x.Id == replyId && (x.User.Id == userId || isAdmin ));

            if(postReply == null)
                throw new ArgumentOutOfRangeException();

            _context.PostReplies.Remove(postReply);
            await _context.SaveChangesAsync();
        }

        public async Task EditPostContent(int id, string userId, string title, string newContent)
        {
            if(!await  _user.ExistsAsync(userId)) throw new ArgumentOutOfRangeException();

            bool isAdmin = await _user.IsAdminAsync(userId);
            Post? post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id && (x.User.Id == userId || isAdmin));

            if(post == null)
                throw new ArgumentOutOfRangeException();

            post.Title = title;
            post.Content = newContent;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(post => post.User)
                .Include(post => post.Replies)
                    .ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies)
                    .ThenInclude(reply =>  reply.User)
                .Include(post => post.Forum)
                .First();
        }

        public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery) 
                ? forum.Posts 
                : forum.Posts
                    .Where(post => post.Title.Contains(searchQuery) 
                        || post.Content.Contains(searchQuery));
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            var normalized = searchQuery.ToLower();

            return GetAll().Where(post
                =>  post.Title.ToLower().Contains(normalized) 
                || post.Content.ToLower().Contains(normalized));
        }

        public IEnumerable<Post> GetLatestPost(int n)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(n);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums
                .Where(forum => forum.Id == id).First()
                .Posts;
        }
    }
}
