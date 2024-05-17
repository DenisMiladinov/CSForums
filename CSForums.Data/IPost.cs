using CSForums.Data.Models;

namespace CSForums.Data
{
    public interface IPost
    {
        Post GetById (int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery);
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);
        IEnumerable<Post> GetLatestPost(int n);

        Task Add(Post post);
        Task Delete(int id);
        Task EdinPostContent(int id, string newContent);
        Task AddReply(PostReply reply);
    }
}
