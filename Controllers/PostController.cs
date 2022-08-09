using Microsoft.AspNetCore.Mvc;
using MvcForum.Models;
using System.Diagnostics;
using MvcForum.Data;
using MvcForum.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MvcForum.Controllers
{
    public class PostController : Controller
    {

        private readonly PostContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public PostController(PostContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _config = configuration;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}




        //Change to support multiple files
        //change request size limit to correct size
        [DisableRequestSizeLimit]
        [Route("/Post/AddPost")]
        public async Task<IActionResult> NewPost(IFormFile file)
        {


            //Gets board from request url (logic handles index and thread urls)

            string Board;
            bool isOP;
            string url = HttpContext.Request.Form["current_url"].ToString();
            int ThreadId = 0;

            if (url.Count(f => f == '/') > 1)
            {

                //Console.WriteLine(url.Substring(1));

                //Board = url.Substring(1, url.IndexOf('/', url.IndexOf('/'))).ToLower();

                //Console.WriteLine("index of second '/':");
                int SecondSlash = url.IndexOf('/', url.IndexOf('/') + 1);
                Board = url.Substring(1, SecondSlash - 1).ToLower();

                Console.WriteLine($"Board: {Board}");

                //is the second slash the last char in the url? 
                Console.WriteLine($"url length: {url.Length}");
                Console.WriteLine($"second slash index: {SecondSlash}");

                if (url.Length > SecondSlash + 1)
                {

                    isOP = false;
                }
                else
                {
                    isOP = true;
                }

                if (!isOP)
                {
                    ThreadId = Int32.Parse(url.Substring(SecondSlash + 1).Trim('/'));
                    Console.WriteLine($"ThreadID: {ThreadId}");
                }


            }
            else
            {
                isOP = true;
                Board = url.Substring(1).ToLower();
            }

            //if op-post, check it has file
            if (isOP)
            {
                if (file == null)
                {
                    Console.WriteLine("New threads must have a file");
                    return View("~/Views/Shared/Error.cshtml");
                }
            }


            //var headers = Request.Headers;

            //var settings = _config["CustomConfig"];

            int MaxThreads = Int32.Parse(_config["CustomConfig:MaxThreadsPerBoard"]);




            string ip = HttpContext.Connection.RemoteIpAddress.ToString();




            bool HasFile = false;

            string userFileName = null;
            string storageFileName = null;
            //string extension = null;
            string ThumbFileName = null;
            

            UploadFile uploadFile = null;

            if (file != null) {

                HasFile = true;

                uploadFile = new UploadFile();
                //Validate file type

                userFileName = file.FileName;

                

                string extension = Path.GetExtension(file.FileName);


                //Expend to other filetypes
                string[] ValidExtensions = { ".png", ".jpg", ".gif", ".jpeg", ".bmp", ".webp" };

                string[] ImageExtensions = { ".png", ".jpg", ".gif", ".jpeg", ".bmp", ".webp" };

                if (!ValidExtensions.Contains(extension)) {

                    Console.WriteLine($"Filetype: {extension}");

                    Console.WriteLine("Invalid filetype submitted");

                    //Replace this with a custom error view
                    return View("~/Views/Shared/Error.cshtml");
                }


                //string storageFileName = $@"{Guid.NewGuid().ToString()}{extension}";
                storageFileName = Guid.NewGuid().ToString() + extension;

                string storagePath = Path.Combine(_env.WebRootPath, "images", storageFileName);

                using (var stream = new FileStream(storagePath, FileMode.Create))
                {
                    try
                    {
                        await file.CopyToAsync(stream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        //Replace this with a custom error view
                        return View("~/Views/Shared/Error.cshtml");

                    }
                }


                //If image, make thumbnail with max dimensions 250x250

                uploadFile.UserFileName = userFileName;
                uploadFile.Extension = extension;
                uploadFile.FullFileName = Path.GetFileNameWithoutExtension(storageFileName);

                if (ImageExtensions.Contains(extension))
                {
                    ThumbFileName = Path.GetFileNameWithoutExtension(storageFileName) + "_thumb" + extension;
                    string ThumbPath = Path.Combine(_env.WebRootPath, "images", ThumbFileName);

                    try
                    {
                        Utility.CreateThumbnail(storagePath, ThumbPath);
                        uploadFile.ThumbFileName = Path.GetFileNameWithoutExtension(storageFileName) + "_thumb";

                    } catch (Exception e)
                    {
                        Console.WriteLine("Image processing error");
                        Console.WriteLine(e);
                        Console.WriteLine(e.Message);
                        //Replace this with a custom error view
                        return View("~/Views/Shared/Error.cshtml");
                    }

                }   



                Console.WriteLine(userFileName);

                Console.WriteLine(storageFileName);

                Console.WriteLine(storagePath);


                

                

            } else
            {
                Console.WriteLine("no file submitted");
            }

            Console.WriteLine($"User IP: {ip}");


            

            //process post text to preserve line breaks
            string PostText = HttpContext.Request.Form["PostText"].ToString();
            PostText = PostText.Replace(Environment.NewLine, "<br/>");

            Console.WriteLine(HttpContext.Request.Form["Subject"].ToString());
            Console.WriteLine(HttpContext.Request.Form["PostText"].ToString());

            bool sage = false;


            if (HttpContext.Request.Form["SageBox"].ToString().Equals("on"))
            {
                sage = true;
            }

            Console.WriteLine($"Sage is: {sage.ToString()}");
            
            Console.WriteLine(".....");
            Console.WriteLine(PostText);
            Console.WriteLine($"url: {url}");






            

            Console.WriteLine($"Board: {Board}");

            int LastPostId = 0;

                //returns null by default, causing exception
                LastPostId = _context.Post.Where(x => x.Board.Equals(Board))
                             .OrderByDescending(x => x.Id)
                             .FirstOrDefault().Id;

                if (LastPostId == null)
                {
                    LastPostId = 0;
                }

            Console.WriteLine($"Last post ID: {LastPostId}");
            Console.WriteLine($"IsOP: {isOP}");


            //int ThreadId;
            if (isOP)
            {
                ThreadId = LastPostId + 1;
            }
            Console.WriteLine(ThreadId);

            if (ThreadId == 0)
            {
                Console.WriteLine("ThreadID is still 0");

                throw new Exception();
            }


            Post NewPost = new Post
            {
                Subject = HttpContext.Request.Form["Subject"].ToString(),
                Timestamp = DateTime.Now,
                IsOP = isOP,
                Sage = sage,
                Text = PostText,
                Board = Board,
                IdOnBoard = LastPostId + 1,
                UserIP = ip,
                ThreadId = ThreadId,
                //For now we only support images
                HasImage = HasFile,
                Files = new List<UploadFile>()

            };

            if (uploadFile is not null)
            {
                uploadFile.Post = NewPost;
                NewPost.Files.Add(uploadFile);
               _context.Files.Add(uploadFile);
            }

            _context.Post.Add(NewPost);

            Console.WriteLine($"threadID: {ThreadId}");


            //logic to purge an old thread if thread limit hit

            bool ThreadLimitReached = false;
            int Threads = 0;


            Threads = _context.Post.Where(x => x.Board.Equals(Board)).Select(x => x.ThreadId == ThreadId).Count();
            Console.WriteLine($"Current threads: {Threads}");
            

            int? OldestThreadId;

            if (Threads >= MaxThreads)
            {
                if (isOP)
                {
                    //get thread that was bumped the longest time ago

                    var posts = _context.Post.Where(x => x.Board.Equals(Board))
                                .Where(x => x.Sage == false);

                    Post OldestPost = posts.First();
                    foreach (Post post in posts)
                    {
                        if (post.Timestamp < OldestPost.Timestamp)
                        {
                            OldestPost = post;
                        }
                    }


                    OldestThreadId = OldestPost.ThreadId;

                    //Delete files and entries in file dbset associated with the thread
                    var ThreadPosts = posts.Where(x => x.ThreadId == OldestThreadId);

                    foreach (Post post in ThreadPosts)
                    {
                        if (post.HasImage)
                        {
                            foreach (UploadFile FileToDelete in post.Files)
                            {
                                string PathToDelete = Path.Combine(_env.WebRootPath, "images", FileToDelete.FullFileName);
                                System.IO.File.Delete(PathToDelete);

                                if (FileToDelete.ThumbFileName is not null)
                                {
                                    PathToDelete = Path.Combine(_env.WebRootPath, "images", FileToDelete.ThumbFileName);
                                    System.IO.File.Delete(PathToDelete);
                                }

                                _context.Files.Remove(FileToDelete);

                                
                            }
                        }
                        _context.Remove(post);
                    }

                    
                }
            }


            _context.SaveChanges();



            return Redirect($"/{Board}/{ThreadId}");

            
        }


    }
}
