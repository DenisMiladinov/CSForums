using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CSForums.Data.Models;

namespace CSForums.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {   
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override DbSet<ApplicationUser> ApplicatuonUsers { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReply> PostReplies { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostReply>().HasOne(x => x.Post).WithMany(x => x.Replies).OnDelete(DeleteBehavior.Restrict);
        }

    }
}