using CSForums.Data;
using CSForums.Data.Models;
using CSForums.Service;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSForums.Tests
{
    [TestFixture]
    public class ForumServiceMethodTests
    {
        [TestCase]
        public void Create_Should_Throw_Exception()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var ctx = new ApplicationDbContext(options))
            {
                ForumService forumService = new ForumService(ctx);
                Assert.ThrowsAsync<DbUpdateException>(async () => 
                {
                    await forumService.Create(new Forum
                    {
                        Title = "icup"
                    });
                });
            }
        }
        
    }
}
