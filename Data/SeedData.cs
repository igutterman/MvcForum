using MvcForum.Data;
using MvcForum.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcForum.Data
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {



            Post post1 = new Post
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
                Files = new List<UploadFile>()
            };


            Post post2 = new Post
            {
                Subject = "Subject 2",
                Timestamp = DateTime.Now.AddSeconds(10),
                IsOP = false,
                Sage = false,
                Text = "First reply to thread 1",
                Board = "int",
                UserIP = "1.1.1.2",
                ThreadId = 1,
                Files = new List<UploadFile>()
            };

            Post post3 = new Post
            {
                Subject = "Subject 3",
                Timestamp = DateTime.Now.AddSeconds(20),
                IsOP = false,
                Sage = false,
                Text = "Second reply to thread 1",
                Board = "int",
                UserIP = "1.1.1.3",
                ThreadId = 1,
                Files = new List<UploadFile>()
            };

            Post post4 = new Post
            {
                Subject = "Subject 4",
                Timestamp = DateTime.Now.AddSeconds(30),
                IsOP = false,
                Sage = true,
                Text = "Third reply to thread 1",
                Board = "int",
                UserIP = "1.1.1.4",
                ThreadId = 1,
                Files = new List<UploadFile>()
            };

            Post post5 = new Post
            {
                Subject = "Subject 5",
                Timestamp = DateTime.Now.AddSeconds(40),
                IsOP = true,
                Sage = false,
                Text = "OP-post second thread",
                Board = "int",
                ThreadId = 2,
                HasImage = true,
                UserIP = "1.1.1.5",
                Files = new List<UploadFile>()
            };

            Post post6 = new Post
            {
                Subject = "Subject 6",
                Timestamp = DateTime.Now.AddSeconds(40),
                IsOP = false,
                Sage = false,
                Text = "first (only) reply to second thread",
                Board = "int",
                UserIP = "1.1.1.6",
                ThreadId = 2,
                Files = new List<UploadFile>()
            };

            Post post7 = new Post
            {
                Subject = "Subject 7",
                Timestamp = DateTime.Now.AddSeconds(50),
                IsOP = true,
                Sage = false,
                Text = "OP-post third thread",
                Board = "int",
                ThreadId = 3,
                HasImage = true,
                UserIP = "1.1.1.7",
                Files = new List<UploadFile>()
            };

            UploadFile file1 = new UploadFile
            {
                Post = post1,
                UserFileName = "lol.jpg",
                Extension = ".jpg",
                FullFileName = "1f.jpg",
                ThumbFileName = "1t.jpg"

            };

            post1.Files.Add(file1);

            UploadFile file2 = new UploadFile
            {
                Post = post2,
                UserFileName = "lol.jpg",
                Extension = ".jpg",
                FullFileName = "1f.jpg",
                ThumbFileName = "1t.jpg"

            };

            post2.Files.Add(file2);

            UploadFile file3 = new UploadFile
            {
                Post = post5,
                UserFileName = "lol.jpg",
                Extension = ".jpg",
                FullFileName = "1f.jpg",
                ThumbFileName = "1t.jpg"

            };

            post5.Files.Add(file3);

            UploadFile file4 = new UploadFile
            {
                Post = post7,
                UserFileName = "lol.jpg",
                Extension = ".jpg",
                FullFileName = "1f.jpg",
                ThumbFileName = "1t.jpg"

            };

            post7.Files.Add(file4);



            using (var PostContext = new PostContext(serviceProvider.GetRequiredService<DbContextOptions<PostContext>>()))
            {
                if (PostContext == null || PostContext.Post == null)
                {
                    throw new ArgumentNullException("Null PostContext");
                }

                if (PostContext.Post.Any())
                {
                    return;
                }


                PostContext.Post.AddRange(
                    post1, post2, post3, post4, post5, post6, post7
                );

                PostContext.Files.AddRange(
                    file1, file2, file3, file4
                );

                PostContext.SaveChanges();

            }
        }
    }

}
