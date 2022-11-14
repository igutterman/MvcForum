using System.Collections.Generic;
using System.Xml.Linq;
using MvcForum.Models;



namespace MvcForum.ViewModels
{
    public class ForumPageViewModel
    {

        
        public List<ThreadViewModel> Threads { get; set; }
        public string WebRootPath { get; set; }


        public ForumPageViewModel(List<ForumThread> threads, string webRootPath)
        {
            WebRootPath = webRootPath;

            Threads = new List<ThreadViewModel>();

            foreach (var thread in threads)
            {
                ThreadViewModel threadView = new ThreadViewModel(thread, webRootPath, true);
                threadView.TruncateForIndex();

                Threads.Add(threadView);
            }

        }
    }
}
