using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcForum.Models;




namespace MvcForum.Data
{
    public class PostContext : DbContext
    {
        public PostContext (DbContextOptions<PostContext> options) : base(options)
        {
        }

        public DbSet<Models.Post> Post { get; set;}

        public DbSet<Models.UploadFile> Files { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Post>()
                .HasMany(c => c.Files)
                .WithOne(e => e.Post);
        }

        
    }
}
