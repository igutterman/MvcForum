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
        private readonly IConfiguration _config;
        private readonly string[] Boards;

        public IntController(PostContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _env = env;
            _config = config;

            Boards = _config.GetSection("CustomConfig:BoardList").Get<string[]>();
        }


        //Trying to turn off caching in browser
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/{board}")]
        public IActionResult Index()
        {

            foreach (string board in Boards)
            {
                Console.WriteLine(board);
            }

            //Get board from url. Use board in db query

            Console.WriteLine(Request.Path);
            string Board = Request.Path.ToString().Trim('/');

            if (!Array.Exists(Boards, element => element == Board)) {
                Console.WriteLine("Invalid board");
                return View("~/Views/Shared/Error.cshtml");
            }

            if (_context.Post is null)
            {
                return View("~/Views/Boards/Int.cshtml");
            }


            var posts = _context.Post
                        .Where(x => x.Board == Board)
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

            foreach (KeyValuePair<int, ForumThread> thread in threads.OrderByDescending(x => x.Value.getLastBumpTime()))
            {
                sortedThreads.Add(thread.Value);
            }
                
                
            foreach (ForumThread thread in sortedThreads)
            {
                thread.sortPostsByTimestamp();


            }

            var threadsView = new ForumPageViewModel(sortedThreads, _env.WebRootPath);


            return View("~/Views/Boards/Int.cshtml", threadsView);


            
        }


        [Route("/{board}/threads/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Thread()
        {

            //Get board from url. Use board in db query


            Console.WriteLine("int thread controller action");
            Console.WriteLine(Request.Path);

            string threadID = Request.Path;
            int pos = threadID.IndexOf('/', threadID.IndexOf('/') + 1);
            string Board = Request.Path.ToString().Substring(0, pos).Trim('/');


            if (!Array.Exists(Boards, element => element == Board))
            {
                Console.WriteLine("Invalid board");
                return View("~/Views/Shared/Error.cshtml");
            }

            Console.WriteLine($"Board: {Board}");
            pos = Utility.getNthIndex(threadID, '/', 3);
            threadID = threadID.Substring(pos, threadID.Length - pos).Trim('/');
            Console.WriteLine("here");
            Console.WriteLine($"ThreadID : {threadID}");

            using (_context)
            {
                var posts = _context.Post
                            .Where(post => post.Board.Equals(Board))
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

                ThreadViewModel threadView = new ThreadViewModel(thread, _env.WebRootPath, false);


                return View("~/Views/Boards/IntThread.cshtml", threadView);

            }
        }







    }
}
