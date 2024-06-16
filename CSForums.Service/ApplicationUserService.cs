using CSForums.Data;
using CSForums.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CSForums.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public ApplicationUser? GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public async Task UpdateUserRating(string userId, Type type)
        {
            var user = GetById(userId);
            user.Rating = CalculateUserRating(type, user.Rating);
            await _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;

            if(type == typeof(Post))
                inc = 1;

            if (type == typeof(PostReply)) 
                inc = 3;

            return userRating + inc;
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(string id)
        {
            return _context.ApplicationUsers.AnyAsync(user => user.Id == id);
        }

        public async Task<bool> IsAdminAsync(string id)
        {
            ApplicationUser? user = await _context.ApplicationUsers.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return false;

            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
