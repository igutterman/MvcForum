using MvcForum.Data;
using MvcForum.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcForum.Data
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PostContext(serviceProvider.GetRequiredService<DbContextOptions<PostContext>>()))
            {
                if (context == null || context.Post == null)
                {
                    throw new ArgumentNullException("Null PostContext");
                }
                
                if (context.Post.Any())
                {
                    return;
                }
                

                context.Post.AddRange(
                    new Post
                    {
                        Subject = "Subject 1",
                        Timestamp = DateTime.Now,
                        IsOP = true,
                        Sage = false,
                        Text = "OP of first thread",
                        Board = "int",
                        UserIP = "1.1.1.1",
                        ThreadId = 1,
                        HasImage = true,
                        UserFileName = "1",
                        FullImageName = "/images/1f.jpg",
                        ThumbImageName = "/images/1t.jpg"


                    },
                    new Post
                    {
                        Subject = "Subject 2",
                        Timestamp = DateTime.Now.AddSeconds(10),
                        IsOP = false,
                        Sage = false,
                        Text = "First reply to thread 1",
                        Board = "int",
                        UserIP = "1.1.1.2",
                        ThreadId = 1

                    },
                    new Post
                    {
                        Subject = "Subject 3",
                        Timestamp = DateTime.Now.AddSeconds(20),
                        IsOP = false,
                        Sage = false,
                        Text = "Second reply to thread 1",
                        Board = "int",
                        UserIP = "1.1.1.3",
                        ThreadId = 1

                    },
                    new Post
                    {
                        Subject = "Subject 4",
                        Timestamp = DateTime.Now.AddSeconds(30),
                        IsOP = false,
                        Sage = true,
                        Text = "Third reply to thread 1",
                        Board = "int",
                        UserIP = "1.1.1.4",
                        ThreadId = 1

                    },

                    new Post
                    {
                        Subject = "Subject 5",
                        Timestamp = DateTime.Now.AddSeconds(40),
                        IsOP = true,
                        Sage = false,
                        Text = "OP-post second thread",
                        Board = "int",
                        ThreadId = 2,
                        HasImage = true,
                        UserFileName = "2",
                        UserIP = "1.1.1.5",
                        FullImageName = "/images/1f.jpg",
                        ThumbImageName = "/images/1t.jpg"

                    },
                    new Post
                    {
                        Subject = "Subject 6",
                        Timestamp = DateTime.Now.AddSeconds(40),
                        IsOP = false,
                        Sage = false,
                        Text = "first (only) reply to second thread",
                        Board = "int",
                        UserIP = "1.1.1.6",
                        ThreadId = 2,

                    },
                    new Post
                    {
                        Subject = "Subject 7",
                        Timestamp = DateTime.Now.AddSeconds(50),
                        IsOP = true,
                        Sage = false,
                        Text = "OP-post third thread",
                        Board = "int",
                        ThreadId = 3,
                        HasImage = true,
                        UserFileName = "2",
                        UserIP = "1.1.1.7",
                        FullImageName = "/images/1f.jpg",
                        ThumbImageName = "/images/1t.jpg"
                    }

                );

                context.SaveChanges();

            }
        }
    }

}
