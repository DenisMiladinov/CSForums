﻿using CSForums.Data;
using CSForums.Data.Models;
using CSForums.Models.Post;
using CSForums.Models.Reply;
using CSForums.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForums.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
        private readonly IApplicationUser _userService;

        private readonly ILogger<PostController> _logger;
        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager, ILogger<PostController> logger, IApplicationUser userService)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            _logger.LogInformation(post.User.ProfileImageUrl);

            PostIndexModel model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                IsAuthorAdmin = IsAuthorAdmin(post.User)
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Create(int id) 
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostModel
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                AuthormName = User.Identity.Name
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(NewPostModel model) 
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var post = BuildPost(model, user);

            await _postService.Add(post);
            await _userService.UpdateUserRating(userId, typeof(Post));

            return RedirectToAction("Index", "Post", new { id = post.Id });
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum
            };
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorId = reply.User.Id,
                AuthorName = reply.User.UserName,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }
        private bool IsAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int postId) 
        {
            try 
            {
                await _postService.Delete(postId, _userManager.GetUserId(User));
                return RedirectToAction("Index", "Forum");
            }
            catch (ArgumentOutOfRangeException ex) 
            {
                return NotFound();
            }
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Edit(int postId)
        {
            Post post = _postService.GetById(postId);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return View(new EditPostModel
            {
                PostId = postId,
                Title = post.Title,
                Content = post.Content,
                AuthorName = user.UserName
            });
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Edit(EditPostModel editPostModel)
        {
            try 
            {
                await _postService.EditPostContent(editPostModel.PostId, _userManager.GetUserId(User), editPostModel.Title, editPostModel.Content);
                return RedirectToAction("Index", "Post", new { id = editPostModel.PostId });
            }
            catch(ArgumentOutOfRangeException ex) 
            {
                return NotFound();
            }
        }
    }
}
