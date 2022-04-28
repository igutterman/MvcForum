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
    }
}
