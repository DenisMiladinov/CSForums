using CSForums.Data.Models;

namespace CSForums.Data
{
    public interface IPost
    {
        Post GetById (int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);

        Task Add(Post post);
        Task Delete(int id);
        Task EdinPostContent(int id, string newContent);
        Task AddReply(PostReply reply);
    }
}
