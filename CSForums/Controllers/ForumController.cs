using CSForums.Data;
using CSForums.Data.Models;
using CSForums.Models.Forum;
using CSForums.Models.Post;
using CSForums.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace CSForums.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public ForumController(IForum forumService, IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            IEnumerable<ForumListingModel> forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel 
            {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
            });

            ForumIndexModel model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            Forum? forum = _forumService.GetById(id);
            IEnumerable<Post> posts = new List<Post>();

            posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();

            IEnumerable<PostListingModel> postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            ForumTopicModel model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new {id, searchQuery});
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            Forum forum = post.Forum;
            return BuildForumListing(forum);
        }
        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl,
            };
        }
    }
}
