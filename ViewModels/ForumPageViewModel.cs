using System.Collections.Generic;
using MvcForum.Models;



namespace MvcForum.ViewModels
{
    public class ForumPageViewModel
    {

        
        public List<ForumThread> Threads { get; set; }
        public string WebRootPath { get; set; }


        public ForumPageViewModel(List<ForumThread> threads, string webRootPath)
        {
            Threads = threads;
            WebRootPath = webRootPath;


        }
    }
}
