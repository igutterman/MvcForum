using Microsoft.AspNetCore.Mvc;
using MvcForum.Models;
using System.Diagnostics;
using MvcForum.Data;
using MvcForum.Models;

namespace MvcForum.Controllers
{
    public class IntController : Controller
    {

        private readonly PostContext _context;

        public IntController(PostContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            using (_context)
            {

                var posts = from post in _context.Post
                            select post;

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

                foreach (KeyValuePair<int, ForumThread> thread in threads.OrderBy(x => x.Value.getLastPostTime()))
                {
                    sortedThreads.Add(thread.Value);
                }
                
                
                foreach (ForumThread thread in sortedThreads)
                {
                    thread.sortPostsByTimestamp();
                }    

                ViewData["threads"] = sortedThreads;



                return View("~/Views/Boards/Int.cshtml");

            }
            
        }

        [Route("/int/{id}")]
        public IActionResult Thread()
        {
            //ViewData["cringe"] = "You posted cringe";



            return View("~/Views/Boards/IntThread.cshtml");
        }
    }
}
