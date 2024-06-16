using CSForums.Data.Models;

namespace CSForums.Data
{
    public interface IApplicationUser
    {
        ApplicationUser? GetById (string id);
        IEnumerable<ApplicationUser> GetAll();

        Task SetProfileImage(string id, Uri uri);
        Task UpdateUserRating(string id, Type type);
        Task<bool> ExistsAsync(string id);
        Task<bool> IsAdminAsync(string id);

    }
}
