using CSForums.Data.Models;
using System;

namespace CSForums.Data
{
    public interface IForum
    {
        Forum GetById (int id);
        IEnumerable<Forum> GetAll();
        IEnumerable<ApplicationUser> GetAllActivities();

        Task Create(Forum forum);
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
    }
}
