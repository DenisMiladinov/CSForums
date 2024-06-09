using CSForums.Data;
using CSForums.Data.Models;
using CSForums.Service;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using NUnit.Framework;

namespace CSForums.Tests
{
    [TestFixture]
    public class Post_Service_Should
    {
        [TestCase("coffee", 3)]
        [TestCase("tea", 1)]
        [TestCase("water", 0)]
        public void Return_Filtered_Results_Corresponding_To_Query(string query, int expected)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            
            using (var ctx = new ApplicationDbContext(options))
            {
                ctx.Forums.Add(new Forum
                {
                    Id = 19,
                    Title = "Foo",
                    Description = "Bar",
                    ImageUrl = "image",                    
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 23532,
                    Title = "First Post",
                    Content = "Coffee",
                    User = new ApplicationUser
                    {
                        UserName = "Foo",
                        Email = "Foo",
                        NormalizedEmail = "Foo",
                        NormalizedUserName = "Foo",
                        PasswordHash = "Foo",
                    }
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = -111,
                    Title = "Coffee",
                    Content = "Some Content",
                    User = new ApplicationUser
                    {
                        UserName = "Foo",
                        Email = "Foo",
                        NormalizedEmail = "Foo",
                        NormalizedUserName = "Foo",
                        PasswordHash = "Foo",
                    }
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 889,
                    Title = "Tea",
                    Content = "Coffee",
                    User = new ApplicationUser
                    {
                        UserName = "Foo",
                        Email = "Foo",
                        NormalizedEmail = "Foo",
                        NormalizedUserName = "Foo",
                        PasswordHash = "Foo",
                    }
                });

                ctx.SaveChanges();
            }

            using (var ctx = new ApplicationDbContext(options))
            {
                var postService = new PostService(ctx);
                var result = postService.GetFilteredPosts(query);
                var postCount = result.ToList().Count();

                Assert.That(expected, Is.EqualTo(postCount));
            }
        }
    }
}
