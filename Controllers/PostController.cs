using Microsoft.AspNetCore.Mvc;
using MvcForum.Models;
using System.Diagnostics;
using MvcForum.Data;
using MvcForum.Models;
using MvcForum.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Linq;

namespace MvcForum.Controllers
{
    public class PostController : Controller
    {

        private readonly PostContext _context;
        private readonly IWebHostEnvironment _env;

        public PostController(PostContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
            //var headers = Request.Headers;

            string ip = HttpContext.Connection.RemoteIpAddress.ToString();

            bool HasFile = false;

            string userFileName = null;
            string storageFileName = null;
            //string extension = null;
            string ThumbFileName = null;
            int ThreadId = 0;

            if (file != null) {

                HasFile = true;

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

                

                if (ImageExtensions.Contains(extension))
                {
                    ThumbFileName = storageFileName + "_thumb" + extension;
                    string ThumbPath = Path.Combine(_env.WebRootPath, "images", ThumbFileName);

                    try
                    {
                        Utility.CreateThumbnail(storagePath, ThumbPath);

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


            string url = HttpContext.Request.Form["current_url"].ToString();

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



            //Gets board from request url (logic handles index and thread urls)
            
            string Board;
            bool isOP;
            
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

                if (url.Length > SecondSlash + 1) {

                    isOP = false;
                } else
                {
                    isOP = true;
                }

                if (!isOP)
                {
                    ThreadId = Int32.Parse(url.Substring(SecondSlash + 1).Trim('/'));
                    Console.WriteLine($"ThreadID: {ThreadId}");
                }
                

            } else
            {
                isOP = true;
                Board = url.Substring(1).ToLower();
            }


            

            Console.WriteLine($"Board: {Board}");

            int LastPostId = 0;
            using (_context)
            {

                //returns null by default, causing exception
                int LastPostID = _context.Post.Where(x => x.Board.Equals(Board))
                             .OrderByDescending(x => x.Id)
                             .FirstOrDefault().Id;

                if (LastPostID == null)
                {
                    LastPostID = 0;
                }

                Console.WriteLine(LastPostID);

            }
            

            //int ThreadId;
            if (isOP)
            {
                ThreadId = LastPostId + 1;
            }

            if (ThreadId == 0)
            {
                Console.WriteLine("ThreadID is still 0");
                Console.WriteLine("PostController.cs 242");
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
                UserFileName = userFileName,
                FullImageName = storageFileName,
                ThumbImageName = ThumbFileName
                

            };

            //_context.Post.Add(NewPost);

            //logic to purge an old thread if thread limit hit

            bool ThreadLimitReached = false;

            if (isOP)
            {
                
            }


            //_context.SaveChanges();



            return Ok();
        }


    }
}
