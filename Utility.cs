﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using MvcForum.Data;
using MvcForum.Models;

namespace MvcForum
{
    //static class providing methods for file processing and read/write operations
    public static class Utility
    {

        //Takes path to full-size image and path to save thumbnail to,
        //creates and saves thumbnail, returns path to thumbnail
        public static void CreateThumbnail(string FullImagePath, string ThumbnailPath)
        {

            using (var image = SixLabors.ImageSharp.Image.Load(FullImagePath))
            {


                    double Factor = Math.Min(250.0 / image.Height, 250.0 / image.Width);

                    int NewHeight = (int)(image.Height * Factor);
                    int NewWidth = (int)(image.Width * Factor);

                    image.Mutate(x => x.Resize(NewWidth, NewHeight));

                    image.Save(ThumbnailPath);

            }

        }

        public static void DeleteFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        //change this to return sorted threads
        public static void SortThreads(PostContext _context, string Board)
        {

            int OldestThreadId;

            //ThreadID : Post
            Dictionary<int, Post> ThreadIdOldestPost = new Dictionary<int, Post>();

            var AllPosts = _context.Post
                            .Where(x => x.Board.Equals(Board))
                            .Where(x => x.Sage == false);

            foreach (var Post in AllPosts)
            {
                if (!ThreadIdOldestPost.ContainsKey(Post.ThreadId))
                {
                    ThreadIdOldestPost.Add(Post.ThreadId, Post);
                }
                else
                {

                    //If the post stored for that threadID is newer than the current post in the loop,
                    //replace it with the current post in the loop (because it's older)
                    if (ThreadIdOldestPost[Post.ThreadId].Timestamp > Post.Timestamp)
                    {
                        ThreadIdOldestPost[Post.ThreadId] = Post;
                    }
                }
            }

            Post Oldest = ThreadIdOldestPost.Values.First();
            foreach (Post post in ThreadIdOldestPost.Values)
            {
                if (post.Timestamp < Oldest.Timestamp)
                {
                    Oldest = post;
                }
            }

            OldestThreadId = Oldest.ThreadId;

        }



    }
}
