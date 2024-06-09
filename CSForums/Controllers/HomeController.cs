using CSForums.Data;
using CSForums.Data.Models;
using CSForums.Models;
using CSForums.Models.Forum;
using CSForums.Models.Home;
using CSForums.Models.Post;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CSForums.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost _postService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IPost postService, ILogger<HomeController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode is not null)
            {
                switch (statusCode)
                {
                    case 404:
                    case 500:
                        return View($"Errors/Error{statusCode}");
                }
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private HomeIndexModel BuildHomeIndexModel()
        {
            var lastestPost = _postService.GetLatestPost(10);

            var posts = lastestPost.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post)
            });

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
        }

        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel 
            {
                Id = forum.Id,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}
