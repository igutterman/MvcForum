using Microsoft.AspNetCore.Mvc;
using MvcForum.Models;
using System.Diagnostics;
using MvcForum.Data;
using MvcForum.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MvcForum.Controllers
{
    public class IntController : Controller
    {

        private readonly PostContext _context;
        private readonly IWebHostEnvironment _env;

        public IntController(PostContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        //Trying to turn off caching in browser
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/{board}")]
        public IActionResult Index()
        {

            //Get board from url. Use board in db query

            Console.WriteLine(Request.Path);


            var posts = _context.Post
                        .Include(post => post.Files)
                        .ToList();

            //List<int> threadIDs = new List<int>();

            //List<ForumThread> threads = new List<ForumThread>();

            Dictionary<int, ForumThread> threads = new Dictionary<int, ForumThread>();



            foreach (Post post in posts)
            {
                if (!threads.ContainsKey(post.ThreadId))
                {
                    threads.Add(post.ThreadId, new ForumThread(post));
                } else
                {
                    threads[post.ThreadId].addPost(post);
                }

            }


            //threads need to be sorted by last post time

            //https://stackoverflow.com/questions/7198724/sorting-a-dictionary-by-value-which-is-an-object-with-a-datetime
                
            List<ForumThread> sortedThreads = new List<ForumThread>();

            foreach (KeyValuePair<int, ForumThread> thread in threads.OrderByDescending(x => x.Value.getLastPostTime()))
            {
                sortedThreads.Add(thread.Value);
            }
                
                
            foreach (ForumThread thread in sortedThreads)
            {
                thread.sortPostsByTimestamp();
            }

            var threadsView = new ForumPageViewModel(_env.WebRootPath)
            {
                Threads = sortedThreads
            };

            //ViewData["threads"] = sortedThreads;



            return View("~/Views/Boards/Int.cshtml", threadsView);


            
        }


        [Route("/{board}/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Thread()
        {

            //Get board from url. Use board in db query


            Console.WriteLine("int thread controller action");
            Console.WriteLine(Request.Path);

            string threadID = Request.Path;
            int pos = threadID.IndexOf('/', threadID.IndexOf('/') + 1);
            threadID = threadID.Substring(pos, threadID.Length - pos).Trim('/');
            Console.WriteLine("here");
            Console.WriteLine($"ThreadID : {threadID}");

            using (_context)
            {
                var posts = _context.Post

                            .Where(post => post.ThreadId == Int32.Parse(threadID))
                            .Include(post => post.Files)
                            .ToList();

                            //where post.ThreadId == Int32.Parse(threadID)  //route id
                            //select post;


                ForumThread thread = new ForumThread();

                foreach (Post post in posts) {
                    thread.addPost(post);
                }

                thread.sortPostsByTimestamp();

                ThreadViewModel threadView = new ThreadViewModel(_env.WebRootPath)
                {
                    Thread = thread
                };

                return View("~/Views/Boards/IntThread.cshtml", threadView);

            }
        }







    }
}
